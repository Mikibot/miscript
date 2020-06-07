using MiScript.Models;
using MiScript.Parser.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MiScript.Parser
{
    public class Parser
    {
        private readonly FunctionManager _functionManager;
        private readonly List<Token> _tokens;
        private int _index = 0;
        private bool _shouldStop = false;
        private Token _current;

        public Parser(List<Token> tokens, FunctionManager functionManager)
        {
            _tokens = tokens;
            _functionManager = functionManager;
            UpdateCurrent();
        }

        private bool Accept(Tokens type)
        {
            if (_current.TokenType == type)
            {
                Next();
                return true;
            }
            return false;
        }

        private bool Accept(Tokens type, out Token token)
        {
            if (_current.TokenType == type)
            {
                token = _current;
                Next();
                return true;
            }

            token = default;
            return false;
        }

        private Token Expect(Tokens type)
        {
            if (!Accept(type, out var token))
            {
                throw new Exception($"Expected '{type}' on token {_index} ({_current.Value}), but received {_current.TokenType}");
            }

            return token;
        }

        private async ValueTask<string> VarAsync(ParseContext context)
        {
            Token token;
            string value;
            
            if (Accept(Tokens.Argument, out token))
            {
                if (context.HasVariableOnStack(token.Value))
                {
                    value = context.GetVariableFromStack(token.Value);
                }
                else if (token.Value != null)
                {
                    value = context.contextVariables[token.Value].ToString();
                }
                else
                {
                    value = string.Empty;
                }
            }
            else if (Accept(Tokens.Name, out token))
            {
                value = await CallFunctionAsync(context, token.Value);
            }
            else if (Accept(Tokens.String, out token))
            {
                if (token.Value == null)
                {
                    value = string.Empty;
                }
                else if (!token.Value.Contains("$"))
                {
                    value = token.Value;
                }
                else
                {
                    value = Regex.Replace(token.Value, "\\$\\[([a-zA-Z0-9.]+)\\]", (m) =>
                    {
                        if (m.Groups.Count < 2)
                        {
                            return m.Value;
                        }

                        if (context.contextVariables
                            .TryGetValue(m.Groups[1].Value, out var variable))
                        {
                            return variable.ToString();
                        }

                        var stackVar = context.stackVariables
                            .FirstOrDefault(x => x.name == m.Groups[1].Value);
                        
                        if (stackVar.value != null)
                        {
                            return stackVar.value.ToString();
                        }

                        return "$deleted";
                    });
                }
            }
            else if (Accept(Tokens.Boolean, out token) || Accept(Tokens.Number, out token))
            {
                value = token.Value;
            }
            else
            {
                throw new Exception("Expected var!");
            }

            if (Accept(Tokens.Equals))
            {
                value = value == await VarAsync(context) ? "true" : "false";
            }
            else if (Accept(Tokens.NotEquals))
            {
                value = value != await VarAsync(context) ? "true" : "false";
            }
            else if (Accept(Tokens.Add))
            {
                value += await VarAsync(context);
            }

            return value;
        }

        private async ValueTask<bool> ExpressionAsync(ParseContext context)
        {
            return await VarAsync(context) == "true";
        }

        public async ValueTask<string> ParseAsync(Dictionary<string, object> contextVariables)
        {
            ParseContext context = new ParseContext(contextVariables);

            _index = 0;
            while (_index < _tokens.Count && !_shouldStop)
            {
                await BodyAsync(context);
            }
            return string.Join("\n", context.responses);
        }

        private async ValueTask IfStatementAsync(ParseContext context)
        {
            if (await ExpressionAsync(context))
            {
                Expect(Tokens.Then);
                context.PushStack();
                while (_current.TokenType != Tokens.End && !_shouldStop)
                {
                    await BodyAsync(context);
                }

                SkipToNext(Tokens.End);
            }
            else
            {
                SkipToNext(Tokens.Else, Tokens.End);
                if (Accept(Tokens.Else))
                {
                    context.PushStack();
                    await ElseStatementAsync(context);
                }
            }
        }

        private async ValueTask ElseStatementAsync(ParseContext context)
        {
            if (Accept(Tokens.If))
            {
                await IfStatementAsync(context);
            }
            else
            {
                while (_current.TokenType != Tokens.End && _current.TokenType != Tokens.Else && !_shouldStop)
                {
                    await BodyAsync(context);
                }
            }
        }

        private async ValueTask BodyAsync(ParseContext context)
        {
            if (Accept(Tokens.If))
            {
                await IfStatementAsync(context);
                Expect(Tokens.End);
                context.PopStack();
            }
            else if (Accept(Tokens.Var))
            {
                var token = Expect(Tokens.Name);
                var name = token.Value;
                string value = null;
                if (Accept(Tokens.Assigns))
                {
                    value = await VarAsync(context);
                }
                context.CreateStackVariable(name, value);
            }
            else if (Accept(Tokens.Name, out var token))
            {
                await CallFunctionAsync(context, token.Value);
            }
            else if (Accept(Tokens.Stop))
            {
                _shouldStop = true;
            }
            else
            {
                _shouldStop = true;
            }
        }

        private async ValueTask<string> CallFunctionAsync(ParseContext context, string name)
        {
            var function = _functionManager.GetFunctionInformation(name);
            var arguments = new string[function.Parameters.Count];

            for (var i = 0; i < arguments.Length; i++)
            {
                arguments[i] = await VarAsync(context);
            }

            return await _functionManager.InvokeAsync(name, context, arguments);
        }

        private void UpdateCurrent()
        {
            _current = _index >= 0 && _index < _tokens.Count ? _tokens[_index] : default;
        }
        
        private void Next()
        {
            _index++;
            UpdateCurrent();
        }

        private void SkipToNext(params Tokens[] token)
        {
            while(_current.TokenType != Tokens.None && token.All(x => x != _current.TokenType))
            {
                Next();
            }
        }
        
        private void SkipToNext(Tokens token)
        {
            while(_current.TokenType != Tokens.None && token != _current.TokenType)
            {
                Next();
            }
        }
    }
}

using MiScript.Models;
using MiScript.Parser.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace MiScript.Parser
{


    public class Parser
    {
        private IEnumerable<Token> _tokens;
        private int _index = 0;
        private bool _shouldStop = false;
        private Token current => _tokens.ElementAtOrDefault(_index);
        private Token previous => _tokens.ElementAtOrDefault(_index - 1);

        public Parser(IEnumerable<Token> tokens)
        {
            _tokens = tokens;
        }

        private bool Accept(Tokens type)
        {
            if (current.TokenType == type)
            {
                _index++;
                return true;
            }
            return false;
        }

        private bool Expect(Tokens type)
        {
            if (!Accept(type))
            {
                throw new Exception($"Expected '{type}' on token {_index} ({current.Value}), but received {current.TokenType}");
            }
            return true;
        }

        private string Var(ParseContext context)
        {
            if (Accept(Tokens.Argument))
            {
                if(context.HasVariableOnStack(previous.Value))
                {
                    return context.GetVariableFromStack(previous.Value);
                }
                return context.contextVariables[previous.Value].ToString();
            }

            if (Accept(Tokens.String))
            {
                return Regex.Replace(previous.Value, "\\$\\[([a-zA-Z0-9.]+)\\]", (m) =>
                {
                    if (m.Groups.Count < 2)
                    {
                        return m.Value;
                    }

                    if (context.contextVariables
                        .TryGetValue(m.Groups[1].Value, out object value))
                    {
                        return value.ToString();
                    }

                    var stackVar = context.stackVariables.FirstOrDefault(x => x.name == m.Groups[1].Value);
                    if (stackVar.value != null)
                    {
                        return stackVar.value.ToString();
                    }

                    return "$deleted";
                });
            }

            if (Accept(Tokens.Boolean) || Accept(Tokens.Number))
            {
                return previous.Value;
            }
            throw new Exception("Expected var!");
        }

        private bool Expression(ParseContext context)
        {
            string v1 = Var(context);
            if (Accept(Tokens.Equals))
            {
                return v1 == Var(context);
            }

            if (Accept(Tokens.NotEquals))
            {
                return v1 != Var(context);
            }

            throw new Exception("Expected operator!");
        }

        public string Parse(Dictionary<string, object> contextVariables)
        {
            ParseContext context = new ParseContext(contextVariables);

            _index = 0;
            while (_index < _tokens.Count() && !_shouldStop)
            {
                Body(context);
            }
            return string.Join("\n", context.responses);
        }

        private void IfStatement(ParseContext context)
        {
            if (Expression(context))
            {
                Expect(Tokens.Then);
                context.PushStack();
                while (current.TokenType != Tokens.End && !_shouldStop)
                {
                    Body(context);
                }

                SkipToNext(Tokens.End);
            }
            else
            {
                SkipToNext(Tokens.Else, Tokens.End);
                if (Accept(Tokens.Else))
                {
                    context.PushStack();
                    ElseStatement(context);
                }
            }
        }

        private void ElseStatement(ParseContext context)
        {
            if (Accept(Tokens.If))
            {
                IfStatement(context);
            }
            else
            {
                while (current.TokenType != Tokens.End && current.TokenType != Tokens.Else && !_shouldStop)
                {
                    Body(context);
                }
            }
        }

        private void Body(ParseContext context)
        {
            if (Accept(Tokens.If))
            {
                IfStatement(context);
                Expect(Tokens.End);
                context.PopStack();
            }
            else if (Accept(Tokens.Var))
            {
                Expect(Tokens.Name);
                var name = previous.Value;
                string value = null;
                if (Accept(Tokens.Assigns))
                {
                    value = Var(context);
                }
                context.CreateStackVariable(name, value);
            }
            else if (Accept(Tokens.Name))
            {
                if (previous.Value == "say")
                {
                    context.responses.Add(Var(context));
                }
            }
            else if (Accept(Tokens.Stop))
            {
                _shouldStop = true;
                return;
            }
            else
            {
                _shouldStop = true;
            }
        }

        private void SkipToNext(params Tokens[] token)
        {
            while(current.TokenType != Tokens.None && !token.Any(x => x == current.TokenType))
            {
                _index++;
            }
        }
    }
}

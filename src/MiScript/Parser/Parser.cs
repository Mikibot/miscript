using MiScript.Models;
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
                throw new Exception($"Expected '{type}' on token {_index}, but received {current.TokenType}");
            }
            return true;
        }

        private string Var(Dictionary<string, object> context)
        {
            if (Accept(Tokens.Argument))
            {
                return context[previous.Value].ToString();
            }

            if(Accept(Tokens.String))
            {
                return Regex.Replace(previous.Value, "\\$\\[([a-zA-Z0-9.]+)\\]", (m) => {
                    if(m.Groups.Count < 2)
                    {
                        return m.Value;
                    }

                    if(context.TryGetValue(m.Groups[1].Value, out object value))
                    {
                        return value.ToString();
                    }
                    return m.Value;
                });
            }

            if (Accept(Tokens.Boolean) || Accept(Tokens.Number))
            {
                return previous.Value;
            }
            throw new Exception("Expected var!");
        }

        private bool Expression(Dictionary<string, object> context)
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

        public string Parse(Dictionary<string, object> context)
        {
            _index = 0;
            string value = null;
            while (_index < _tokens.Count() && !_shouldStop)
            {
                value = Body(null, context);
            }
            return value;
        }

        private string IfStatement(string value, Dictionary<string, object> context)
        {
            if (Expression(context))
            {
                Expect(Tokens.Then);
                while (current.TokenType != Tokens.End && !_shouldStop)
                {
                    value = Body(value, context);
                }

                SkipToNext(Tokens.End);
            }
            else
            {
                SkipToNext(Tokens.Else, Tokens.End);
                if (Accept(Tokens.Else))
                {
                    value = ElseStatement(value, context);
                }
            }
            return value;
        }

        private string ElseStatement(string value, Dictionary<string, object> context)
        {
            if (Accept(Tokens.If))
            {
                value = IfStatement(value, context);
            }
            else
            {
                while (current.TokenType != Tokens.End && current.TokenType != Tokens.Else)
                {
                    value = Body(value, context);
                }
            }
            return value;
        }

        private string Body(string value, Dictionary<string, object> context)
        {
            if (Accept(Tokens.If))
            {
                value = IfStatement(value, context);
                Expect(Tokens.End);
            }
            else if (Accept(Tokens.Name))
            {
                if (previous.Value == "say")
                {
                    value = Var(context);
                }
            }
            else if(Accept(Tokens.Stop))
            {
                _shouldStop = true;
                return value;
            }
            else
            {
                _shouldStop = true;
            }
            return value;
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

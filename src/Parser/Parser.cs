using MiScript.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiScript.Parser
{
    public class Parser
    {
        private IEnumerable<Token> _tokens;
        private int _index = 0;

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

        private string Var(Dictionary<string, string> context)
        {
            if (Accept(Tokens.Argument))
            {
                return context[previous.Value];
            }

            if (Accept(Tokens.Boolean) || Accept(Tokens.Number) || Accept(Tokens.String))
            {
                return previous.Value;
            }
            throw new Exception("Expected var!");
        }

        private bool Expression(Dictionary<string, string> context)
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

        public string Parse(Dictionary<string, string> context)
        {
            _index = 0;
            return Body(null, context);
        }

        private string Body(string value, Dictionary<string, string> context)
        {
            if (Accept(Tokens.If))
            {
                if (Expression(context))
                {
                    Expect(Tokens.Then);
                    value = Body(value, context);
                    Expect(Tokens.End);
                }
                else
                {
                    do
                    {
                        _index++;
                    } while (previous.TokenType != Tokens.End);
                }
            }

            if (Accept(Tokens.Name))
            {
                if (previous.Value == "say")
                {
                    return current.Value;
                }
            }

            return value;
        }
    }
}

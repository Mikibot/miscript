using MiScript.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace MiScript
{
    public class Tokenizer
    {
        private Dictionary<string, Tokens> patterns = new Dictionary<string, Tokens>()
        {
            { "^end", Tokens.End },
            { "^if", Tokens.If },
            { "^then", Tokens.Then },
            { "^else", Tokens.Else },
            { "^=", Tokens.Equals},
            { "^!=", Tokens.NotEquals },
            { "^\"(.*?)\"", Tokens.String },
            { "^([a-zA-Z_-]+)", Tokens.Name },
            { "^([0-9]+)", Tokens.Number },
            { "^(true|false)", Tokens.Boolean },
            { "^\\$([a-zA-Z_-]+)", Tokens.Argument }
        };  

        public IEnumerable<Token> Tokenize(string script)
        {
            List<Token> tokens = new List<Token>();

            while(script.Length > 0)
            {
                bool failed = true;
                foreach(var x in patterns.Keys)
                {
                    var match = Regex.Match(script, x);
                    if (match.Success)
                    {
                        script = script.Substring(match.Length);
                        var token = new Token { TokenType = patterns[x], Value = null };
                        if (match.Groups.Count > 1)
                        {
                            token.Value = match.Groups[1].Value;
                        }
                        tokens.Add(token);
                        failed = false;
                        break;
                    }
                }

                if (failed)
                {
                    script = script.Substring(1);
                }
            }

            return tokens;
        }
    }
}

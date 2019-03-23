using MiScript.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace MiScript.Tests
{
    public class TokenizerTests
    {
        [Fact]
        public void TokenizerParse()
        {
            var script = "if $author.username = \"user\" then say \"hello\" else say \"no\" end";
            var expectedTokens = new List<Token>()
            {
                new Token { TokenType = Tokens.If },
                new Token { TokenType = Tokens.Argument, Value = "author.username" },
                new Token { TokenType = Tokens.Equals},
                new Token { TokenType = Tokens.String, Value = "user" },
                new Token { TokenType = Tokens.Then},
                new Token { TokenType = Tokens.Name, Value = "say" },
                new Token { TokenType = Tokens.String, Value = "hello" },
                new Token { TokenType = Tokens.Else},
                new Token { TokenType = Tokens.Name, Value = "say" },
                new Token { TokenType = Tokens.String, Value = "no" },
                new Token { TokenType = Tokens.End},
            };

            var tokens = new Tokenizer().Tokenize(script);

            Assert.Equal(expectedTokens.Count, tokens.Count());

            for(int i = 0; i < tokens.Count(); i++)
            {
                Assert.Equal(expectedTokens[i].TokenType, tokens.ElementAt(i).TokenType);
                Assert.Equal(expectedTokens[i].Value, tokens.ElementAt(i).Value);
            }
        }
    }
}

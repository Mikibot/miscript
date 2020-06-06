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
            const string script = "if $author.username = \"user\" then say \"hello\" else say \"no\" end";
            var expectedTokens = new List<Token>
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
                new Token { TokenType = Tokens.End },
            };

            Assert.Equal(expectedTokens, Tokenizer.Parse(script).Tokens);
        }
        
        [Fact]
        public void TokenizerNewLineString()
        {
            const string script = "say \"test\ntest\"";
            var expectedTokens = new List<Token>
            {
                new Token { TokenType = Tokens.Name, Value = "say" },
                new Token { TokenType = Tokens.String, Value = "test\ntest" }
            };

            Assert.Equal(expectedTokens, Tokenizer.Parse(script).Tokens);
        }
        
        [Fact]
        public void TokenizerTestWarning()
        {
            const string script = "\"say Hello\"";

            var warnings = Tokenizer.Parse(script).Warnings;
            
            Assert.NotEmpty(warnings);
            Assert.Equal(new SourceRange(1, 0, 1, script.Length), warnings[0].Range);
        }
    }
}

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
                new Token(Tokens.If),
                new Token(Tokens.Argument, "author.username"),
                new Token(Tokens.Equals),
                new Token(Tokens.String, "user"),
                new Token(Tokens.Then),
                new Token(Tokens.Name, "say"),
                new Token(Tokens.String, "hello"),
                new Token(Tokens.Else),
                new Token(Tokens.Name, "say"),
                new Token(Tokens.String, "no"),
                new Token(Tokens.End),
            };

            Assert.Equal(expectedTokens, Tokenizer.Parse(script).Tokens);
        }
        
        [Fact]
        public void TokenizerNewLineString()
        {
            const string script = "say \"test\ntest\"";
            var expectedTokens = new List<Token>
            {
                new Token(Tokens.Name, "say"),
                new Token(Tokens.String, "test\ntest")
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

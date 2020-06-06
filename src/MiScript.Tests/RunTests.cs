using System.Collections.Generic;
using Xunit;

namespace MiScript.Tests
{
    public class RunTests
    {
        private static string Run(string script)
        {
            var tokens = Tokenizer.Parse(script);

            return new Parser.Parser(tokens.Tokens).Parse(new Dictionary<string, object>
            {
                ["author"] = "Miki"
            });
        }
        
        [Fact]
        public void TokenizerParse() 
        {
            Assert.Equal("Hello Miki, how's life?", Run(@"say ""Hello $author, how's life?"""));
        }
    }
}
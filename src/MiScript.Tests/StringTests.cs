using Xunit;

namespace MiScript.Tests
{
    public class StringTests
    {
        [Fact]
        public void TokenizerTestWarning()
        {
            const string script = "Hello world";

            Assert.Equal("Hello world\n      ^^^^^", script.GetPeek(6, 5));
        }
        
        [Fact]
        public void TokenizerTestNewLineWarning()
        {
            const string script = "Foo\nHello world";

            Assert.Equal("Hello world\n      ^^^^^", script.GetPeek(10, 5));
        }
    }
}
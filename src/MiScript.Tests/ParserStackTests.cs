using MiScript.Parser.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace MiScript.Tests
{
    public class ParserStackTests
    {
        [Fact]
        public void CreateStackTest()
        {
            ParseContext context = new ParseContext(null);

            context.PushStack();
            context.CreateStackVariable("test", 4);
            context.PopStack();

            Assert.False(context.HasVariableOnStack("test"));
        }

        [Fact]
        public void CreateInvalidStack()
        {
            ParseContext context = new ParseContext(null);
            Assert.Throws<IndexOutOfRangeException>(() =>
            {
                context.PushStack();
                context.CreateStackVariable("test", 4);
                context.PopStack();
                context.PopStack();
                context.CreateStackVariable("test", 4);
            });
        }
    }
}

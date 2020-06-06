using System.Collections.Generic;
using System.Linq;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;

namespace MiScript
{
    internal static class AntlrExtensions
    {
        public static T Visit<T>(this ParserRuleContext context, IParseTreeVisitor<T> visitor)
        {
            return visitor.Visit(context);
        }
        
        public static IEnumerable<T> Visit<T>(this IEnumerable<ParserRuleContext> context, IParseTreeVisitor<T> visitor)
        {
            return context.Select(visitor.Visit);
        }
    }
}
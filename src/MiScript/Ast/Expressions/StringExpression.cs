using MiScript.Models;

namespace MiScript.Ast
{
    public class StringExpression : Expression
    {
        public StringExpression(SourceRange range, string value)
            : base(range)
        {
            Value = value;
        }
        
        public string Value { get; set; }
        
        public override void Compile(CompileContext context)
        {
            context.Add(Tokens.String, Value);
        }
    }
}
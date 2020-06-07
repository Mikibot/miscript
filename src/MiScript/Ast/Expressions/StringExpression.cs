using MiScript.Models;

namespace MiScript.Ast
{
    public class StringExpression : Expression
    {
        public StringExpression(SourceRange range, string value, string rawValue)
            : base(range)
        {
            Value = value;
            RawValue = rawValue;
        }
        
        public string Value { get; set; }
        
        public string RawValue { get; set; }
        
        public override void Compile(CompileContext context)
        {
            context.Add(Tokens.String, Value);
        }
    }
}
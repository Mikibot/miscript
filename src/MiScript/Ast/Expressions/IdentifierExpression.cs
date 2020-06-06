using MiScript.Models;

namespace MiScript.Ast
{
    public class IdentifierExpression : Expression
    {
        public IdentifierExpression(SourceRange range, string name) : base(range)
        {
            Name = name;
        }
        
        public string Name { get; set; }

        public override void Compile(CompileContext context)
        {
            context.Add(Tokens.Argument, Name);
        }
    }
}
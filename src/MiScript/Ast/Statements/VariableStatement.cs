using MiScript.Models;

namespace MiScript.Ast
{
    public class VariableStatement : Statement
    {
        public VariableStatement(SourceRange range, string identifier, Expression initializer = null) : base(range)
        {
            Identifier = identifier;
            Initializer = initializer;
        }
        
        public string Identifier { get; }
        
        public Expression Initializer { get; }

        public override void Compile(CompileContext context)
        {
            context.Add(Tokens.Var);
            context.Add(Tokens.Name, Identifier);

            if (Initializer != null)
            {
                context.Add(Tokens.Assigns);
                Initializer.Compile(context);
            }
        }
    }
}
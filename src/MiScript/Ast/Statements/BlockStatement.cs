using System.Collections.Generic;

namespace MiScript.Ast
{
    public class BlockStatement : Statement
    {
        public BlockStatement(SourceRange range)
            : base(range)
        {
        }
        
        public IList<Statement> Statements { get; } = new List<Statement>();
        
        public override void Compile(CompileContext context)
        {
            foreach (var statement in Statements)
            {
                statement.Compile(context);
            }
        }
    }
}
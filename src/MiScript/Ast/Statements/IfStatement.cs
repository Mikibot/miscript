using MiScript.Models;

namespace MiScript.Ast
{
    public class IfStatement : Statement
    {
        public IfStatement(SourceRange range, Expression condition, Statement thenStatement, Statement? elseStatement = null) : base(range)
        {
            ThenStatement = thenStatement;
            Condition = condition;
            ElseStatement = elseStatement;
        }
        
        public Expression Condition { get; set; }
        
        public Statement ThenStatement { get; set; }
        
        public Statement? ElseStatement { get; set; }
        
        public override void Compile(CompileContext context)
        {
            context.Add(Tokens.If);
            Condition.Compile(context);
            context.Add(Tokens.Then);
            ThenStatement.Compile(context);

            if (ElseStatement != null)
            {
                context.Add(Tokens.Else);
                ElseStatement.Compile(context);
            }

            context.Add(Tokens.End);
        }
    }
}
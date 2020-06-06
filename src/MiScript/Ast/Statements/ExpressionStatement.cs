namespace MiScript.Ast
{
    public class ExpressionStatement : Statement
    {
        public ExpressionStatement(SourceRange range, Expression expression) : base(range)
        {
            Expression = expression;
        }
        
        public Expression Expression { get; set; }

        public override void Compile(CompileContext context)
        {
            if (Expression is StringExpression stringExpression)
            {
                context.AddWarning(this, $"This string is unused, did you mean to call 'say \"{stringExpression}\"'");
            }
            
            Expression.Compile(context);
        }
    }
}
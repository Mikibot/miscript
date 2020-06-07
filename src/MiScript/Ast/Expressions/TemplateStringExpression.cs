namespace MiScript.Ast
{
    public class TemplateStringExpression : Expression
    {
        public TemplateStringExpression(SourceRange range, Expression expression, string rawValue) : base(range)
        {
            Expression = expression;
            RawValue = rawValue;
        }
        
        public Expression Expression { get; }
        
        public string RawValue { get; set; }

        public override void Compile(CompileContext context)
        {
            Expression.Compile(context);
        }
    }
}
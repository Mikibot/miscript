using Miki.Localization;

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
            switch (Expression)
            {
                case StringExpression stringExpression:
                    context.AddWarning(this, LocalizationKey.UnusedString, stringExpression.RawValue);
                    break;
                case TemplateStringExpression stringExpression:
                    context.AddWarning(this, LocalizationKey.UnusedString, stringExpression.RawValue);
                    break;
            }

            Expression.Compile(context);
        }
    }
}
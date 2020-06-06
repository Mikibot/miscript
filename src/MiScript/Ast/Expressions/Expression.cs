namespace MiScript.Ast
{
    public abstract class Expression : Node
    {
        protected Expression(SourceRange range)
            : base(range)
        {
        }
    }
}
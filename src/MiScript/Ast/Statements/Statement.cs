namespace MiScript.Ast
{
    public abstract class Statement : Node
    {
        protected Statement(SourceRange range)
            : base(range)
        {
        }
    }
}
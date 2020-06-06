namespace MiScript.Ast
{
    public abstract class Node
    {
        protected Node(SourceRange range)
        {
            Range = range;
        }

        public SourceRange Range { get; }

        public abstract void Compile(CompileContext context);
    }
}
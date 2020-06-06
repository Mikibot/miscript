namespace MiScript
{
    public class CompileWarning
    {
        public CompileWarning(SourceRange range, string message)
        {
            Range = range;
            Message = message;
        }

        public SourceRange Range { get; }
        
        public string Message { get; }
    }
}
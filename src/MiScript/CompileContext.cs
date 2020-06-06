using System.Collections.Generic;
using MiScript.Models;

namespace MiScript
{
    public class CompileContext
    {
        public List<CompileWarning> Warnings { get; } = new List<CompileWarning>();
        
        public List<Token> Tokens { get; } = new List<Token>();

        public void Add(Tokens tokenType, string value = null)
        {
            Tokens.Add(new Token(tokenType, value));
        }

        public void AddWarning(SourceRange range, string message)
        {
            Warnings.Add(new CompileWarning(range, message));
        }
    }
}
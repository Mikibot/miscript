using System;
using Miki.Localization;

namespace MiScript
{
    public readonly struct CompileWarning : IEquatable<CompileWarning>
    {
        public CompileWarning(SourceRange range, string message, string sourcePeek)
        {
            Range = range;
            Message = message;
            SourcePeek = sourcePeek;
        }

        public SourceRange Range { get; }
        
        public string Message { get; }
        
        public string SourcePeek { get; }

        public bool Equals(CompileWarning other)
        {
            return Range.Equals(other.Range) && Message == other.Message && SourcePeek == other.SourcePeek;
        }

        public override bool Equals(object obj)
        {
            return obj is CompileWarning other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Range, Message, SourcePeek);
        }

        public static bool operator ==(CompileWarning left, CompileWarning right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(CompileWarning left, CompileWarning right)
        {
            return !left.Equals(right);
        }
    }
}
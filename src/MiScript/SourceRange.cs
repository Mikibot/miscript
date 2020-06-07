using System.Text.RegularExpressions;
using Antlr4.Runtime;
using MiScript.Ast;

namespace MiScript
{
    public struct SourceRange
    {
        public static readonly SourceRange Zero = new SourceRange(0, 0, 0, 0, 0, 0);
        
        public SourceRange(int startLine, int startColumn, int endLine, int endColumn, int index, int length)
        {
            StartLine = startLine;
            StartColumn = startColumn;
            EndLine = endLine;
            EndColumn = endColumn;
            Index = index;
            Length = length;
        }
        
        public int Index { get; }
        
        public int Length { get; }

        public int StartLine { get; }

        public int StartColumn { get; }

        public int EndLine { get; }

        public int EndColumn { get; }

        public static (int stopLine, int stopColumn) CalculateStop(IToken token)
        {
            return CalculateStop(token.Line, token.Column, token.Text);
        }
        
        public static (int stopLine, int stopColumn) CalculateStop(int line, int column, string text)
        {
            var match = Regex.Matches(text, @"(\r\n|\r|\n)");

            int stopLine;
            int stopColumn;

            if (match.Count > 0)
            {
                stopLine = line + match.Count;
                stopColumn = column + text.Length - (match[^1].Index + match[^1].Length);
            }
            else
            {
                stopLine = line;
                stopColumn = column + text.Length;
            }

            return (stopLine, stopColumn);
        }

        public static implicit operator SourceRange(ParserRuleContext context)
        {
            var startLine = context.Start.Line;
            var startColumn = context.Start.Column;
            var end = context.stop ?? context.start;
            var (endLine, endColumn) = CalculateStop(end);
            
            return new SourceRange(
                startLine,
                startColumn,
                endLine,
                endColumn,
                context.Start.StartIndex,
                (end.StartIndex - context.Start.StartIndex) + end.Text.Length
            );
        }

        public static implicit operator SourceRange(Node node)
        {
            return node.Range;
        }

        public bool IsInRange(int line, int column)
        {
            return line >= StartLine && column >= StartColumn && 
                   line <= EndLine && column <= EndColumn;
        }

        public override string ToString()
        {
            return $"{StartLine}:{StartColumn} - {EndLine}:{EndColumn} (index: {Index}, length: {Length})";
        }
    }
}
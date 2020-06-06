using System;
using MiScript.Models;

namespace MiScript.Ast
{
    public enum BinaryType
    {
        Add,
        Equal,
        NotEqual
    }
    
    public class BinaryExpression : Expression
    {
        public BinaryExpression(SourceRange range, BinaryType type, Expression left, Expression right)
            : base(range)
        {
            Type = type;
            Left = left;
            Right = right;
        }
        
        public BinaryType Type { get; }
        
        public Expression Left { get; }
        
        public Expression Right { get; }

        public override void Compile(CompileContext context)
        {
            Left.Compile(context);
            
            switch (Type)
            {
                case BinaryType.Add:
                    context.Add(Tokens.Add);
                    break;
                case BinaryType.Equal:
                    context.Add(Tokens.Equals);
                    break;
                case BinaryType.NotEqual:
                    context.Add(Tokens.NotEquals);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            Right.Compile(context);
        }
    }
}
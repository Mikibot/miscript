using System;
using Antlr4.Runtime.Tree;
using MiScript.Ast;

namespace MiScript.Language
{
    public class ExpressionVisitor : MiScriptParserBaseVisitor<Expression>
    {
        public override Expression Visit(IParseTree tree)
        {
            var result = base.Visit(tree);

            if (result == null)
            {
                throw new InvalidOperationException($"The tree {tree.GetType()} is not implemented yet.");
            }

            return result;
        }

        public override Expression VisitIdentifier(MiScriptParser.IdentifierContext context)
        {
            return new IdentifierExpression(context, context.GetText());
        }

        public override Expression VisitEqualExpression(MiScriptParser.EqualExpressionContext context)
        {
            return new BinaryExpression(
                context,
                BinaryType.Equal,
                context.expression(0).Visit(this),
                context.expression(1).Visit(this));
        }

        public override Expression VisitNotEqualExpression(MiScriptParser.NotEqualExpressionContext context)
        {
            return new BinaryExpression(
                context,
                BinaryType.NotEqual,
                context.expression(0).Visit(this),
                context.expression(1).Visit(this));
        }
        
        public override Expression VisitStringExpression(MiScriptParser.StringExpressionContext context)
        {
            Expression? expression = null;
            
            foreach (var part in context.@string().stringPart())
            {
                var identifier = part.stringIdentifier()?.identifier();

                Expression current;
                
                if (identifier != null)
                {
                    current = identifier.Visit(this);
                }
                else
                {
                    current = new StringExpression(part, part.GetText(), part.GetText());
                }

                if (expression == null)
                {
                    expression = current;
                }
                else
                {
                    expression = new BinaryExpression(current, BinaryType.Add, expression, current);
                }
            }

            if (expression is StringExpression stringExpression)
            {
                return new StringExpression(context, stringExpression.Value, context.GetText());
            }

            return new TemplateStringExpression(context, expression, context.GetText());
        }
        
        public override Expression VisitCallExpression(MiScriptParser.CallExpressionContext context)
        {
            return new CallExpression(context,
                context.functionName().GetText(),
                context.expression().Visit(this));
        }
    }
}
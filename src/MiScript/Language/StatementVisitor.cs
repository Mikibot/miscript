using System;
using Antlr4.Runtime.Tree;
using MiScript.Ast;

namespace MiScript.Language
{
    public class StatementVisitor : MiScriptParserBaseVisitor<Statement>
    {
        private readonly ExpressionVisitor _expressionVisitor = new ExpressionVisitor();
        
        public override Statement Visit(IParseTree tree)
        {
            var result = base.Visit(tree);

            if (result == null)
            {
                throw new InvalidOperationException($"The tree {tree.GetType()} is not implemented yet.");
            }

            return result;
        }
        
        public override Statement VisitScript(MiScriptParser.ScriptContext context)
        {
            var program = new Program(context);

            foreach (var statementContext in context.statement())
            {
                program.Statements.Add(VisitStatement(statementContext));
            }
            
            return program;
        }

        public override Statement VisitIfStatement(MiScriptParser.IfStatementContext context)
        {
            return new IfStatement(context, 
                context.expression().Visit(_expressionVisitor),
                context.blockStatement().Visit(this),
                context.elseStatement()?.Visit(this));
        }

        public override Statement VisitCallStatement(MiScriptParser.CallStatementContext context)
        {
            return new CallStatement(context, 
                context.singleIdentifier().GetText(),
                context.expression().Visit(_expressionVisitor));
        }

        public override Statement VisitExpressionStatement(MiScriptParser.ExpressionStatementContext context)
        {
            return new ExpressionStatement(context, context.expression().Visit(_expressionVisitor));
        }
    }

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
                    current = new StringExpression(part, part.GetText());
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
            
            return expression ?? new StringExpression(context, string.Empty);
        }
    }
}
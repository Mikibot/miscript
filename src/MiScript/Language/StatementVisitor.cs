using System;
using Antlr4.Runtime.Tree;
using MiScript.Ast;

namespace MiScript.Language
{
    [CLSCompliant(false)]
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
            return new ExpressionStatement(context, new CallExpression(context, 
                context.call().functionName().GetText(),
                context.call().expression().Visit(_expressionVisitor)));
        }

        public override Statement VisitExpressionStatement(MiScriptParser.ExpressionStatementContext context)
        {
            return new ExpressionStatement(context, context.expression().Visit(_expressionVisitor));
        }

        public override Statement VisitSetStatement(MiScriptParser.SetStatementContext context)
        {
            Expression? expression;

            if (context.call() == null)
            {
                expression = context.expression()?.Visit(_expressionVisitor);
            }
            else
            {
                expression = new CallExpression(context,
                    context.call().functionName().GetText(),
                    context.call().expression().Visit(_expressionVisitor));
            }
            
            return new VariableStatement(context, context.singleIdentifier().GetText(), expression);
        }
    }
}
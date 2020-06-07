using System.Collections.Generic;
using System.Linq;
using Miki.Localization;
using MiScript.Models;

namespace MiScript.Ast
{
    public class CallExpression : Expression
    {
        public CallExpression(SourceRange range, string name, IEnumerable<Expression>? arguments = null) : base(range)
        {
            Name = name;
            Arguments = new List<Expression>(arguments ?? Enumerable.Empty<Expression>());
        }
        
        public string Name { get; }
        
        public IList<Expression> Arguments { get; }
        
        public override void Compile(CompileContext context)
        {
            Token token;
            int argumentCount;
            
            switch (Name)
            {
                case "stop":
                    context.Add(Tokens.Stop);
                    token = new Token(Tokens.Stop);
                    argumentCount = 0;
                    break;
                default:
                    token = new Token(Tokens.Name, Name);
                    var information = context.FunctionManager.GetFunctionInformation(Name);
                    argumentCount = information.Parameters.Count;
                    break;
            }


            if (Arguments.Count < argumentCount)
            {
                context.AddWarning(Arguments[^1], LocalizationKey.MissingArgument, Name, argumentCount);
                return;
            }
            
            context.Add(token);

            for (var i = 0; i < argumentCount; i++)
            {
                Arguments[i].Compile(context);
            }

            if (Arguments.Count > argumentCount)
            {
                context.AddWarning(Arguments[^1], LocalizationKey.TooManyArguments, Name, argumentCount);
            }
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using MiScript.Models;

namespace MiScript.Ast
{
    public class CallStatement : Statement
    {
        public CallStatement(SourceRange range, string name, IEnumerable<Expression> arguments = null) : base(range)
        {
            Name = name;
            Arguments = new List<Expression>(arguments ?? Enumerable.Empty<Expression>());
        }
        
        public string Name { get; }
        
        public IList<Expression> Arguments { get; }
        
        public override void Compile(CompileContext context)
        {
            int argumentCount;

            switch (Name)
            {
                case "say":
                    argumentCount = 1;
                    break;
                default:
                    context.AddWarning(this, $"The method {Name} does not exists, did you mean 'say'?");
                    return;
            }
            
            context.Add(Tokens.Name, Name);
            
            if (Arguments.Count < argumentCount)
            {
                context.AddWarning(this, $"The method {Name} takes {argumentCount}, did you forgot an argument?");
                return;
            }

            for (var i = 0; i < argumentCount; i++)
            {
                Arguments[i].Compile(context);
            }

            if (Arguments.Count > argumentCount)
            {
                context.AddWarning(this, $"The method {Name} takes {argumentCount}, did you accidentally add an argument?");
            }
        }
    }
}
using MiScript.Parser.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MiScript.Parser.Models
{
    public class ParseContext
    {
        internal Dictionary<string, object> contextVariables;
        internal Stack<StackVariable> stackVariables = new Stack<StackVariable>();
        internal HashSet<string> stackVariablesUsed = new HashSet<string>();
        internal int stackDepth = 0;
        internal List<string> responses = new List<string>();

        public ParseContext(Dictionary<string, object> c)
        {
            contextVariables = c;
        }

        public void PushStack()
        {
            UpdateStack(stackDepth + 1);
        }

        public void PopStack()
        {
            if(stackDepth == 0)
            {
                throw new IndexOutOfRangeException();
            }
            UpdateStack(stackDepth - 1);
        }

        public string GetVariableFromStack(string name)
        {
            if(!HasVariableOnStack(name))
            {
                throw new ParseException(this,
                    $"Tried to get a variable from stack of name '{name}', but it did not exist");
            }
            return stackVariables
                .First(x => x.name == name).value
                .ToString();
        }

        public bool HasVariableOnStack(string name)
            => stackVariablesUsed.Contains(name);

        public void CreateStackVariable(string name, object value)
        {
            if(stackVariablesUsed.Contains(name))
            {
                throw new ParseException(this, $"Couldn't create a variable with the same name ({name})");
            }

            stackVariablesUsed.Add(name);
            stackVariables.Push(new StackVariable
            {
                name = name,
                value = value,
                stackDepth = stackDepth
            });
        }

        private void UpdateStack(int value)
        {
            stackDepth = value;

            while (stackVariables.Count > 0
                && stackVariables.Peek().stackDepth > value)
            {
                var variable = stackVariables.Pop();
                stackVariablesUsed.Remove(variable.name);
            }
        }

    }
}
 
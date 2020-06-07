using System;
using System.Linq.Expressions;

namespace MiScript.Providers
{
    public class StringParameterProvider : IParameterProvider
    {
        public Type Type => typeof(string);
        
        public bool IsValid(string input)
        {
            return true;
        }

        public Expression GetArgument(Expression context, Expression argument)
        {
            return argument;
        }
    }
}
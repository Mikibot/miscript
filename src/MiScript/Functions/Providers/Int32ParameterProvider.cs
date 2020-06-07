using System;
using System.Linq.Expressions;
using System.Reflection;

namespace MiScript.Providers
{
    public class Int32ParameterProvider : IParameterProvider
    {
        private static readonly MethodInfo IntParse = typeof(int).GetMethod(nameof(int.Parse), BindingFlags.Static | BindingFlags.Public, null, new [] { typeof(string) }, null);
        
        public Type Type => typeof(int);
        
        public bool IsValid(string input)
        {
            return int.TryParse(input, out _);
        }

        public Expression GetArgument(Expression context, Expression argument)
        {
            return Expression.Call(IntParse, argument);
        }
    }
}
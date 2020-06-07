using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Miki.Localization;
using MiScript.Parser.Models;
using MiScript.Providers;

namespace MiScript
{
    public class FunctionInformation
    {
        private readonly Func<object, CallContext, ValueTask<string>> _invoke;
        
        public FunctionInformation(string name, Type type, IDictionary<Type, IParameterProvider> providers)
        {
            Name = name;
            Description = new LanguageResource($"function_{name}");
            (_invoke, Parameters) = Compile(name, type, providers);
        }

        public string Name { get; }
        
        public IResource Description { get; }
        
        public IReadOnlyList<FunctionParameterInformation> Parameters { get; }

        public ValueTask<string> InvokeAsync(object instance, CallContext context)
        {
            return _invoke(instance, context);
        }

        private static (Func<object, CallContext, ValueTask<string>>, List<FunctionParameterInformation>) Compile(
            string name,
            Type type,
            IDictionary<Type, IParameterProvider> providers)
        {
            var method = type.GetMethod("Invoke") ?? type.GetMethod("InvokeAsync");

            if (method == null)
            {
                throw new MissingMethodException(type.Name, "Invoke");
            }

            var parameterInfos = new List<FunctionParameterInformation>();
            var instanceParam = Expression.Parameter(typeof(object), "instance");
            var instance = Expression.Convert(instanceParam, type);
            var context = Expression.Parameter(typeof(CallContext), "context");
            var parameters = method.GetParameters();
            var parameterExpressions = new Expression[parameters.Length];
            var argIndex = 0;
            
            for (var i = 0; i < parameters.Length; i++)
            {
                var parameter = parameters[i];
                Expression expression;

                if (typeof(CallContext).IsAssignableFrom(parameter.ParameterType))
                {
                    expression = context;
                }
                else if (providers.TryGetValue(parameter.ParameterType, out var provider))
                {
                    var arguments = Expression.Property(context, nameof(CallContext.Arguments));
                    var getMethod = arguments.Type.GetMethod("get_Item") ?? throw new InvalidOperationException("Could not get the getter");
                    var argument = Expression.Call(arguments, getMethod, Expression.Constant(argIndex++));
                    expression = provider.GetArgument(context, argument);
                    parameterInfos.Add(new FunctionParameterInformation(name, parameter.Name, parameter.ParameterType));
                }
                else
                {
                    throw new InvalidOperationException($"The method {method.Name} in class {type.Name} contains a parameter (name: {parameter.Name}, type: {parameter.ParameterType}) that is not supported by the runtime");
                }

                parameterExpressions[i] = expression;
            }

            Expression call = Expression.Call(instance, method, parameterExpressions);

            if (call.Type == typeof(Task) || call.Type == typeof(Task<string>))
            {
                if (call.Type == typeof(Task))
                {
                    Expression<Func<Task, Task<string>>> convert = e => e.ContinueWith(t => string.Empty);

                    call = Expression.Invoke(convert, call);
                }
                
                var constructor = typeof(ValueTask<string>).GetConstructor(new[] {typeof(Task<string>)})
                                  ?? throw new InvalidOperationException("Could not find the ValueTask constructor");

                call = Expression.New(constructor, call);
            }
            else if (call.Type == typeof(string))
            {
                var constructor = typeof(ValueTask<string>).GetConstructor(new[] {typeof(string)})
                                  ?? throw new InvalidOperationException("Could not find the ValueTask constructor");

                call = Expression.New(constructor, call);
            }
            else if (call.Type == typeof(void))
            {
                var defaultTask = Expression.Default(typeof(ValueTask<string>));
                var returnTarget = Expression.Label(typeof(ValueTask<string>));
                var returnExpression = Expression.Return(returnTarget, defaultTask, typeof(ValueTask<string>));
                var returnLabel = Expression.Label(returnTarget, defaultTask);
                call = Expression.Block(call, returnExpression, returnLabel);
            }
            else if (call.Type != typeof(ValueTask<string>))
            {
                throw new InvalidOperationException($"The method {method.Name} in class {type.Name} has an invalid return type.");
            }
            
            var lambda = Expression.Lambda<Func<object, CallContext, ValueTask<string>>>(call, instanceParam, context);

            return (lambda.Compile(), parameterInfos);
        }
    }
}
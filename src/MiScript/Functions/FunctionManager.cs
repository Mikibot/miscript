using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MiScript.Functions;
using MiScript.Parser.Models;
using MiScript.Providers;

namespace MiScript
{
    public class FunctionManager
    {
        public static readonly FunctionManager Empty = new FunctionManager(
            Enumerable.Empty<IParameterProvider>(),
            Enumerable.Empty<IScriptFunction>()
        );
        
        private readonly ConcurrentDictionary<string, FunctionInformation> _functionInformations;
        private readonly IDictionary<string, IScriptFunction> _functions;
        private readonly IDictionary<Type, IParameterProvider> _providers;
        
        public FunctionManager(IEnumerable<IParameterProvider> providers, IEnumerable<IScriptFunction> functions)
        {
            _functionInformations = new ConcurrentDictionary<string, FunctionInformation>();
            _functions = functions.ToDictionary(p => p.Name);
            _providers = providers.ToDictionary(p => p.Type);
        }

        public FunctionInformation GetFunctionInformation(string name)
        {
            return _functionInformations.GetOrAdd(name, _ =>
            {
                if (!_functions.TryGetValue(name, out var function))
                {
                    throw new InvalidOperationException($"The function {name} does not exists.");
                }

                return new FunctionInformation(name, function.GetType(), _providers);
            });
        }

        public ValueTask<string> InvokeAsync(string name, ParseContext context, string[] arguments)
        {
            var callContext = new CallContext(context, arguments);
            var info = GetFunctionInformation(name);
            var function = _functions[name];

            return info.InvokeAsync(function, callContext);
        }
    }
}
using System.Collections.Generic;
using MiScript.Parser.Models;

namespace MiScript
{
    public readonly struct CallContext
    {
        private readonly ParseContext _parseContext;
        
        public CallContext(ParseContext parseContext, IReadOnlyList<string> arguments)
        {
            Arguments = arguments;
            _parseContext = parseContext;
        }

        public IReadOnlyList<string> Arguments { get; }

        public void AddResponse(string text)
        {
            _parseContext.responses.Add(text);
        }
    }
}
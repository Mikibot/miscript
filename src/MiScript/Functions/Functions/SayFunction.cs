using MiScript.Parser.Models;

namespace MiScript.Functions
{
    public class SayFunction : IScriptFunction
    {
        public string Name => "say";

        public void Invoke(CallContext context, string input)
        {
            context.AddResponse(input);
        }
    }
}
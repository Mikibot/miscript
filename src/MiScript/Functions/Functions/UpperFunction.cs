using MiScript.Parser.Models;

namespace MiScript.Functions
{
    public class UpperFunction : IScriptFunction
    {
        public string Name => "upper";

        public string Invoke(CallContext context, string input)
        {
            return input.ToUpper();
        }
    }
}
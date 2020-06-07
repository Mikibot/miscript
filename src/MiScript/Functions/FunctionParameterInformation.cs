using System;
using Miki.Localization;

namespace MiScript
{
    public class FunctionParameterInformation
    {
        public FunctionParameterInformation(string functionName, string name, Type type)
        {
            Name = name;
            Type = type;
            Description = new LanguageResource($"function_{functionName}_{name}");
        }

        public string Name { get; }
        
        public IResource Description { get; }
        
        public Type Type { get; }
    }
}
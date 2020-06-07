using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Miki.Localization;
using MiScript.Ast;
using MiScript.Functions;
using MiScript.Models;

namespace MiScript
{
    public class CompileContext
    {
        private readonly string _script;
        private readonly IResourceManager _resourceManager;
        
        public CompileContext(string script, FunctionManager functionManager, IResourceManager? resourceManager = null)
        {
            _script = script;
            FunctionManager = functionManager;
            _resourceManager = resourceManager ?? new ResxResourceManager(typeof(Resources));
        }

        public FunctionManager FunctionManager { get; }
        
        public List<CompileWarning> Warnings { get; } = new List<CompileWarning>();
        
        public List<Token> Tokens { get; } = new List<Token>();

        public void Add(Token token)
        {
            Tokens.Add(token);
        }
        
        public void Add(Tokens tokenType, string? value = null)
        {
            Tokens.Add(new Token(tokenType, value));
        }

        public void AddWarning(SourceRange range, string message, params object[] arguments)
        {
            var localizedMessage = new LanguageResource(message, arguments).Get(_resourceManager);
            var sourcePeek = _script.GetPeek(range.Index, range.Length);
            
            Warnings.Add(new CompileWarning(range, localizedMessage, sourcePeek));
        }
    }
}
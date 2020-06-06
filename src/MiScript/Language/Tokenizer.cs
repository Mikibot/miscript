﻿using MiScript.Models;
using System;
using System.Collections.Generic;
using Antlr4.Runtime;
using MiScript.Ast;
using MiScript.Language;

namespace MiScript
{
    public class Tokenizer
    {
        public static Node CompileNode(string code)
        {
            var listener = new StatementVisitor();
            var inputStream = new AntlrInputStream(code);
            var lexer = new MiScriptLexer(inputStream);
            var commonTokenStream = new CommonTokenStream(lexer);
            var parser = new MiScriptParser(commonTokenStream)
            {
                ErrorHandler = new BailErrorStrategy(),
            };

            return listener.VisitScript(parser.script());;
        }
        
        public static CompileContext Parse(string script)
        {
            var root = CompileNode(script);
            var context = new CompileContext();
            
            root.Compile(context);

            return context;
        }
        
        [Obsolete("Use the static method Tokenizer.Parse instead.")]
        // ReSharper disable once MemberCanBeMadeStatic.Global
        public List<Token> Tokenize(string script)
        {
            return Parse(script).Tokens;
        }
    }
}
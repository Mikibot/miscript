using System;
using System.Linq.Expressions;
using MiScript.Parser.Models;

namespace MiScript.Providers
{
    public interface IParameterProvider
    {
        /// <summary>
        /// Type that the provider provides.
        /// </summary>
        Type Type { get; }

        /// <summary>
        /// Validates if the argument coming from the runtime is valid.
        /// </summary>
        /// <param name="input">Input argument.</param>
        /// <returns>True if valid.</returns>
        bool IsValid(string input);

        /// <summary>
        /// Provide the <see cref="argument"/> to the type <see cref="Type"/>.
        /// </summary>
        /// <param name="context">Instance of <see cref="CallContext"/></param>
        /// <param name="argument">String argument.</param>
        /// <returns>Parsed value.</returns>
        Expression GetArgument(Expression context, Expression argument);
    }
}
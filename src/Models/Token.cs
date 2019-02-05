using System;
using System.Collections.Generic;
using System.Text;

namespace MiScript.Models
{
    public enum Tokens
    {
        If = 0,
        Then = 1,
        Else = 2,
        Equals = 3,
        NotEquals = 4,
        Name = 5,
        String = 6,
        Number = 7,
        Boolean = 8,
        Argument = 9,
        End = 10,
        Stop = 11
    }

    /// <summary>
    /// Tokenized token for parsing.
    /// </summary>
    public struct Token
    {
        /// <summary>
        /// Type of the token.
        /// </summary>
        public Tokens TokenType;

        /// <summary>
        /// Value of the token if needed.
        /// </summary>
        public string Value;

        public override string ToString()
        {
            string value = TokenType.ToString();

            if (Value != null)
            {
                value += $"({Value})";
            }

            return $"[{value}]";
        }
    }
}

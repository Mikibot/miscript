using ProtoBuf;

namespace MiScript.Models
{
    public enum Tokens : byte
    {
        None = 0,
        If = 1,
        Then = 2,
        Else = 3,
        Equals = 4,
        NotEquals = 5,
        Name = 6,
        String = 7,
        Number = 8,
        Boolean = 9,
        Argument = 10,
        End = 11,
        Stop = 12,
        Var = 13,
        Assigns = 14,
        Add = 15
    }

    /// <summary>
    /// Tokenized token for parsing.
    /// </summary>
    [ProtoContract]
    public struct Token
    {
        public Token(Tokens tokenType, string value = null)
        {
            TokenType = tokenType;
            Value = value;
        }

        /// <summary>
        /// Type of the token.
        /// </summary>
        [ProtoMember(1)]
        public Tokens TokenType;

        /// <summary>
        /// Value of the token if needed.
        /// </summary>
        [ProtoMember(2)]
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

        private bool Equals(Token other)
        {
            return TokenType == other.TokenType && Value == other.Value;
        }

        public override bool Equals(object obj)
        {
            return obj is Token other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((int) TokenType * 397) ^ (Value != null ? Value.GetHashCode() : 0);
            }
        }
    }
}

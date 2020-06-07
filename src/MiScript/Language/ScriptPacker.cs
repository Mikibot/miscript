using ProtoBuf;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace MiScript.Models
{
    [DataContract]
    [ProtoContract]
    public class ScriptPackage
    {
        [DataMember(Order = 1)]
        [ProtoMember(1)]
        public string[] strValues;
        [DataMember(Order = 2)]
        [ProtoMember(2)]
        public PackedToken[] tokens;
    }

    [ProtoContract]
    public struct PackedToken
    {
        [DataMember(Order = 1)]
        [ProtoMember(1)]
        public Tokens token;
        [DataMember(Order = 2)]
        [ProtoMember(2)]
        public byte? valueIndex;
    }

    public static class ScriptPacker
    {
        public static ScriptPackage Pack(IEnumerable<Token> tokens)
        {
            List<string> valueCollection = new List<string>();
            List<PackedToken> tokenCollection = new List<PackedToken>();

            foreach(var t in tokens)
            {
                byte? valueIndex = null;
                if (t.Value != null)
                {
                    var index = valueCollection.IndexOf(t.Value);
                    
                    if (index == -1)
                    {
                        valueIndex = (byte) valueCollection.Count;
                        valueCollection.Add(t.Value);
                    }
                    else
                    {
                        valueIndex = (byte) index;
                    }
                }

                tokenCollection.Add(new PackedToken { token = t.TokenType, valueIndex = valueIndex });
            }
            return new ScriptPackage { strValues = valueCollection.ToArray(), tokens = tokenCollection.ToArray() };
        }
        public static List<Token> Unpack(ScriptPackage package)
        {
            List<Token> tokenCollection = new List<Token>();
            foreach(var token in package.tokens)
            {
                tokenCollection.Add(new Token(token.token, token.valueIndex.HasValue ? package.strValues[token.valueIndex.Value] : ""));
            }
            return tokenCollection;
        }

        public static string ToString(ScriptPackage pack, bool pretty = false)
        {
            StringBuilder b = new StringBuilder();
            var indent = 0;
            var isNewLine = true;

            foreach(var token in pack.tokens)
            {
                if (isNewLine)
                {
                    if (pretty && indent > 0)
                    {
                        b.Append(new string(' ', indent));
                    }

                    isNewLine = false;
                }
                else
                {
                    b.Append(' ');
                }
                
                switch (token.token)
                {
                    case Tokens.Then:
                    {
                        indent += 2;
                        b.AppendLine(token.token.ToString().ToLowerInvariant());
                        isNewLine = true;
                    } break;
                    
                    case Tokens.Else:
                    {
                        b.Length -= 2;
                        b.AppendLine();
                        b.AppendLine(token.token.ToString().ToLowerInvariant());
                        isNewLine = true;
                    } break;

                    case Tokens.Stop:
                    case Tokens.If:
                    {
                        b.Append(token.token.ToString().ToLowerInvariant());
                    } break;

                    case Tokens.Equals:
                    {
                        b.Append("=");
                    } break;

                    case Tokens.NotEquals:
                    {
                        b.Append("!=");
                    } break;

                    case Tokens.Name:
                    case Tokens.Number:
                    case Tokens.Boolean:
                    {
                        b.Append(pack.strValues[token.valueIndex.Value]);
                    } break;
                    
                    case Tokens.String:
                    {
                        b.Append('"');
                        b.Append(pack.strValues[token.valueIndex.Value]);
                        b.Append('"');
                    } break;

                    case Tokens.Add:
                    {
                        b.Append(" + ");
                    } break;

                    case Tokens.Argument:
                    {
                        b.Append($"${pack.strValues[token.valueIndex.Value]}");
                    } break;

                    case Tokens.End:
                    {
                        indent -= 2;
                        b.Append($"\n{token.token.ToString().ToLowerInvariant()}");
                    } break;

                    case Tokens.None:
                    default:
                        break;
                }
            }
            return b.ToString();
        }
    }
}

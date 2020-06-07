using System;
using System.Text;

namespace MiScript
{
    public static class StringExtensions
    {
        public static string GetPeek(this string input, int index, int length)
        {
            var text = input.AsSpan();

            var lineIndex = index;
            var offset = index;

            for (; lineIndex >= 0; lineIndex--)
            {
                var c = text[lineIndex];

                if (c == '\n' || c == '\r')
                {
                    break;
                }
            }

            if (lineIndex > 0)
            {
                text = text.Slice(lineIndex + 1);
                offset -= lineIndex + 1;
            }
        
            var endIndex = text.IndexOfAny('\r', '\n');

            if (endIndex != -1)
            {
                text = text.Slice(0, endIndex);
            }

            var sb = new StringBuilder();

            sb.Append(text);
            sb.Append('\n');
            sb.Append(new string(' ', offset));
            sb.Append(new string('^', length));

            return sb.ToString();
        }
    }
}
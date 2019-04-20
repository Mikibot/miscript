using System;
using System.Collections.Generic;
using System.Text;

namespace MiScript.Parser.Models
{
    public struct StackVariable
    {
        public string name;
        public object value;
        public int stackDepth;

        public override string ToString()
        {
            return $"{name}:{value}#{stackDepth}";
        }
    }
}

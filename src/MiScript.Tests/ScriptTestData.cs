using System.Collections.Generic;

namespace MiScript.Tests
{
    public class ScriptTestData
    {
        public ScriptTestData(string path, IReadOnlyDictionary<string, string> data)
        {
            Path = path;
            Test = data.TryGetValue("TEST", out var value) ? value.Trim() : "Unnamed test";
            Script = data.TryGetValue("SCRIPT", out value) ? value : null;
            Model = data.TryGetValue("MODEL", out value) ? value.Trim() : null;
            Expected = data.TryGetValue("EXPECT", out value) ? value.Trim() : null;
        }

        public string Path { get; }

        public string Test { get; }
        
        public string Script { get; }
        
        public string Model { get; }

        public string Expected { get; }

        public override string ToString()
        {
            return Path;
        }
    }
}
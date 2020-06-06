using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MiScript.Models;
using Xunit;
using Xunit.Abstractions;
using YamlDotNet.RepresentationModel;

namespace MiScript.Tests
{
    public class RunTests
    {
        private readonly ITestOutputHelper _output;

        public RunTests(ITestOutputHelper output)
        {
            _output = output;
        }
        
        [Theory]
        [MemberData(nameof(GetData))]
        public void Run(ScriptTestData test)
        {
            RunTest(test, false);
        }
        
        [Theory]
        [MemberData(nameof(GetData))]
        public void RunPacked(ScriptTestData test)
        {
            RunTest(test, true);
        }

        private void RunTest(ScriptTestData test, bool pack)
        {
            var result = Tokenizer.Parse(test.Script);

            _output.WriteLine(test.Path);
            _output.WriteLine(new string('-', test.Path.Length));

            var tokenLength = result.Tokens.Max(t => t.TokenType.ToString().Length) + 2;

            var sb = new StringBuilder();

            if (result.Warnings.Count > 0)
            {
                _output.WriteLine("Warnings:");
                foreach (var warning in result.Warnings)
                {
                    _output.WriteLine("[" + warning.Range.StartLine + ":" + warning.Range.StartColumn + "] " + warning.Message);
                }

                _output.WriteLine("");
            }

            var tokens = result.Tokens;

            _output.WriteLine("Tokens:");
            foreach (var token in tokens)
            {
                var type = token.TokenType.ToString();
                sb.Append(type);

                if (token.Value != null)
                {
                    sb.Append(new string(' ', tokenLength - type.Length));
                    sb.Append('"');
                    sb.Append(token.Value);
                    sb.Append('"');
                }

                _output.WriteLine(sb.ToString());
                sb.Clear();
            }

            if (pack)
            {
                var package = ScriptPacker.Pack(tokens);

                _output.WriteLine("");
                _output.WriteLine("Packaged:");
                _output.WriteLine(ScriptPacker.ToString(package, true));

                tokens = ScriptPacker.Unpack(package);
            }

            Dictionary<string, object> context;

            if (test.Model == null)
            {
                context = new Dictionary<string, object>();
            }
            else
            {
                var deserializer = new YamlDotNet.Serialization.Deserializer();

                context = deserializer.Deserialize<Dictionary<string, object>>(test.Model);
            }

            var parser = new Parser.Parser(tokens);
            var actual = parser.Parse(context);

            if (test.Expected != null)
            {
                Assert.Equal(test.Expected, actual);
            }
        }

        public static IEnumerable<object[]> GetData()
        {
            var assembly = typeof(RunTests).Assembly;
            var sb = new StringBuilder();

            foreach (var name in assembly.GetManifestResourceNames().Where(f => f.EndsWith(".mst")))
            {
                yield return new object[]
                {
                    LoadScript(assembly, name, sb)
                };
            }
        }
        
        private static ScriptTestData LoadScript(string name)
        {
            return LoadScript(typeof(RunTests).Assembly, name, new StringBuilder());
        }

        private static ScriptTestData LoadScript(Assembly assembly, string name, StringBuilder sb)
        {
            var stream = assembly.GetManifestResourceStream(name);
            var reader = new StreamReader(stream);

            string current = null;
            var data = new Dictionary<string, string>();

            void SetData()
            {
                if (current == null)
                {
                    return;
                }

                data[current] = sb.ToString();
                sb.Clear();
            }

            while (true)
            {
                var line = reader.ReadLine();

                if (line == null)
                {
                    break;
                }

                if (line.Length > 4 && line.StartsWith("--") && line.EndsWith("--"))
                {
                    SetData();
                    current = line.Substring(2, line.Length - 4).Trim();
                    continue;
                }

                sb.Append(line);
                sb.Append("\n");
            }

            SetData();

            return new ScriptTestData(name, data);
        }
    }
}
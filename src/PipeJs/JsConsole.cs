using Jint;
using Jint.Native;
using Jint.Runtime;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PipeJs
{
    public class JsConsole
    {
        private int _indent = 0;
        private Engine _engine;

        public JsConsole(Engine engine)
        {
            _engine = engine;
        }

        public void Log(params JsValue[] values)
        {
            var first = true;
            foreach (var value in values)
            {
                Console.Write(new String(' ', _indent * 2));
                if(value.IsPrimitive()) {
                    Console.Write(value.ToString());
                }
                else{
                    var json = _engine.Json.Stringify(JsValue.Null, Arguments.From(value));
                    Console.Write(json);
                }

                if (!first)
                {
                    Console.Write(" ");
                }

                first = false;
            }

            Console.WriteLine();
        }

        public void Info(params JsValue[] values)
        {
            WriteColored(ConsoleColor.Cyan, values);
        }

        public void Warning(params JsValue[] values)
        {
            WriteColored(ConsoleColor.Yellow, values);
        }

        public void Error(params JsValue[] values)
        {
            WriteColored(ConsoleColor.Red, values);
        }

        public void WriteColored(ConsoleColor color, params JsValue[] values)
        {
            Console.ForegroundColor = color;
            Log(values);
            Console.ResetColor();
        }

        public void Group()
        {
            _indent++;
        }

        public void GroupEnd()
        {
            _indent--;
        }

        public string Start(string filename, string arguments)
        {
            Process notePad = new Process();

            notePad.StartInfo.FileName = filename;
            notePad.StartInfo.Arguments = arguments;
            notePad.StartInfo.RedirectStandardOutput = true;
            notePad.StartInfo.UseShellExecute = false;

            notePad.Start();
            StreamReader s = notePad.StandardOutput;
            String output = s.ReadToEnd();
            notePad.WaitForExit();

            return output;
        }
    }
}

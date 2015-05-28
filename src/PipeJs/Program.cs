using Jint;
using Jint.Native;
using Jint.Parser;
using Jint.Runtime;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PipeJs
{
    class Program
    {
        static string script = @"
            var $input = {$input};
            function $bootstrap() {
                {$script}
            }

            $bootstrap.call($input);
        ";

        static int Main(string[] args)
        {
            var allowClr = args.Any() && args[0] == "--allow-clr";

            if (allowClr)
            {
                args = args.Skip(1).ToArray();
            }

            var engine = new Engine(cfg =>  {
                if(allowClr) 
                {
                    cfg.AllowClr();
                }
            });

            engine.SetValue("console", new JsConsole(engine));

            var arg = String.Join(" ", args);

            // load args as a filename if it starts with @
            if (arg.StartsWith("@"))
            {
                arg = arg.Substring(1);
                try
                {
                    File.ReadAllText(arg);
                }
                catch
                {
                    Console.WriteLine("Could not read specified file: " + arg);
                    return -1;
                }
            }
            
            var startPipeJsFilename = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),  "start.pipe.js");
            string startPipeJs = "";
            if (File.Exists(startPipeJsFilename))
            {
                startPipeJs = File.ReadAllText(startPipeJsFilename) + Environment.NewLine;
            }

            string thisText = "{}";
            bool isKeyAvailable;

            try
            {
                isKeyAvailable = System.Console.KeyAvailable;
            }
            catch (InvalidOperationException)
            {
                thisText = System.Console.In.ReadToEnd();
            }

            var fullScript = startPipeJs + script
                .Replace("{$input}", thisText)
                .Replace("{$script}", arg);

            try
            {
                engine.Execute(fullScript);

                var result = engine.GetCompletionValue();

                if (result != JsValue.Undefined)
                {
                    if (result.IsPrimitive())
                    {
                        Console.WriteLine(result.ToString());
                    }
                    else
                    {
                        Console.WriteLine(engine.Json.Stringify(JsValue.Null, Arguments.From(result)).ToString());
                    }
                }
            }
            catch (ParserException e)
            {
                Error(e.Message);
                Error(fullScript);

                return -2;
            }

            return 0;
        }

        private static void Error(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }


}

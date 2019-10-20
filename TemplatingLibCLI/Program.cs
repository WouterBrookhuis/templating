using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using TemplatingLib;
using TemplatingLibCLI.Messages;

namespace TemplatingLibCLI
{
    class Program
    {
        public static string GetArgument(string flag, string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == flag && i + 1 < args.Length)
                {
                    return args[i + 1];
                }
            }
            return null;
        }

        public static bool HasFlag(string flag, string[] args)
        {
            for(int i = 0; i < args.Length; i++)
            {
                if(args[i] == flag)
                {
                    return true;
                }
            }
            return false;
        }

        static void Main(string[] args)
        {
            if(HasFlag("-h", args))
            {
                Console.WriteLine("Message protocol code generator");
                Console.WriteLine();
                Console.WriteLine("-h Show this help page");
                Console.WriteLine("-i [path] Path to protocol xml file");
                Console.WriteLine("-t [path] Path to template library xml file");
                Console.WriteLine("-o [path] Path to output file");
                return;
            }

            var protocolFile = GetArgument("-i", args);
            var templateLibFile = GetArgument("-t", args);
            var outputFile = GetArgument("-o", args);

            if (protocolFile == null || templateLibFile == null || outputFile == null)
            {
                Console.WriteLine("Missing parameters, use -h to show help");
                return;
            }

            try
            {
                Console.WriteLine($"Input file: {protocolFile}");
                Console.WriteLine($"Template lib: {templateLibFile}");
                Console.WriteLine($"Output file: {outputFile}");

                var protocol = ProtocolLoader.LoadProtocol(protocolFile);

                var testThing = new TestThing();
                var modules = testThing.FromProtocol(protocol);

                var templateLib = TemplateLibrary.LoadFromFile(templateLibFile);

     
                using(var writer = new StreamWriter(outputFile, false, Encoding.UTF8))
                {
                    testThing.WriteModule(writer, templateLib, modules[0]);
                }
                Console.WriteLine("Done!");
            }
            catch(Exception e)
            {
                Console.WriteLine("ERROR");
                Console.WriteLine(e.Message);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemplatingLib;
using TemplatingLibCLI.Messages;

namespace TemplatingLibCLI
{
    class Program
    {
        private static void OutputObject(TextWriter writer, Template objectTemplate, Template fieldTemplate, string moduleName, string objectName, IEnumerable<Tuple<string, string>> fieldsTypeName)
        {
            var baseDictionary = new Dictionary<string, IInsertable>
            {
                { "module_name", new StringLiteral(moduleName) },
                { "obj_name", new StringLiteral(objectName) },
            };

            var fieldsContainer = new InsertableContainer()
            {
                AutoIndentAndNewline = true
            };

            foreach (var field in fieldsTypeName)
            {
                fieldsContainer.Insertables.Add(
                    new TemplateInsert(
                        fieldTemplate,
                        new NestedDictionary<string, IInsertable>(baseDictionary)
                        {
                            { "type", new StringLiteral(field.Item1) },
                            { "name", new StringLiteral(field.Item2) },
                        }
                        ));
            }

            var structDict = new NestedDictionary<string, IInsertable>(baseDictionary)
            {
                { "fields", fieldsContainer }
            };

            objectTemplate.Apply(writer, structDict);
        }

        private static void OutputObjectParser(TextWriter writer, Template parseFuncTemplate, Template parseFieldTemplate, string moduleName, string objectName, IEnumerable<Tuple<string, string, int>> fieldsTypeName)
        {
            var baseDictionary = new Dictionary<string, IInsertable>
            {
                { "module_name", new StringLiteral(moduleName) },
                { "obj_name", new StringLiteral(objectName) },
                { "obj_type", new StringLiteral(moduleName + "_" + objectName) }
            };

            var fieldsContainer = new InsertableContainer()
            {
                AutoIndentAndNewline = true
            };

            foreach (var field in fieldsTypeName)
            {
                fieldsContainer.Insertables.Add(
                    new TemplateInsert(
                        parseFieldTemplate,
                        new NestedDictionary<string, IInsertable>(baseDictionary)
                        {
                            { "field_type", new StringLiteral(field.Item1) },
                            { "field_name", new StringLiteral(field.Item2) },
                            { "offset", new StringLiteral(field.Item3.ToString()) },
                        }
                        ));
            }

            var structDict = new NestedDictionary<string, IInsertable>(baseDictionary)
            {
                { "parse_fields", fieldsContainer }
            };

            parseFuncTemplate.Apply(writer, structDict);
        }

        static void Main(string[] args)
        {
            var protocol = ProtocolLoader.LoadProtocol("ptp.xml");

            var testThing = new TestThing();
            var modules = testThing.FromProtocol(protocol);

            var templateLib = new TemplateLibary();
            templateLib.FileTemplate = new Template().LoadFromFile("Templates/c_file.t");
            templateLib.TypeNameTemplate = new Template().LoadFromFile("Templates/c_typename.t");
            templateLib.GenericTypeTemplates = new TemplateLibary.TypeTemplates
            {
                ReadCall = new Template().LoadFromFile("Templates/c_read_call.t"),
                WriteCall = new Template().LoadFromFile("Templates/c_write_call.t"),
                ReadFunction = new Template().LoadFromFile("Templates/c_read_function.t"),
                WriteFunction = new Template().LoadFromFile("Templates/c_write_function.t"),
                TypeTemplate = new Template().LoadFromFile("Templates/c_struct.t"),
                TypeFieldTemplate = new Template().LoadFromFile("Templates/c_struct_field.t"),
            };

            templateLib.CustomTypeTemplates.Add("MessageType", new TemplateLibary.TypeTemplates
            {
                ReadCall = new Template().LoadFromFile("Templates/c_read_call_uint4.t"),
                WriteCall = new Template().LoadFromFile("Templates/c_write_call_uint4.t"),
                ReadFunction = new Template().LoadFromFile("Templates/blank.t"),
                WriteFunction = new Template().LoadFromFile("Templates/blank.t"),
                TypeTemplate = new Template().LoadFromFile("Templates/c_typedef_uint4.t"),
                TypeFieldTemplate = new Template().LoadFromFile("Templates/blank.t"),
            });

            templateLib.CustomTypeTemplates.Add("uint4", new TemplateLibary.TypeTemplates
            {
                ReadCall = new Template().LoadFromFile("Templates/c_read_call_uint4.t"),
                WriteCall = new Template().LoadFromFile("Templates/c_write_call_uint4.t"),
                ReadFunction = new Template().LoadFromFile("Templates/blank.t"),
                WriteFunction = new Template().LoadFromFile("Templates/blank.t"),
                TypeTemplate = new Template().LoadFromFile("Templates/c_typedef_uint4.t"),
                TypeFieldTemplate = new Template().LoadFromFile("Templates/blank.t"),
            });

            templateLib.CustomTypeTemplates.Add("uint8", new TemplateLibary.TypeTemplates
            {
                ReadCall = new Template().LoadFromFile("Templates/c_read_call_uint8.t"),
                WriteCall = new Template().LoadFromFile("Templates/c_write_call_uint8.t"),
                ReadFunction = new Template().LoadFromFile("Templates/blank.t"),
                WriteFunction = new Template().LoadFromFile("Templates/blank.t"),
                TypeTemplate = new Template().LoadFromFile("Templates/c_typedef_uintx.t"),
                TypeFieldTemplate = new Template().LoadFromFile("Templates/blank.t"),
            });

            templateLib.CustomTypeTemplates.Add("int8", new TemplateLibary.TypeTemplates
            {
                ReadCall = new Template().LoadFromFile("Templates/c_read_call_uint8.t"),
                WriteCall = new Template().LoadFromFile("Templates/c_write_call_uint8.t"),
                ReadFunction = new Template().LoadFromFile("Templates/blank.t"),
                WriteFunction = new Template().LoadFromFile("Templates/blank.t"),
                TypeTemplate = new Template().LoadFromFile("Templates/c_typedef_uintx.t"),
                TypeFieldTemplate = new Template().LoadFromFile("Templates/blank.t"),
            });

            templateLib.CustomTypeTemplates.Add("uint16", new TemplateLibary.TypeTemplates
            {
                ReadCall = new Template().LoadFromFile("Templates/c_read_call_uint16.t"),
                WriteCall = new Template().LoadFromFile("Templates/c_write_call_uint16.t"),
                ReadFunction = new Template().LoadFromFile("Templates/blank.t"),
                WriteFunction = new Template().LoadFromFile("Templates/blank.t"),
                TypeTemplate = new Template().LoadFromFile("Templates/c_typedef_uintx.t"),
                TypeFieldTemplate = new Template().LoadFromFile("Templates/blank.t"),
            });

            templateLib.CustomTypeTemplates.Add("int16", new TemplateLibary.TypeTemplates
            {
                ReadCall = new Template().LoadFromFile("Templates/c_read_call_uint16.t"),
                WriteCall = new Template().LoadFromFile("Templates/c_write_call_uint16.t"),
                ReadFunction = new Template().LoadFromFile("Templates/blank.t"),
                WriteFunction = new Template().LoadFromFile("Templates/blank.t"),
                TypeTemplate = new Template().LoadFromFile("Templates/c_typedef_uintx.t"),
                TypeFieldTemplate = new Template().LoadFromFile("Templates/blank.t"),
            });

            templateLib.CustomTypeTemplates.Add("uint32", new TemplateLibary.TypeTemplates
            {
                ReadCall = new Template().LoadFromFile("Templates/c_read_call_uint32.t"),
                WriteCall = new Template().LoadFromFile("Templates/c_write_call_uint32.t"),
                ReadFunction = new Template().LoadFromFile("Templates/blank.t"),
                WriteFunction = new Template().LoadFromFile("Templates/blank.t"),
                TypeTemplate = new Template().LoadFromFile("Templates/c_typedef_uintx.t"),
                TypeFieldTemplate = new Template().LoadFromFile("Templates/blank.t"),
            });

            templateLib.CustomTypeTemplates.Add("int32", new TemplateLibary.TypeTemplates
            {
                ReadCall = new Template().LoadFromFile("Templates/c_read_call_uint32.t"),
                WriteCall = new Template().LoadFromFile("Templates/c_write_call_uint32.t"),
                ReadFunction = new Template().LoadFromFile("Templates/blank.t"),
                WriteFunction = new Template().LoadFromFile("Templates/blank.t"),
                TypeTemplate = new Template().LoadFromFile("Templates/c_typedef_uintx.t"),
                TypeFieldTemplate = new Template().LoadFromFile("Templates/blank.t"),
            });

            templateLib.CustomTypeTemplates.Add("uint48", new TemplateLibary.TypeTemplates
            {
                ReadCall = new Template().LoadFromFile("Templates/c_read_call.t"),
                WriteCall = new Template().LoadFromFile("Templates/c_write_call.t"),
                ReadFunction = new Template().LoadFromFile("Templates/c_read_function_uint48.t"),
                WriteFunction = new Template().LoadFromFile("Templates/c_write_function_uint48.t"),
                TypeTemplate = new Template().LoadFromFile("Templates/c_typedef_uint48.t"),
                TypeFieldTemplate = new Template().LoadFromFile("Templates/blank.t"),
            });

            templateLib.CustomTypeTemplates.Add("uint64", new TemplateLibary.TypeTemplates
            {
                ReadCall = new Template().LoadFromFile("Templates/c_read_call_uint64.t"),
                WriteCall = new Template().LoadFromFile("Templates/c_write_call_uint64.t"),
                ReadFunction = new Template().LoadFromFile("Templates/blank.t"),
                WriteFunction = new Template().LoadFromFile("Templates/blank.t"),
                TypeTemplate = new Template().LoadFromFile("Templates/c_typedef_uintx.t"),
                TypeFieldTemplate = new Template().LoadFromFile("Templates/blank.t"),
            });

            templateLib.CustomTypeTemplates.Add("int64", new TemplateLibary.TypeTemplates
            {
                ReadCall = new Template().LoadFromFile("Templates/c_read_call_uint64.t"),
                WriteCall = new Template().LoadFromFile("Templates/c_write_call_uint64.t"),
                ReadFunction = new Template().LoadFromFile("Templates/blank.t"),
                WriteFunction = new Template().LoadFromFile("Templates/blank.t"),
                TypeTemplate = new Template().LoadFromFile("Templates/c_typedef_uintx.t"),
                TypeFieldTemplate = new Template().LoadFromFile("Templates/blank.t"),
            });

            string outputFile = modules[0].Name + ".c";
            Console.WriteLine($"Outputting to {outputFile}");
            try
            {
                using(var writer = new StreamWriter(outputFile, false, Encoding.UTF8))
                {
                    testThing.WriteModule(writer, templateLib, modules[0]);
                }
                Console.WriteLine("Done!");
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }

            Console.ReadKey();
        }
    }
}

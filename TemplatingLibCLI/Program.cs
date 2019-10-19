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
            var structFieldTemplate = new Template().LoadFromFile("Templates/c_struct_field.t");
            var structTemplate = new Template().LoadFromFile("Templates/c_struct.t");
            var typeNameTemplate = new Template().LoadFromFile("Templates/c_typename.t");
            var parseFuncTemplate = new Template().LoadFromFile("Templates/c_internal_parse_func.t");
            var parseFieldTemplate = new Template().LoadFromFile("Templates/c_parse_field.t");

            var protocol = ProtocolLoader.LoadProtocol("ptp.xml");

            

            var writer = Console.Out;

            var baseDictionary = new Dictionary<string, IInsertable>
            {
                { "obj_name", new StringLiteral("PTP_Header") },
                { "obj_size", new StringLiteral("32") },
            };

            Template template = new Template();
            template.LoadFromFile("Templates/c_parsing.t");
            try
            {
                template.Apply(Console.Out, new NestedDictionary<string, IInsertable>(baseDictionary)
                {
                    { "struct_name", new StringLiteral("PTP_Header_t") },
                });

                OutputObject(Console.Out, structTemplate, structFieldTemplate, "PTP", "MessageHeader", new[] {
                    new Tuple<string, string>("PTP_MessageType" , "messageType"),
                    new Tuple<string, string>("uint8" , "version"),
                    new Tuple<string, string>("uint16" , "messageLength"),
                    new Tuple<string, string>("uint8" , "domainNumber"),
                    new Tuple<string, string>("uint8" , "reserved2"),
                    new Tuple<string, string>("uint16" , "flagField"),
                    new Tuple<string, string>("int64" , "correctionField"),
                    new Tuple<string, string>("uint32" , "reserved3"),
                    new Tuple<string, string>("PTP_PortIdentity" , "sourcePortIdentity"),
                    new Tuple<string, string>("uint16" , "sequenceId"),
                    new Tuple<string, string>("uint8" , "controlField"),
                    new Tuple<string, string>("int8" , "logMessageInterval")
                });

                Console.Out.WriteLine();

                OutputObjectParser(Console.Out, parseFuncTemplate, parseFieldTemplate, "PTP", "MessageHeader", new[] {
                    new Tuple<string, string, int>("PTP_MessageType" , "messageType", 0),
                    new Tuple<string, string, int>("uint8" , "version", 1),
                    new Tuple<string, string, int>("uint16" , "messageLength", 2),
                    new Tuple<string, string, int>("uint8" , "domainNumber", 4),
                    new Tuple<string, string, int>("uint8" , "reserved2", 5),
                    new Tuple<string, string, int>("uint16" , "flagField", 6),
                    new Tuple<string, string, int>("int64" , "correctionField", 8),
                    new Tuple<string, string, int>("uint32" , "reserved3", 16),
                    new Tuple<string, string, int>("PTP_PortIdentity" , "sourcePortIdentity", 20),
                    new Tuple<string, string, int>("uint16" , "sequenceId", 30),
                    new Tuple<string, string, int>("uint8" , "controlField", 32),
                    new Tuple<string, string, int>("int8" , "logMessageInterval", 33)
                });
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
            Console.ReadKey();
        }
    }
}

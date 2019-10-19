using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemplatingLib;

namespace TemplatingLibCLI
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                return;
            }

            var structFieldTemplate = new Template().LoadFromFile("Templates/c_struct_field.t");
            var structTemplate = new Template().LoadFromFile("Templates/c_struct.t");

            var writer = Console.Out;

            var ptpHeaderTemplate = new Template(structTemplate);


            var baseDictionary = new Dictionary<string, IInsertable>
            {
                { "obj_name", new StringLiteral("PTP_Header") },
                { "obj_size", new StringLiteral("32") },
            };

            Template parseFieldTemplate = new Template();
            parseFieldTemplate.LoadFromString("%obj_name%->%field_name% = Parse%field_name%(&buffer[%offset%]);", ParsingOptions.Default);

            var parseFieldsContainer = new InsertableContainer(new IInsertable[]
            {
                new TemplateInsert(parseFieldTemplate, new NestedDictionary<string, IInsertable>(baseDictionary)
                {
                    { "field_name", new StringLiteral("timestamp") },
                    { "offset", new StringLiteral("0") },
                }),
                new TemplateInsert(parseFieldTemplate, new NestedDictionary<string, IInsertable>(baseDictionary)
                {
                    { "field_name", new StringLiteral("messageType") },
                    { "offset", new StringLiteral("4") },
                }),
                new TemplateInsert(parseFieldTemplate, new NestedDictionary<string, IInsertable>(baseDictionary)
                {
                    { "obj_name", new StringLiteral("PTPHeader") },
                    { "field_name", new StringLiteral("version") },
                    { "offset", new StringLiteral("5") },
                }),
            });
            parseFieldsContainer.AutoIndentAndNewline = true;

            Template template = new Template();
            template.LoadFromFile(args[0]);
            try
            {
                template.Apply(Console.Out, new NestedDictionary<string, IInsertable>(baseDictionary)
                {
                    { "struct_name", new StringLiteral("PTP_Header_t") },
                    { "parse_fields", parseFieldsContainer },
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

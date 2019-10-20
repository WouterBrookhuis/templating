using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using TemplatingLib;

namespace TemplatingLibCLI
{
    public class TemplateLibrary
    {
        public struct TypeTemplates
        {
            public Template WriteCall { get; set; }
            public Template ReadCall { get; set; }
            public Template WriteFunction { get; set; }
            public Template ReadFunction { get; set; }
            public Template TypeTemplate { get; set; }
            public Template TypeFieldTemplate { get; set; }
        }

        public TypeTemplates GenericTypeTemplates { get; set; }
        public Template FileTemplate { get; set; }
        public Template TypeNameTemplate { get; set; }
        public Dictionary<string, TypeTemplates> CustomTypeTemplates { get; set; }

        public TemplateLibrary()
        {
            CustomTypeTemplates = new Dictionary<string, TypeTemplates>();
        }

        public static TemplateLibrary LoadFromFile(string filename)
        {
            try
            {
                using(var reader = new StreamReader(filename))
                {
                    XmlSerializer xml = new XmlSerializer(typeof(TemplateLibraryXML.TemplateLibraryXML));
                    return LoadFromXml(xml.Deserialize(reader) as TemplateLibraryXML.TemplateLibraryXML);
                }
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        private static TemplateLibrary LoadFromXml(TemplateLibraryXML.TemplateLibraryXML xml)
        {
            var lib = new TemplateLibrary();

            lib.FileTemplate = new Template().LoadFromFile(xml.FileTemplate);
            lib.TypeNameTemplate = new Template().LoadFromFile(xml.TypeNameTemplate);
            lib.GenericTypeTemplates = new TypeTemplates
            {
                ReadCall = new Template().LoadFromFile(xml.DefaultTypeTemplates.ReadCall),
                ReadFunction = new Template().LoadFromFile(xml.DefaultTypeTemplates.ReadFunction),
                WriteCall = new Template().LoadFromFile(xml.DefaultTypeTemplates.WriteCall),
                WriteFunction = new Template().LoadFromFile(xml.DefaultTypeTemplates.WriteFunction),
                TypeFieldTemplate = new Template().LoadFromFile(xml.DefaultTypeTemplates.TypeFieldTemplate),
                TypeTemplate = new Template().LoadFromFile(xml.DefaultTypeTemplates.TypeTemplate)
            };

            foreach(var customTypeTemplate in xml.CustomTypeTemplates)
            {
                lib.CustomTypeTemplates.Add(customTypeTemplate.Type, new TypeTemplates
                {
                    ReadCall = new Template().LoadFromFile(customTypeTemplate.ReadCall),
                    ReadFunction = new Template().LoadFromFile(customTypeTemplate.ReadFunction),
                    WriteCall = new Template().LoadFromFile(customTypeTemplate.WriteCall),
                    WriteFunction = new Template().LoadFromFile(customTypeTemplate.WriteFunction),
                    TypeFieldTemplate = new Template().LoadFromFile(customTypeTemplate.TypeFieldTemplate),
                    TypeTemplate = new Template().LoadFromFile(customTypeTemplate.TypeTemplate)
                });
            }

            return lib;
        }
    }
}

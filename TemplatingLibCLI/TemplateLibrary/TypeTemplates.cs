using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TemplatingLibCLI.TemplateLibraryXML
{
    [XmlType(TypeName = "TypeTemplates")]
    public class TypeTemplatesXML
    {
        [XmlAttribute]
        public string Type { get; set; }
        public string WriteCall { get; set; }
        public string ReadCall { get; set; }
        public string WriteFunction { get; set; }
        public string ReadFunction { get; set; }
        public string TypeTemplate { get; set; }
        public string TypeFieldTemplate { get; set; }
        public string TypeNameTemplate { get; set; }
    }
}

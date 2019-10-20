using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TemplatingLibCLI.TemplateLibraryXML
{
    [XmlRoot(ElementName = "TemplateLibrary")]
    public class TemplateLibraryXML
    {
        [XmlAttribute]
        public string Name { get; set; }
        public string FileTemplate { get; set; }
        public TypeTemplatesXML DefaultTypeTemplates { get; set; }
        public List<TypeTemplatesXML> CustomTypeTemplates { get; set; }

        public TemplateLibraryXML()
        {
            CustomTypeTemplates = new List<TypeTemplatesXML>();
        }
    }
}

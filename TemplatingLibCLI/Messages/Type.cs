using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TemplatingLibCLI.Messages
{
    public class Type
    {
        [XmlAttribute]
        public string Name { get; set; }
        [XmlAttribute]
        public int NumBits { get; set; }
        public List<Field> Fields { get; set; }
    }
}

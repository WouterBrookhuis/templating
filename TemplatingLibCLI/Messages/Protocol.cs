using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TemplatingLibCLI.Messages
{
    [XmlRoot]
    public class Protocol
    {
        [XmlAttribute]
        public string Name { get; set; }

        public List<Type> Types { get; set; }

        public List<Message> Messages { get; set; }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TemplatingLibCLI.Messages
{
    public class Field
    {
        [XmlAttribute]
        public string Type { get; set; }
        [XmlAttribute]
        public string Name { get; set; }
    }
}

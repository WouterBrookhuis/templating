using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemplatingLib;

namespace TemplatingLibCLI
{
    public class TemplateLibary
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

        public TemplateLibary()
        {
            CustomTypeTemplates = new Dictionary<string, TypeTemplates>();
        }
    }
}

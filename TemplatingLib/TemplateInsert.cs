using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplatingLib
{
    public class TemplateInsert : IInsertable
    {
        private Template _template;
        private IDictionary<string, IInsertable> _variables;

        public TemplateInsert(Template template)
        {
            _template = template ?? throw new ArgumentNullException(nameof(template));
            _variables = new Dictionary<string, IInsertable>();
        }

        public TemplateInsert(Template template, IDictionary<string, IInsertable> variables)
        {
            _template = template ?? throw new ArgumentNullException(nameof(template));
            _variables = variables ?? throw new ArgumentNullException(nameof(variables));
        }

        public void Insert(TextWriter output, int currentIndentation)
        {
            _template.Apply(output, _variables);
        }
    }
}

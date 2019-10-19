using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplatingLib
{
    public class Template
    {
        private string[] _hardStrings;
        private InsertionPoint[] _insertionPoints;

        public bool IgnoreMissingVariables { get; set; }

        public Template()
        {
            _hardStrings = new string[0];
            _insertionPoints = new InsertionPoint[0];
            IgnoreMissingVariables = false;
        }

        public Template(Template template)
        {
            _hardStrings = new string[template._hardStrings.Length];
            Array.Copy(template._hardStrings, _hardStrings, _hardStrings.Length);
            _insertionPoints = new InsertionPoint[template._insertionPoints.Length];
            Array.Copy(template._insertionPoints, _insertionPoints, _insertionPoints.Length);
            IgnoreMissingVariables = template.IgnoreMissingVariables;
        }

        public void Apply(TextWriter output, IDictionary<string, IInsertable> variables)
        {
            for(int i = 0; i < _hardStrings.Length; i++)
            {
                output.Write(_hardStrings[i]);

                if(i < _insertionPoints.Length)
                {
                    if(variables.TryGetValue(_insertionPoints[i].Name, out IInsertable insertable))
                    {
                        insertable.Insert(output, _insertionPoints[i].Column - 1);
                    }
                    else if(!IgnoreMissingVariables)
                    {
                        throw new Exception($"Missing variable {_insertionPoints[i].Name}");
                    }
                }
            }
        }

        public Template LoadFromFile(string file)
        {
            try
            {
                using(var reader = File.OpenText(file))
                {
                    var templateString = reader.ReadToEnd();
                    LoadFromString(templateString, ParsingOptions.Default);
                }
            }
            catch(Exception e)
            {
                throw new Exception("Could not open file to load template", e);
            }

            return this;
        }

        public Template LoadFromString(string templateString, ParsingOptions parsingOptions)
        {
            var hardStrings = new List<string>();
            var insertionPoints = new List<InsertionPoint>();
            bool isReadingName = false;
            int line = 1;
            int column = 1;

            int prevEnd = 0;
            int pos;
            for (pos = 0; pos < templateString.Length; pos++)
            {
                if (string.Compare(parsingOptions.NewLine, 0, templateString, pos, parsingOptions.NewLine.Length) == 0)
                {
                    if (isReadingName)
                    {
                        throw new ParsingException(line, column, "Found newline inside name");
                    }

                    line++;
                    column = 1;
                    // Skip the newline sequence
                    pos += parsingOptions.NewLine.Length - 1;
                }
                else
                {

                    if(isReadingName)
                    {
                        if(templateString[pos] == parsingOptions.InsertionPointPostfix)
                        {
                            if(pos == prevEnd + 1)
                            {
                                throw new ParsingException(line, column, "Zero length name");
                            }
                            isReadingName = false;
                            insertionPoints.Add(new InsertionPoint(column - (pos - prevEnd), line, templateString.Substring(prevEnd + 1, pos - prevEnd - 1)));
                            prevEnd = pos + 1;
                        }
                    }
                    else
                    {
                        if(templateString[pos] == parsingOptions.InsertionPointPrefix)
                        {
                            isReadingName = true;
                            hardStrings.Add(templateString.Substring(prevEnd, pos - prevEnd));
                            prevEnd = pos;
                        }
                    }

                    column++;
                }
            }

            if (pos > prevEnd)
            {
                hardStrings.Add(templateString.Substring(prevEnd, pos - prevEnd));
            }

            _hardStrings = hardStrings.ToArray();
            _insertionPoints = insertionPoints.ToArray();

            return this;
        }
    }
}

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
        private struct Line
        {
            public string[] hardStrings;
            public InsertionPoint[] insertionPoints;

            public Line(string[] hardStrings, InsertionPoint[] insertionPoints)
            {
                this.hardStrings = hardStrings ?? throw new ArgumentNullException(nameof(hardStrings));
                this.insertionPoints = insertionPoints ?? throw new ArgumentNullException(nameof(insertionPoints));
            }
        }

        private Line[] _lines;

        public bool IgnoreMissingVariables { get; set; }

        public Template()
        {
            _lines = new Line[0];
            IgnoreMissingVariables = false;
        }

        public void Apply(TextWriter output, IDictionary<string, IInsertable> variables, int indentation)
        {
            for(int l = 0; l < _lines.Length; l++)
            {
                if (l > 0)
                {
                    output.Write("".PadLeft(indentation));
                }

                for(int i = 0; i < _lines[l].hardStrings.Length; i++)
                {
                    output.Write(_lines[l].hardStrings[i]);

                    if(i < _lines[l].insertionPoints.Length)
                    {
                        if(variables.TryGetValue(_lines[l].insertionPoints[i].Name, out IInsertable insertable))
                        {
                            insertable.Insert(output, indentation + _lines[l].insertionPoints[i].Column - 1);
                        }
                        else if(!IgnoreMissingVariables)
                        {
                            throw new Exception($"Missing variable {_lines[l].insertionPoints[i].Name}");
                        }
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
            var lines = new List<Line>();
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
                    // Store line
                    hardStrings.Add(templateString.Substring(prevEnd, pos - prevEnd + parsingOptions.NewLine.Length));
                    lines.Add(new Line(hardStrings.ToArray(), insertionPoints.ToArray()));
                    hardStrings.Clear();
                    insertionPoints.Clear();
                    // Skip the newline sequence
                    pos += parsingOptions.NewLine.Length - 1;
                    prevEnd = pos + 1;
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

            if (hardStrings.Count > 0)
            {
                lines.Add(new Line(hardStrings.ToArray(), insertionPoints.ToArray()));
            }

            _lines = lines.ToArray();

            return this;
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TemplatingLibCLI.Messages
{
    class ProtocolLoader
    {
        public static Protocol LoadProtocol(string filename)
        {
            try
            {
                using (var reader = new StreamReader(filename))
                {
                    XmlSerializer xml = new XmlSerializer(typeof(Protocol));
                    return xml.Deserialize(reader) as Protocol;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static bool SaveProtocol(string filename, Protocol protocol)
        {
            try
            {
                using (var writer = new StreamWriter(filename, false, Encoding.UTF8))
                {
                    XmlSerializer xml = new XmlSerializer(typeof(Protocol));
                    xml.Serialize(writer, protocol);
                    return true;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}

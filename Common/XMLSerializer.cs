using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using System.IO;

namespace DotNet.Utilities
{
   public class XMLSerializer
    {

        public static string XmlSerializer<T>(T serialObject)
        {
            XmlSerializer ser = new XmlSerializer(typeof(T));
            System.IO.MemoryStream mem = new MemoryStream();
            XmlTextWriter writer = new XmlTextWriter(mem, Encoding.UTF8);
            ser.Serialize(writer, serialObject);
            writer.Close();

            return Encoding.UTF8.GetString(mem.ToArray());
        }


        public static T XmlDeserialize<T>(string str)
        {
            XmlSerializer mySerializer = new XmlSerializer(typeof(T));
            StreamReader mem2 = new StreamReader(
                    new MemoryStream(System.Text.Encoding.UTF8.GetBytes(str)),
                    System.Text.Encoding.UTF8);

            return (T)mySerializer.Deserialize(mem2);
        }

    }
}

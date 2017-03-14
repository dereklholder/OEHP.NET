using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DoYouEvenXMLBro
{
    public class BroBuilder
    {
        public static string BuildXMLLine(string field, string value)
        {
            StringBuilder sb = new StringBuilder();
            XmlWriterSettings xs = new XmlWriterSettings();
            xs.OmitXmlDeclaration = true;
            xs.Indent = false;


            using (XmlWriter xml = XmlWriter.Create(sb, xs))
            {
                xml.WriteStartElement(field);
                xml.WriteString(value);
                xml.WriteEndElement();
            }
            return sb.ToString();
        }
        public static string BuildXMLStart(string field)
        {
            StringBuilder sb = new StringBuilder();
            XmlWriterSettings xs = new XmlWriterSettings();
            xs.OmitXmlDeclaration = true;
            xs.Indent = false;

            using (XmlWriter xml = XmlWriter.Create(sb, xs))
            {
                xml.WriteStartElement(field);
            }
            return sb.ToString();
        }
        public static string BuildXMLEnd(string field)
        {
            StringBuilder sb = new StringBuilder();
            XmlWriterSettings xs = new XmlWriterSettings();
            xs.OmitXmlDeclaration = true;
            xs.Indent = false;


            using (XmlWriter xml = XmlWriter.Create(sb, xs))
            {

                xml.WriteStartElement(field);
            }
            string content = sb.ToString();
            content.Trim('/');
            return sb.ToString();
        }
    }
}

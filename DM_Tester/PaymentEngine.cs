using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using System.Net;
using System.IO;

namespace DM_Tester
{
    public class PaymentEngine
    {
        public string CallGateway(string request, string url)
        {
            try
            {
                string response;

                HttpWebRequest dtgRequest = (HttpWebRequest)WebRequest.Create(url);

                //Connection Settings
                dtgRequest.KeepAlive = false;
                dtgRequest.Timeout = 15000;
                dtgRequest.Method = "POST";

                //Connection Settings
                byte[] dtgByteArray = Encoding.ASCII.GetBytes(request);
                dtgRequest.ContentType = "application/x-www-form-urlencoded";
                dtgRequest.ContentLength = dtgByteArray.Length;

                Stream dtgDataStream = dtgRequest.GetRequestStream();

                dtgDataStream.Write(dtgByteArray, 0, dtgByteArray.Length);
                dtgDataStream.Close();

                WebResponse dtgResponse = dtgRequest.GetResponse();

                dtgDataStream = dtgResponse.GetResponseStream();

                StreamReader reader =new StreamReader(dtgDataStream);

                response = reader.ReadToEnd();

                reader.Close();
                dtgDataStream.Close();
                dtgResponse.Close();

                return response;
                
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public string UpdateAliasGenerator(string alias, string xWebID, string authKey, string terminalID, string acctNum, string expDate)
        {
            StringBuilder sb = new StringBuilder();

            XmlWriterSettings ws = new XmlWriterSettings();
            ws.Indent = true;

            using (XmlWriter xw = XmlWriter.Create(sb, ws))
            {
                xw.WriteStartDocument();

                xw.WriteStartElement("GatewayRequest");

                xw.WriteStartElement("SpecVersion");
                xw.WriteString("XWeb3.6");
                xw.WriteEndElement();

                xw.WriteStartElement("XWebID");
                xw.WriteString(xWebID);
                xw.WriteEndElement();

                xw.WriteStartElement("AuthKey");
                xw.WriteString(authKey);
                xw.WriteEndElement();

                xw.WriteStartElement("TerminalID");
                xw.WriteString(terminalID);
                xw.WriteEndElement();

                xw.WriteStartElement("Alias");
                xw.WriteString(alias);
                xw.WriteEndElement();

                xw.WriteStartElement("ExpDate");
                xw.WriteString(expDate);
                xw.WriteEndElement();

                xw.WriteStartElement("AcctNum");
                xw.WriteString(acctNum);
                xw.WriteEndElement();

                xw.WriteStartElement("POSType");
                xw.WriteString("PC");
                xw.WriteEndElement();

                xw.WriteStartElement("PinCapabilities");
                xw.WriteString("FALSE");
                xw.WriteEndElement();

                xw.WriteStartElement("TrackCapabilities");
                xw.WriteString("BOTH");
                xw.WriteEndElement();

                xw.WriteStartElement("TransactionType");
                xw.WriteString("AliasUpdateTransaction");
                xw.WriteEndElement();

            }

            return sb.ToString();
        }
        public string DeleteAliasGenerator(string alias, string xWebID, string authKey, string terminalID)
        {
            StringBuilder sb = new StringBuilder();

            XmlWriterSettings ws = new XmlWriterSettings();
            ws.Indent = true;

            using (XmlWriter xw = XmlWriter.Create(sb, ws))
            {
                xw.WriteStartDocument();

                xw.WriteStartElement("GatewayRequest");

                xw.WriteStartElement("SpecVersion");
                xw.WriteString("XWeb3.6");
                xw.WriteEndElement();

                xw.WriteStartElement("XWebID");
                xw.WriteString(xWebID);
                xw.WriteEndElement();

                xw.WriteStartElement("AuthKey");
                xw.WriteString(authKey);
                xw.WriteEndElement();

                xw.WriteStartElement("TerminalID");
                xw.WriteString(terminalID);
                xw.WriteEndElement();

                xw.WriteStartElement("Alias");
                xw.WriteString(alias);
                xw.WriteEndElement();

                xw.WriteStartElement("POSType");
                xw.WriteString("PC");
                xw.WriteEndElement();

                xw.WriteStartElement("PinCapabilities");
                xw.WriteString("FALSE");
                xw.WriteEndElement();

                xw.WriteStartElement("TrackCapabilities");
                xw.WriteString("BOTH");
                xw.WriteEndElement();

                xw.WriteStartElement("TransactionType");
                xw.WriteString("AliasDeleteTransaction");
                xw.WriteEndElement();

            }

            return sb.ToString();
        }
        public string LookupAliasGenerator(string alias, string xWebID, string authKey, string terminalID)
        {
            StringBuilder sb = new StringBuilder();

            XmlWriterSettings ws = new XmlWriterSettings();
            ws.Indent = true;

            using (XmlWriter xw = XmlWriter.Create(sb, ws))
            {
                xw.WriteStartDocument();

                xw.WriteStartElement("GatewayRequest");

                xw.WriteStartElement("SpecVersion");
                xw.WriteString("XWeb3.6");
                xw.WriteEndElement();

                xw.WriteStartElement("XWebID");
                xw.WriteString(xWebID);
                xw.WriteEndElement();

                xw.WriteStartElement("AuthKey");
                xw.WriteString(authKey);
                xw.WriteEndElement();

                xw.WriteStartElement("TerminalID");
                xw.WriteString(terminalID);
                xw.WriteEndElement();

                xw.WriteStartElement("Alias");
                xw.WriteString(alias);
                xw.WriteEndElement();

                xw.WriteStartElement("POSType");
                xw.WriteString("PC");
                xw.WriteEndElement();

                xw.WriteStartElement("PinCapabilities");
                xw.WriteString("FALSE");
                xw.WriteEndElement();

                xw.WriteStartElement("TrackCapabilities");
                xw.WriteString("BOTH");
                xw.WriteEndElement();

                xw.WriteStartElement("TransactionType");
                xw.WriteString("AliasLookupTransaction");
                xw.WriteEndElement();

            }

            return sb.ToString();
        }

        public string BatchInquiryGenerator(string xWebID, string authKey, string terminalID)
        {
            StringBuilder sb = new StringBuilder();

            XmlWriterSettings ws = new XmlWriterSettings();
            ws.Indent = true;

            using (XmlWriter xw = XmlWriter.Create(sb, ws))
            {
                xw.WriteStartDocument();

                xw.WriteStartElement("GatewayRequest");

                xw.WriteStartElement("SpecVersion");
                xw.WriteString("XWeb3.6");
                xw.WriteEndElement();

                xw.WriteStartElement("XWebID");
                xw.WriteString(xWebID);
                xw.WriteEndElement();

                xw.WriteStartElement("AuthKey");
                xw.WriteString(authKey);
                xw.WriteEndElement();

                xw.WriteStartElement("TerminalID");
                xw.WriteString(terminalID);
                xw.WriteEndElement();

                xw.WriteStartElement("POSType");
                xw.WriteString("PC");
                xw.WriteEndElement();

                xw.WriteStartElement("PinCapabilities");
                xw.WriteString("FALSE");
                xw.WriteEndElement();

                xw.WriteStartElement("TrackCapabilities");
                xw.WriteString("BOTH");
                xw.WriteEndElement();

                xw.WriteStartElement("TransactionType");
                xw.WriteString("BatchRequestTransaction");
                xw.WriteEndElement();

                xw.WriteStartElement("BatchTransactionType");
                xw.WriteString("INQUIRY");
                xw.WriteEndElement();
            }

            return sb.ToString();
        }
    }
}

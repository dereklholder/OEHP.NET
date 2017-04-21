using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;
using System.Xml.Serialization;
using System.Net;
using System.Web.Script.Serialization;

namespace EdgeExpressCloudSaaS
{
    public class EdgeEngine : VariableHandler
    {
        #region NonFinancialXMLBuilders
        public static string PromptCreditDebitXML() //Creates XML for a PromptCreditDebit Transaction
        {
            StringBuilder sb = new StringBuilder();
            XmlWriterSettings ws = new XmlWriterSettings();
            ws.Indent = false;
            ws.OmitXmlDeclaration = true;

            using (XmlWriter xml = XmlWriter.Create(sb, ws))
            {
                xml.WriteStartElement("XLINKEMVREQUEST");

                xml.WriteStartElement("TRANSACTIONTYPE");
                xml.WriteString("PPDPROMPTDEBITOPRCREDIT");
                xml.WriteEndElement();

                xml.WriteEndElement();
            }
            return sb.ToString();
        }
        public static string PromptSignatureXML() //Creates XML for Prompting for Signature after Transaction
        {
            StringBuilder sb = new StringBuilder();
            XmlWriterSettings ws = new XmlWriterSettings();
            ws.Indent = false;
            ws.OmitXmlDeclaration = true;

            using (XmlWriter xml = XmlWriter.Create(sb, ws))
            {
                xml.WriteStartElement("XLINKEMVREQUEST");

                xml.WriteStartElement("TRANSACTIONTYPE");
                xml.WriteString("PPDPROMPTSIGNATURE");
                xml.WriteEndElement();

                xml.WriteStartElement("TITLE");
                xml.WriteString("Please Sign");
                xml.WriteEndElement();

                xml.WriteStartElement("TEXT");
                xml.WriteString(Properties.Settings.Default.CardholderAgreement);
                xml.WriteEndElement();

                xml.WriteStartElement("SIGNATUREFILEFORMAT");
                xml.WriteString("BMP");
                xml.WriteEndElement();

                xml.WriteStartElement("RETURNSIGNATUREFORMAT");
                xml.WriteString("BASE64");
                xml.WriteEndElement();

                xml.WriteEndElement();

            }
            return sb.ToString();
        }
        public static string GetCapabilitiesSignatureXML() //Creates XML for CHecking for Signature Capability on Connected device
        {
            StringBuilder sb = new StringBuilder();
            XmlWriterSettings ws = new XmlWriterSettings();
            ws.Indent = false;
            ws.OmitXmlDeclaration = true;

            using (XmlWriter xml = XmlWriter.Create(sb, ws))
            {
                xml.WriteStartElement("XLINKEMVREQUEST");

                xml.WriteStartElement("TRANSACTIONTYPE");
                xml.WriteString("SUPPORTSFEATURE");
                xml.WriteEndElement();

                xml.WriteStartElement("SUPPORTED");
                xml.WriteString("Signature");
                xml.WriteEndElement();

                xml.WriteEndElement();

            }
            return sb.ToString();
        }
        #endregion
        #region FinancialXMLBuilders
        public static string CreditCardSaleXML(string amount) // Builds XML for CreditCardSale
        {
            StringBuilder sb = new StringBuilder();
            XmlWriterSettings ws = new XmlWriterSettings();
            ws.Indent = false;
            ws.OmitXmlDeclaration = true;

            using (XmlWriter xml = XmlWriter.Create(sb, ws))
            {
                xml.WriteStartElement("XLINKEMVREQUEST");

                xml.WriteStartElement("XWEBID");
                xml.WriteString(Properties.Settings.Default.XWebID);
                xml.WriteEndElement();

                xml.WriteStartElement("XWEBTERMINALID");
                xml.WriteString(Properties.Settings.Default.XWebTerminalID);
                xml.WriteEndElement();

                xml.WriteStartElement("XWEBAUTHKEY");
                xml.WriteString(Properties.Settings.Default.XWebAuthKey);
                xml.WriteEndElement();

                xml.WriteStartElement("TRANSACTIONTYPE");
                xml.WriteString("CREDITSALE");
                xml.WriteEndElement();

                xml.WriteStartElement("AMOUNT");
                xml.WriteString(amount);
                xml.WriteEndElement();

                xml.WriteStartElement("ALLOWDUPLICATES");
                xml.WriteEndElement();

                xml.WriteStartElement("CLERK");
                xml.WriteString(Properties.Settings.Default.Clerk);
                xml.WriteEndElement();

                xml.WriteEndElement();

            }

            return sb.ToString();
        }
        public static string DebitSaleXML(string amount) //Builds XML for DebitCardSale
        {
            {
                StringBuilder sb = new StringBuilder();
                XmlWriterSettings ws = new XmlWriterSettings();
                ws.Indent = false;
                ws.OmitXmlDeclaration = true;

                using (XmlWriter xml = XmlWriter.Create(sb, ws))
                {
                    xml.WriteStartElement("XLINKEMVREQUEST");

                    xml.WriteStartElement("XWEBID");
                    xml.WriteString(Properties.Settings.Default.XWebID);
                    xml.WriteEndElement();

                    xml.WriteStartElement("XWEBTERMINALID");
                    xml.WriteString(Properties.Settings.Default.XWebTerminalID);
                    xml.WriteEndElement();

                    xml.WriteStartElement("XWEBAUTHKEY");
                    xml.WriteString(Properties.Settings.Default.XWebAuthKey);
                    xml.WriteEndElement();

                    xml.WriteStartElement("TRANSACTIONTYPE");
                    xml.WriteString("DEBITSALE");
                    xml.WriteEndElement();

                    xml.WriteStartElement("AMOUNT");
                    xml.WriteString(amount);
                    xml.WriteEndElement();

                    xml.WriteStartElement("ALLOWDUPLICATES");
                    xml.WriteEndElement();

                    xml.WriteStartElement("CLERK");
                    xml.WriteString(Properties.Settings.Default.Clerk);
                    xml.WriteEndElement();

                    xml.WriteEndElement();

                }

                return sb.ToString();
            }
        }
        public static string CreditReturnXML(string amount, bool dependent, string transactionID) //Builds XML for Credit Return, Bool for Dependent vs Indepdent
        {
            {
                StringBuilder sb = new StringBuilder();
                XmlWriterSettings ws = new XmlWriterSettings();
                ws.Indent = false;
                ws.OmitXmlDeclaration = true;

                using (XmlWriter xml = XmlWriter.Create(sb, ws))
                {
                    xml.WriteStartElement("XLINKEMVREQUEST");

                    xml.WriteStartElement("XWEBID");
                    xml.WriteString(Properties.Settings.Default.XWebID);
                    xml.WriteEndElement();

                    xml.WriteStartElement("XWEBTERMINALID");
                    xml.WriteString(Properties.Settings.Default.XWebTerminalID);
                    xml.WriteEndElement();

                    xml.WriteStartElement("XWEBAUTHKEY");
                    xml.WriteString(Properties.Settings.Default.XWebAuthKey);
                    xml.WriteEndElement();

                    xml.WriteStartElement("TRANSACTIONTYPE");
                    xml.WriteString("CREDITRETURN");
                    xml.WriteEndElement();

                    xml.WriteStartElement("AMOUNT");
                    xml.WriteString(amount);
                    xml.WriteEndElement();

                    if (dependent == true)
                    {
                        xml.WriteStartElement("TRANSACTIONID");
                        xml.WriteString(transactionID);
                        xml.WriteEndElement();
                    }

                    xml.WriteStartElement("ALLOWDUPLICATES");
                    xml.WriteEndElement();

                    xml.WriteStartElement("CLERK");
                    xml.WriteString(Properties.Settings.Default.Clerk);
                    xml.WriteEndElement();

                    xml.WriteEndElement();

                }

                return sb.ToString();
            }
        }
        public static string DebitReturnXML(string amount) //Builds XML for DebitReturn
        {
            {
                StringBuilder sb = new StringBuilder();
                XmlWriterSettings ws = new XmlWriterSettings();
                ws.Indent = false;
                ws.OmitXmlDeclaration = true;

                using (XmlWriter xml = XmlWriter.Create(sb, ws))
                {
                    xml.WriteStartElement("XLINKEMVREQUEST");

                    xml.WriteStartElement("XWEBID");
                    xml.WriteString(Properties.Settings.Default.XWebID);
                    xml.WriteEndElement();

                    xml.WriteStartElement("XWEBTERMINALID");
                    xml.WriteString(Properties.Settings.Default.XWebTerminalID);
                    xml.WriteEndElement();

                    xml.WriteStartElement("XWEBAUTHKEY");
                    xml.WriteString(Properties.Settings.Default.XWebAuthKey);
                    xml.WriteEndElement();

                    xml.WriteStartElement("TRANSACTIONTYPE");
                    xml.WriteString("DEBITRETURN");
                    xml.WriteEndElement();

                    xml.WriteStartElement("AMOUNT");
                    xml.WriteString(amount);
                    xml.WriteEndElement();

                    xml.WriteStartElement("ALLOWDUPLICATES");
                    xml.WriteEndElement();

                    xml.WriteStartElement("CLERK");
                    xml.WriteString(Properties.Settings.Default.Clerk);
                    xml.WriteEndElement();

                    xml.WriteEndElement();

                }

                return sb.ToString();
            }
        }
        public static string CreditVoidXML(string transactionID) //Builds XML for Credit Void
        {
            StringBuilder sb = new StringBuilder();
            XmlWriterSettings ws = new XmlWriterSettings();
            ws.Indent = false;
            ws.OmitXmlDeclaration = true;

            using (XmlWriter xml = XmlWriter.Create(sb, ws))
            {
                xml.WriteStartElement("XLINKEMVREQUEST");

                xml.WriteStartElement("XWEBID");
                xml.WriteString(Properties.Settings.Default.XWebID);
                xml.WriteEndElement();

                xml.WriteStartElement("XWEBTERMINALID");
                xml.WriteString(Properties.Settings.Default.XWebTerminalID);
                xml.WriteEndElement();

                xml.WriteStartElement("XWEBAUTHKEY");
                xml.WriteString(Properties.Settings.Default.XWebAuthKey);
                xml.WriteEndElement();

                xml.WriteStartElement("TRANSACTIONTYPE");
                xml.WriteString("CREDITVOID");
                xml.WriteEndElement();

                xml.WriteStartElement("TRANSACTIONID");
                xml.WriteString(transactionID);
                xml.WriteEndElement();

                xml.WriteStartElement("ALLOWDUPLICATES");
                xml.WriteEndElement();

                xml.WriteStartElement("CLERK");
                xml.WriteString(Properties.Settings.Default.Clerk);
                xml.WriteEndElement();

                xml.WriteEndElement();

            }

            return sb.ToString();
        }
        #endregion
        #region EE Cloud Implementation
        public static string SendToEdgeExpressCloud(string parameters)
        {
            WebRequest wr = WebRequest.Create(RCMUrl + RCMMethod + RCMQuerystring + parameters);
            wr.Method = "GET";

            Stream objStream = wr.GetResponse().GetResponseStream();
            StreamReader sr = new StreamReader(objStream);

            return sr.ReadToEnd();
        }
#endregion
    }
    public class VariableHandler
    {
        public static string RCMUrl = "https://localsystem.paygateway.com:21113/RcmService.svc";
        public static string RCMMethod = "/Initialize";
        public static string RCMQuerystring = "?xl2Parameters=";
        public static string RCMStatusMethod = "/Status";
    }
}
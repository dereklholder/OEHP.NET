using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace EdgeExpressDemoPOS
{
    public class PaymentEngine
    {
        public static string SendToEdgeExpress(string parameters) // Sends listed Parameters to XL2, Output as string.
        {
            string response;
            XCharge.XpressLink2.XLEmv EdgeExpress = new XCharge.XpressLink2.XLEmv();
            EdgeExpress.Execute(parameters, out response);

            return response;
        }

        #region TransactionBasedXMLBuilders
        public static string BuildXMLSale(string XWebID, string XWebTerminalID, string XWebAuthKey, string amount, string clerkid) // Builds XML for a Simple SALE (Prompt for Debit/Credit) Transaction
        {
            StringBuilder sb = new StringBuilder();
            XmlWriterSettings ws = new XmlWriterSettings();
            ws.Indent = false;
            ws.OmitXmlDeclaration = true;

            using (XmlWriter xml = XmlWriter.Create(sb, ws))
            {
                xml.WriteStartElement("XLINKEMVREQUEST");

                xml.WriteStartElement("XWEBID");
                xml.WriteString(XWebID);
                xml.WriteEndElement();

                xml.WriteStartElement("XWEBTERMINALID");
                xml.WriteString(XWebTerminalID);
                xml.WriteEndElement();

                xml.WriteStartElement("XWEBAUTHKEY");
                xml.WriteString(XWebAuthKey);
                xml.WriteEndElement();

                xml.WriteStartElement("TRANSACTIONTYPE");
                xml.WriteString("SALE");
                xml.WriteEndElement();

                xml.WriteStartElement("AMOUNT");
                xml.WriteString(amount);
                xml.WriteEndElement();

                xml.WriteStartElement("ALLOWDUPLICATES");
                xml.WriteEndElement();

                xml.WriteStartElement("CLERK");
                xml.WriteString(clerkid);
                xml.WriteEndElement();

                xml.WriteEndElement();

            }

            return sb.ToString();
        }
        public static string BuildXMLCreditReturn(string XWebID, string XWebTerminalID, string XWebAuthKey, string amount, bool dependent, string transactionID, string clerkid) // Build XML for Credit Return
        {
            StringBuilder sb = new StringBuilder();
            XmlWriterSettings ws = new XmlWriterSettings();
            ws.Indent = false;
            ws.OmitXmlDeclaration = true;

            using (XmlWriter xml = XmlWriter.Create(sb, ws))
            {
                xml.WriteStartElement("XLINKEMVREQUEST");

                xml.WriteStartElement("XWEBID");
                xml.WriteString(XWebID);
                xml.WriteEndElement();

                xml.WriteStartElement("XWEBTERMINALID");
                xml.WriteString(XWebTerminalID);
                xml.WriteEndElement();

                xml.WriteStartElement("XWEBAUTHKEY");
                xml.WriteString(XWebAuthKey);
                xml.WriteEndElement();

                xml.WriteStartElement("TRANSACTIONTYPE");
                xml.WriteString("CREDITRETURN");
                xml.WriteEndElement();

                xml.WriteStartElement("ALLOWDUPLICATES");
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

                xml.WriteStartElement("CLERK");
                xml.WriteString(clerkid);
                xml.WriteEndElement();

                xml.WriteEndElement();

            }

            return sb.ToString();
        }
        public static string BuildXMLVoid(string XWebID, string XWebTerminalID, string XWebAuthKey, string TransactionID, string clerkid) // Build XMl for Credit Void
        {
            StringBuilder sb = new StringBuilder();
            XmlWriterSettings ws = new XmlWriterSettings();
            ws.Indent = false;
            ws.OmitXmlDeclaration = true;

            using (XmlWriter xml = XmlWriter.Create(sb, ws))
            {
                xml.WriteStartElement("XLINKEMVREQUEST");

                xml.WriteStartElement("XWEBID");
                xml.WriteString(XWebID);
                xml.WriteEndElement();

                xml.WriteStartElement("XWEBTERMINALID");
                xml.WriteString(XWebTerminalID);
                xml.WriteEndElement();

                xml.WriteStartElement("XWEBAUTHKEY");
                xml.WriteString(XWebAuthKey);
                xml.WriteEndElement();

                xml.WriteStartElement("TRANSACTIONTYPE");
                xml.WriteString("CREDITVOID");
                xml.WriteEndElement();

                xml.WriteStartElement("TRANSACTIONID");
                xml.WriteString(TransactionID);
                xml.WriteEndElement();

                xml.WriteStartElement("ALLOWDUPLICATES");
                xml.WriteEndElement();

                xml.WriteStartElement("CLERK");
                xml.WriteString(clerkid);
                xml.WriteEndElement();

                xml.WriteEndElement();

            }

            return sb.ToString();
        }
        public static string BuildXMLDebitReturn(string XWebID, string XWebTerminalID, string XWebAuthKey, string amount, string clerkid)
        {
            StringBuilder sb = new StringBuilder();
            XmlWriterSettings ws = new XmlWriterSettings();
            ws.Indent = false;
            ws.OmitXmlDeclaration = true;

            using (XmlWriter xml = XmlWriter.Create(sb, ws))
            {
                xml.WriteStartElement("XLINKEMVREQUEST");

                xml.WriteStartElement("XWEBID");
                xml.WriteString(XWebID);
                xml.WriteEndElement();

                xml.WriteStartElement("XWEBTERMINALID");
                xml.WriteString(XWebTerminalID);
                xml.WriteEndElement();

                xml.WriteStartElement("XWEBAUTHKEY");
                xml.WriteString(XWebAuthKey);
                xml.WriteEndElement();

                xml.WriteStartElement("TRANSACTIONTYPE");
                xml.WriteString("DEBITRETURN");
                xml.WriteEndElement();

                xml.WriteStartElement("ALLOWDUPLICATES");
                xml.WriteEndElement();


                xml.WriteStartElement("AMOUNT");
                xml.WriteString(amount);
                xml.WriteEndElement();

                xml.WriteStartElement("CLERK");
                xml.WriteString(clerkid);
                xml.WriteEndElement();

                xml.WriteEndElement();

            }

            return sb.ToString();
        }
        #endregion
        #region NonTransactionBasedXMLBuilders
        public static string BuildXMLSignaturePromptForPayment(string amount)
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

                xml.WriteStartElement("DISPLAYCAPTUREDSIGNATURE");
                xml.WriteString("F");
                xml.WriteEndElement();

                xml.WriteStartElement("TITLE");
                xml.WriteString("Please Sign");
                xml.WriteEndElement();

                xml.WriteStartElement("TEXT");
                xml.WriteString("Buyer Agrees to pay $" + amount + " according to the terms of the cardholder agreement.");
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
        public static string BuildXMLSignaturePromptForOther(string agreement)
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

                xml.WriteStartElement("DISPLAYCAPTURESIGNATURE");
                xml.WriteString("F");
                xml.WriteEndElement();

                xml.WriteStartElement("TITLE");
                xml.WriteString("Please Sign");
                xml.WriteEndElement();

                xml.WriteStartElement("TEXT");
                xml.WriteString(agreement);
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
        public static string BuildXMLSurvey(string question)
        {
            StringBuilder sb = new StringBuilder();
            XmlWriterSettings ws = new XmlWriterSettings();
            ws.Indent = false;
            ws.OmitXmlDeclaration = true;

            using (XmlWriter xml = XmlWriter.Create(sb, ws))
            {
                xml.WriteStartElement("XLINKEMVREQUEST");

                xml.WriteStartElement("TRANSACTIONTYPE");
                xml.WriteString("PPDPROMPTYESNO");
                xml.WriteEndElement();

                xml.WriteStartElement("TITLE");
                xml.WriteString("Survey");
                xml.WriteEndElement();

                xml.WriteStartElement("TEXT");
                xml.WriteString(question);
                xml.WriteEndElement();
                
                xml.WriteEndElement();
            }
            return sb.ToString();
        }
        #endregion

    }
    #region ParsingObjects
    [Serializable, XmlRoot("XLinkEMVResult")]
    public class SaleResultXML
    {
        public string DUPLICATECARD { get; set; }
        public string DATE_TIME { get; set; }
        public string HOSTRESPONSECODE { get; set; }
        public string HOSTRESPONSEDESCRIPTION { get; set; }
        public string RESULT { get; set; }
        public string RESULTMSG { get; set; }
        public string APPROVEDAMOUNT { get; set; }
        public string BATCHNO { get; set; }
        public string BATCHAMOUNT { get; set; }
        public string APPROVALCODE { get; set; }
        public string ACCOUNT { get; set; } //Masked Account Number
        public string CARDHOLDERNAME { get; set; }
        public string CARDTYPE { get; set; } //CREDIT OR DEBIT
        public string CARDBRAND { get; set; }
        public string CARDBRANDSHORT { get; set; }
        public string LANGUAGE { get; set; }
        public string ALIAS { get; set; }
        public string ENTRYTYPE { get; set; }
        public string RECEIPTTEXT { get; set; }
        public string EXPMONTH { get; set; }
        public string EXPYEAR { get; set; }
        public string TRANSACTIONID { get; set; }
        public string EMVTRANSACTION { get; set; }

    }
    [Serializable, XmlRoot("XLinkEMVResult")]
    public class PromptSignatureResultXML
    {
        public string SIGNATUREFORMAT { get; set; }
        public string SIGNATUREIMAGE { get; set; }
        public string RESULT { get; set; }
        public string RESULTMSG { get; set; }
    }
    [Serializable, XmlRoot("XLinkEMVResult")]
    public class PromptYesNoResultXML
    {
        public string YESORNO { get; set; }
        public string RESULT { get; set; }
    }
    [Serializable, XmlRoot("XlinkEMVResult")]
    public class DebitReturnResultXML
    {
        public string CASHBACKAMOUNT { get; set; }
        public string DUPLICATECARD { get; set; }
        public string DATE_TIME { get; set; }
        public string HOSTRESPONSECODE { get; set; }
        public string HOSTRESPONSEDESCRIPTION { get; set; }
        public string RESULT { get; set; }
        public string RESULTMSG { get; set; }
        public string APPROVEDAMOUNT { get; set; }
        public string BATCHNO { get; set; }
        public string ACCOUNT { get; set; }
        public string CARDHOLDERNAME { get; set; }
        public string CARDTYPE { get; set; }
        public string CARDBRAND { get; set; }
        public string ENTRYTYPE { get; set; }
        public string RECEIPTTEXT { get; set; }
        public string EXPMONTH { get; set; }
        public string EXPYEAR { get; set; }
        public string TRANSACTIONID { get; set; }
    }
    #endregion
}

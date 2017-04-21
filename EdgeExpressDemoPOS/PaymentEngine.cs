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
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace EdgeExpressDemoPOS
{
    public class PaymentEngine : VariableHandler
    {
        public static string SendToEdgeExpress(string parameters) // Sends listed Parameters to XL2, Output as string.
        {
            string response = null;
            if (Globals.Default.IntegrationMode == "PC")
            {
                XCharge.XpressLink2.XLEmv EdgeExpress = new XCharge.XpressLink2.XLEmv();
                EdgeExpress.Execute(parameters, out response);
            }
            if (Globals.Default.IntegrationMode == "Cloud")
            {
                response = SendToEdgeExpressCloud(parameters);
            }

            return response;
        }
        #region EE CLoud Implementeation
        /// <summary>
        /// Separate Region for CLOUD implementation due to some of the diffrences in response, and the more verbose nature of invoking the calls. 
        /// </summary>
        public static string SendToEdgeExpressCloud(string parameters) //Called to Send transactions to Cloud Rather than PC
        {
            string url = RCMUrl + RCMMethod + RCMQuerystring + parameters;
            HttpWebRequest getRequest = (HttpWebRequest)WebRequest.Create(url);
            getRequest.Method = "GET";
            getRequest.ContentType = "application/xml"; //Sets the response type as XML

            WebResponse response = getRequest.GetResponse();

            Stream dataStream = response.GetResponseStream();
            StreamReader responseReader = new StreamReader(dataStream);
            string rawResponse = responseReader.ReadToEnd();

            responseReader.Close();
            dataStream.Close();

            string result = GetContentContentOfXmlRcmResponse(rawResponse);

            return result;

        }
        public static string GetEECloudStatus(string sessionID)
        {
            WebRequest wr = WebRequest.Create(RCMUrl + RCMStatusMethod + "&sessionID" + sessionID);
            wr.Method = "GET";

            Stream objstream = wr.GetResponse().GetResponseStream();
            StreamReader sr = new StreamReader(objstream);

            return sr.ReadToEnd();
        }
        #endregion
        #region TransactionBasedXMLBuilders
        public static string BuildXMLSale(string XWebID, string XWebTerminalID, string XWebAuthKey, string amount, string clerkid) // Builds XML for a Simple SALE (Prompt for Debit/Credit) Transaction
        {
            StringBuilder sb = new StringBuilder();
            XmlWriterSettings ws = new XmlWriterSettings();
            ws.Indent = false;
            ws.OmitXmlDeclaration = true;

            using (XmlWriter xml = XmlWriter.Create(sb, ws))
            {
                xml.WriteStartElement("REQUEST");

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
                xml.WriteStartElement("REQUEST");

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
                xml.WriteStartElement("REQUEST");

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
                xml.WriteStartElement("REQUEST");

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
        public static bool TransactionSuccessful(TranType TransactionType, SaleResultXML sRX, DebitReturnResultXML dRX, CreditReturnResultXML cRX, CreditVoidResultXML cVX)
        {
            bool isSuccessful = false;
            switch (TransactionType.ToString())
            {

                case "Sale":
                    switch (sRX.RESULT)
                    {
                        case "0":
                            GeneralFunctions.WriteToLog(sRX.RECEIPTTEXT);
                            isSuccessful = true;
                            break;
                            ;
                        case "1":
                            MessageBox.Show("Error Has Occured" + Environment.NewLine + sRX.RESULTMSG);
                            GeneralFunctions.WriteToLog(sRX.RESULTMSG);
                            break;

                        case "2":
                            MessageBox.Show("Error Has Occured" + Environment.NewLine + sRX.RESULTMSG);
                            GeneralFunctions.WriteToLog(sRX.RESULTMSG);
                            break;

                        case "3":
                            MessageBox.Show("Error Has Occured" + Environment.NewLine + sRX.RESULTMSG);
                            GeneralFunctions.WriteToLog(sRX.RESULTMSG);
                            break;

                        case "4":
                            MessageBox.Show("Error Has Occured" + Environment.NewLine + sRX.RESULTMSG);
                            GeneralFunctions.WriteToLog(sRX.RESULTMSG);
                            break;

                        case "5":
                            MessageBox.Show("Error Has Occured" + Environment.NewLine + sRX.RESULTMSG);
                            GeneralFunctions.WriteToLog(sRX.RESULTMSG);
                            break;

                        case "6":
                            MessageBox.Show("Error Has Occured" + Environment.NewLine + sRX.RESULTMSG);
                            GeneralFunctions.WriteToLog(sRX.RESULTMSG);
                            break;

                        case "7":
                            MessageBox.Show("Error Has Occured" + Environment.NewLine + sRX.RESULTMSG);
                            GeneralFunctions.WriteToLog(sRX.RESULTMSG);
                            break;

                        case "8":
                            MessageBox.Show("Error Has Occured" + Environment.NewLine + sRX.RESULTMSG);
                            GeneralFunctions.WriteToLog(sRX.RESULTMSG);
                            break;

                        case "9":
                            MessageBox.Show("Error Has Occured" + Environment.NewLine + sRX.RESULTMSG);
                            GeneralFunctions.WriteToLog(sRX.RESULTMSG);
                            break;

                        case "10":
                            MessageBox.Show("Error Has Occured" + Environment.NewLine + sRX.RESULTMSG);
                            GeneralFunctions.WriteToLog(sRX.RESULTMSG);
                            break;

                        case "12":
                            MessageBox.Show("The Payment Has Been Declined");
                            GeneralFunctions.WriteToLog(sRX.RESULTMSG);
                            break;

                        case "14":
                            MessageBox.Show("Error Has Occured" + Environment.NewLine + sRX.RESULTMSG);
                            GeneralFunctions.WriteToLog(sRX.RESULTMSG);
                            break;

                        case "15":
                            MessageBox.Show("Error Has Occured" + Environment.NewLine + sRX.RESULTMSG);
                            GeneralFunctions.WriteToLog(sRX.RESULTMSG);
                            break;

                        case "19":
                            MessageBox.Show("Error Has Occured" + Environment.NewLine + sRX.RESULTMSG);
                            GeneralFunctions.WriteToLog(sRX.RESULTMSG);
                            break;

                        case "20":
                            MessageBox.Show("Error Has Occured" + Environment.NewLine + sRX.RESULTMSG);
                            GeneralFunctions.WriteToLog(sRX.RESULTMSG);
                            break;

                        case "100":
                            MessageBox.Show("Error Has Occured" + Environment.NewLine + sRX.RESULTMSG);
                            GeneralFunctions.WriteToLog(sRX.RESULTMSG);
                            break;

                        case "101":
                            MessageBox.Show("Error Has Occured" + Environment.NewLine + sRX.RESULTMSG);
                            GeneralFunctions.WriteToLog(sRX.RESULTMSG);
                            break;

                        default:
                            MessageBox.Show("Payment Not Completed,An Undefined Error Occured, Please Try Again");
                            GeneralFunctions.WriteToLog(sRX.RESULTMSG);
                            break;
                    }
                    break;
                case "DebitReturn":
                    switch (dRX.RESULT)
                    {
                        case "0":
                            GeneralFunctions.WriteToLog(dRX.RECEIPTTEXT);
                            isSuccessful = true;
                            break;
                            ;
                        case "1":
                            MessageBox.Show("Error Has Occured" + Environment.NewLine + dRX.RESULTMSG);
                            GeneralFunctions.WriteToLog(dRX.RESULTMSG);
                            break;

                        case "2":
                            MessageBox.Show("Error Has Occured" + Environment.NewLine + dRX.RESULTMSG);
                            GeneralFunctions.WriteToLog(dRX.RESULTMSG);
                            break;

                        case "3":
                            MessageBox.Show("Error Has Occured" + Environment.NewLine + dRX.RESULTMSG);
                            GeneralFunctions.WriteToLog(dRX.RESULTMSG);
                            break;

                        case "4":
                            MessageBox.Show("Error Has Occured" + Environment.NewLine + dRX.RESULTMSG);
                            GeneralFunctions.WriteToLog(dRX.RESULTMSG);
                            break;

                        case "5":
                            MessageBox.Show("Error Has Occured" + Environment.NewLine + dRX.RESULTMSG);
                            GeneralFunctions.WriteToLog(dRX.RESULTMSG);
                            break;

                        case "6":
                            MessageBox.Show("Error Has Occured" + Environment.NewLine + dRX.RESULTMSG);
                            GeneralFunctions.WriteToLog(dRX.RESULTMSG);
                            break;

                        case "7":
                            MessageBox.Show("Error Has Occured" + Environment.NewLine + dRX.RESULTMSG);
                            GeneralFunctions.WriteToLog(dRX.RESULTMSG);
                            break;

                        case "8":
                            MessageBox.Show("Error Has Occured" + Environment.NewLine + dRX.RESULTMSG);
                            GeneralFunctions.WriteToLog(dRX.RESULTMSG);
                            break;

                        case "9":
                            MessageBox.Show("Error Has Occured" + Environment.NewLine + dRX.RESULTMSG);
                            GeneralFunctions.WriteToLog(dRX.RESULTMSG);
                            break;

                        case "10":
                            MessageBox.Show("Error Has Occured" + Environment.NewLine + dRX.RESULTMSG);
                            GeneralFunctions.WriteToLog(dRX.RESULTMSG);
                            break;

                        case "12":
                            MessageBox.Show("The Payment Has Been Declined");
                            GeneralFunctions.WriteToLog(dRX.RESULTMSG);
                            break;

                        case "14":
                            MessageBox.Show("Error Has Occured" + Environment.NewLine + dRX.RESULTMSG);
                            GeneralFunctions.WriteToLog(dRX.RESULTMSG);
                            break;

                        case "15":
                            MessageBox.Show("Error Has Occured" + Environment.NewLine + dRX.RESULTMSG);
                            GeneralFunctions.WriteToLog(dRX.RESULTMSG);
                            break;

                        case "19":
                            MessageBox.Show("Error Has Occured" + Environment.NewLine + dRX.RESULTMSG);
                            GeneralFunctions.WriteToLog(dRX.RESULTMSG);
                            break;

                        case "20":
                            MessageBox.Show("Error Has Occured" + Environment.NewLine + dRX.RESULTMSG);
                            GeneralFunctions.WriteToLog(dRX.RESULTMSG);
                            break;

                        case "100":
                            MessageBox.Show("Error Has Occured" + Environment.NewLine + dRX.RESULTMSG);
                            GeneralFunctions.WriteToLog(dRX.RESULTMSG);
                            break;

                        case "101":
                            MessageBox.Show("Error Has Occured" + Environment.NewLine + dRX.RESULTMSG);
                            GeneralFunctions.WriteToLog(dRX.RESULTMSG);
                            break;

                        default:
                            MessageBox.Show("Payment Not Completed, An Undefined Error Occured, Please Try Again");
                            GeneralFunctions.WriteToLog(dRX.RESULTMSG);
                            break;
                    }
                    break;
                case "CreditReturn":
                    switch (cRX.RESULT)
                    {
                        case "0":
                            GeneralFunctions.WriteToLog(cRX.RECEIPTTEXT);
                            isSuccessful = true;
                            break;
                            ;
                        case "1":
                            MessageBox.Show("Error Has Occured" + Environment.NewLine + cRX.RESULTMSG);
                            GeneralFunctions.WriteToLog(cRX.RESULTMSG);
                            break;

                        case "2":
                            MessageBox.Show("Error Has Occured" + Environment.NewLine + cRX.RESULTMSG);
                            GeneralFunctions.WriteToLog(cRX.RESULTMSG);
                            break;

                        case "3":
                            MessageBox.Show("Error Has Occured" + Environment.NewLine + cRX.RESULTMSG);
                            GeneralFunctions.WriteToLog(cRX.RESULTMSG);
                            break;

                        case "4":
                            MessageBox.Show("Error Has Occured" + Environment.NewLine + cRX.RESULTMSG);
                            GeneralFunctions.WriteToLog(cRX.RESULTMSG);
                            break;

                        case "5":
                            MessageBox.Show("Error Has Occured" + Environment.NewLine + cRX.RESULTMSG);
                            GeneralFunctions.WriteToLog(cRX.RESULTMSG);
                            break;

                        case "6":
                            MessageBox.Show("Error Has Occured" + Environment.NewLine + cRX.RESULTMSG);
                            GeneralFunctions.WriteToLog(cRX.RESULTMSG);
                            break;

                        case "7":
                            MessageBox.Show("Error Has Occured" + Environment.NewLine + cRX.RESULTMSG);
                            GeneralFunctions.WriteToLog(cRX.RESULTMSG);
                            break;

                        case "8":
                            MessageBox.Show("Error Has Occured" + Environment.NewLine + cRX.RESULTMSG);
                            GeneralFunctions.WriteToLog(cRX.RESULTMSG);
                            break;

                        case "9":
                            MessageBox.Show("Error Has Occured" + Environment.NewLine + cRX.RESULTMSG);
                            GeneralFunctions.WriteToLog(cRX.RESULTMSG);
                            break;

                        case "10":
                            MessageBox.Show("Error Has Occured" + Environment.NewLine + cRX.RESULTMSG);
                            GeneralFunctions.WriteToLog(cRX.RESULTMSG);
                            break;

                        case "12":
                            MessageBox.Show("The Payment Has Been Declined");
                            GeneralFunctions.WriteToLog(cRX.RESULTMSG);
                            break;

                        case "14":
                            MessageBox.Show("Error Has Occured" + Environment.NewLine + cRX.RESULTMSG);
                            GeneralFunctions.WriteToLog(cRX.RESULTMSG);
                            break;

                        case "15":
                            MessageBox.Show("Error Has Occured" + Environment.NewLine + cRX.RESULTMSG);
                            GeneralFunctions.WriteToLog(cRX.RESULTMSG);
                            break;

                        case "19":
                            MessageBox.Show("Error Has Occured" + Environment.NewLine + cRX.RESULTMSG);
                            GeneralFunctions.WriteToLog(cRX.RESULTMSG);
                            break;

                        case "20":
                            MessageBox.Show("Error Has Occured" + Environment.NewLine + cRX.RESULTMSG);
                            GeneralFunctions.WriteToLog(cRX.RESULTMSG);
                            break;

                        case "100":
                            MessageBox.Show("Error Has Occured" + Environment.NewLine + cRX.RESULTMSG);
                            GeneralFunctions.WriteToLog(cRX.RESULTMSG);
                            break;

                        case "101":
                            MessageBox.Show("Error Has Occured" + Environment.NewLine + cRX.RESULTMSG);
                            GeneralFunctions.WriteToLog(cRX.RESULTMSG);
                            break;

                        default:
                            MessageBox.Show("Payment Not Completed, An Undefined Error Occured, Please Try Again");
                            GeneralFunctions.WriteToLog(cRX.RESULTMSG);
                            break;
                    }
                    break;
                case "Void":
                    switch (cVX.RESULT)
                    {
                        case "0":
                            GeneralFunctions.WriteToLog(cVX.RECEIPTTEXT);
                            isSuccessful = true;
                            break;
                            ;
                        case "1":
                            MessageBox.Show("Error Has Occured" + Environment.NewLine + cVX.RESULTMSG);
                            GeneralFunctions.WriteToLog(cVX.RESULTMSG);
                            break;

                        case "2":
                            MessageBox.Show("Error Has Occured" + Environment.NewLine + cVX.RESULTMSG);
                            GeneralFunctions.WriteToLog(cVX.RESULTMSG);
                            break;

                        case "3":
                            MessageBox.Show("Error Has Occured" + Environment.NewLine + cVX.RESULTMSG);
                            GeneralFunctions.WriteToLog(cVX.RESULTMSG);
                            break;

                        case "4":
                            MessageBox.Show("Error Has Occured" + Environment.NewLine + cVX.RESULTMSG);
                            GeneralFunctions.WriteToLog(cVX.RESULTMSG);
                            break;

                        case "5":
                            MessageBox.Show("Error Has Occured" + Environment.NewLine + cVX.RESULTMSG);
                            GeneralFunctions.WriteToLog(cVX.RESULTMSG);
                            break;

                        case "6":
                            MessageBox.Show("Error Has Occured" + Environment.NewLine + cVX.RESULTMSG);
                            GeneralFunctions.WriteToLog(cVX.RESULTMSG);
                            break;

                        case "7":
                            MessageBox.Show("Error Has Occured" + Environment.NewLine + cVX.RESULTMSG);
                            GeneralFunctions.WriteToLog(cVX.RESULTMSG);
                            break;

                        case "8":
                            MessageBox.Show("Error Has Occured" + Environment.NewLine + cVX.RESULTMSG);
                            GeneralFunctions.WriteToLog(cVX.RESULTMSG);
                            break;

                        case "9":
                            MessageBox.Show("Error Has Occured" + Environment.NewLine + cVX.RESULTMSG);
                            GeneralFunctions.WriteToLog(cVX.RESULTMSG);
                            break;

                        case "10":
                            MessageBox.Show("Error Has Occured" + Environment.NewLine + cVX.RESULTMSG);
                            GeneralFunctions.WriteToLog(cVX.RESULTMSG);
                            break;

                        case "12":
                            MessageBox.Show("The Payment Has Been Declined");
                            GeneralFunctions.WriteToLog(cVX.RESULTMSG);
                            break;

                        case "14":
                            MessageBox.Show("Error Has Occured" + Environment.NewLine + cVX.RESULTMSG);
                            GeneralFunctions.WriteToLog(cVX.RESULTMSG);
                            break;

                        case "15":
                            MessageBox.Show("Error Has Occured" + Environment.NewLine + cVX.RESULTMSG);
                            GeneralFunctions.WriteToLog(cVX.RESULTMSG);
                            break;

                        case "19":
                            MessageBox.Show("Error Has Occured" + Environment.NewLine + cVX.RESULTMSG);
                            GeneralFunctions.WriteToLog(cVX.RESULTMSG);
                            break;

                        case "20":
                            MessageBox.Show("Error Has Occured" + Environment.NewLine + cVX.RESULTMSG);
                            GeneralFunctions.WriteToLog(cVX.RESULTMSG);
                            break;

                        case "100":
                            MessageBox.Show("Error Has Occured" + Environment.NewLine + cVX.RESULTMSG);
                            GeneralFunctions.WriteToLog(cVX.RESULTMSG);
                            break;

                        case "101":
                            MessageBox.Show("Error Has Occured" + Environment.NewLine + cRX.RESULTMSG);
                            GeneralFunctions.WriteToLog(cVX.RESULTMSG);
                            break;

                        default:
                            MessageBox.Show("Payment Not Completed, An Undefined Error Occured, Please Try Again");
                            GeneralFunctions.WriteToLog(cVX.RESULTMSG);
                            break;
                    }
                    break;
                default:
                    MessageBox.Show("Unexpected Result.");
                    GeneralFunctions.WriteToLog("Unexpected Result Happened");
                    break;
            }
            return isSuccessful;
        }
        #region NonTransactionBasedXMLBuilders
        public static string BuildXMLSignaturePromptForPayment(string amount)
        {
            StringBuilder sb = new StringBuilder();
            XmlWriterSettings ws = new XmlWriterSettings();
            ws.Indent = false;
            ws.OmitXmlDeclaration = true;

            using (XmlWriter xml = XmlWriter.Create(sb, ws))
            {
                xml.WriteStartElement("REQUEST");

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
                xml.WriteStartElement("REQUEST");

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
                xml.WriteStartElement("REQUEST");

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
        public static string BuildXMLSupportSignature()
        {
            StringBuilder sb = new StringBuilder();
            XmlWriterSettings ws = new XmlWriterSettings();
            ws.Indent = false;
            ws.OmitXmlDeclaration = true;
            using (XmlWriter xml = XmlWriter.Create(sb, ws))
            {
                xml.WriteStartElement("REQUEST");

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
        #region Parsers
        public static string GetContentContentOfXmlRcmResponse(string cloudTransactionResponseString) // Reads the XML to get only the Parent/Child Nodes I care about (RESPONSE and all Childs)
        {
            string parsedData = null;
            using (XmlTextReader xr = new XmlTextReader(new StringReader(cloudTransactionResponseString)))
            {
                while (xr.Read())
                {
                    while (xr.ReadToFollowing("XmlRcmResponse"))
                    {

                        parsedData = Regex.Replace(xr.ReadInnerXml(), @"\t|\n|\r", ""); //Get rid of escape characters, because they are lame.
                    }
                }
                xr.Close();


                return parsedData;
            }

        }
        public static T FromXml<T>(String xml) //Deserializes Result into one of the defined result object classes.
        {
            T returnedXmlClass = default(T);
            try
            {
                using (TextReader reader = new StringReader(xml))
                {
                    try
                    {
                        returnedXmlClass = (T)new XmlSerializer(typeof(T)).Deserialize(reader);
                    }
                    catch (InvalidOperationException)
                    {
                        //String Passed is not XML, simply return defaultXMlClass
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetBaseException().ToString());
            }
            return returnedXmlClass;
        }
        #endregion
    }
    #region EdgeExpress ParsingObjects

    [Serializable, XmlRoot("RESPONSE")]
    public class GetSignatureSupportedResultXML
    {
        public string SUPPORTED { get; set; }
        public string RESULT { get; set; }
        public string RESULTMSG { get; set; }
    }
    [Serializable, XmlRoot("RESPONSE")]
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
    [Serializable, XmlRoot("RESPONSE")]
    public class PromptSignatureResultXML
    {
        public string SIGNATUREFORMAT { get; set; }
        public string SIGNATUREIMAGE { get; set; }
        public string RESULT { get; set; }
        public string RESULTMSG { get; set; }
    }
    [Serializable, XmlRoot("RESPONSE")]
    public class PromptYesNoResultXML
    {
        public string YESORNO { get; set; }
        public string RESULT { get; set; }
    }
    [Serializable, XmlRoot("RESPONSE")]
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
    [Serializable, XmlRoot("RESPONSE")]
    public class CreditReturnResultXML
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
    }
    [Serializable, XmlRoot("RESPONSE")]
    public class CreditVoidResultXML
    {
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
    }

    #endregion
    interface IEdgeExpress
    {
        string SendToEdgeExpress();
        bool TransactionSuccessful();

    }
}

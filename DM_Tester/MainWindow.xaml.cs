using System;
using System.Collections.Generic;
using System.IO;
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
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace DM_Tester
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            InitializeComponent();
        }

        private void UpdateAliasButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ResultTextBox.Text = "";
                PaymentEngine pe = new PaymentEngine();
                string request = pe.UpdateAliasGenerator(AliasBox.Text, Globals.Default.XWebID, Globals.Default.AuthKey, Globals.Default.TerminalID, acctNumBox.Text, expDateBox.Text);
                string result = pe.CallGateway(request, VariableHandler.TestDtGURL);
                string[] success = ResultReader.IsSuccess(result);

                if (success[0] == "005")
                {
                    string[] results = ResultReader.GetResult(result);
                    if (results != null)
                    {
                        string maskedCC = results[0].ToString();
                        string span = maskedCC.Substring(maskedCC.Length - 4);
                        ResultTextBox.Text = "SPAN: " + span + Environment.NewLine + "Expiration Date: " + results[1];
                    }

                }
                else
                {
                    ResultTextBox.Text = "An Error Occured while Processing AliasUpdateTransaction, Please Try Again." + Environment.NewLine + "Description: " + success[1];
                }
            }
            catch (NullReferenceException)
            {
               
            }

        }

        private void exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void credentials_Click(object sender, RoutedEventArgs e)
        {
            Credentials cred = new Credentials();
            cred.ShowDialog();
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            
            switch (FunctionComboBox.SelectedValue.ToString())
            {
                case "UpdateAlias":
                    ResultTextBox.Text = "";
                    PaymentEngine pe = new PaymentEngine();
                    string request = pe.UpdateAliasGenerator(AliasBox.Text, Globals.Default.XWebID, Globals.Default.AuthKey, Globals.Default.TerminalID, acctNumBox.Text, expDateBox.Text);
                    string result = pe.CallGateway(request, VariableHandler.TestDtGURL);
                    string[] success = ResultReader.IsSuccess(result);

                    if (success[0] == "005")
                    {
                        string[] results = ResultReader.GetResult(result);
                        if (results != null)
                        {
                            string maskedCC = results[0].ToString();
                            string span = maskedCC.Substring(maskedCC.Length - 4);
                            ResultTextBox.Text = "SPAN: " + span + Environment.NewLine + "Expiration Date: " + results[1];
                        }

                    }
                    else
                    {
                        ResultTextBox.Text = "An Error Occured while Processing AliasUpdateTransaction, Please Try Again." + Environment.NewLine + "Description: " + success[1];
                    }
                    break;
                case "DeleteAlias":
                    ResultTextBox.Text = "";
                    PaymentEngine pe1 = new PaymentEngine();
                    string request1 = pe1.DeleteAliasGenerator(AliasBox.Text, Globals.Default.XWebID, Globals.Default.AuthKey, Globals.Default.TerminalID);
                    string result1 = pe1.CallGateway(request1, VariableHandler.TestDtGURL);
                    string[] success1 = ResultReader.IsSuccess(result1);

                    if (success1[0] == "005")
                    {
                        string[] results = ResultReader.GetResult(result1);
                        if (results != null)
                        {
                            
                            ResultTextBox.Text = success1[1];
                        }

                    }
                    else
                    {
                        ResultTextBox.Text = "An Error Occured while Processing AliasDeleteTransaction, Please Try Again." + Environment.NewLine + "Description: " + success1[1];
                    }
                    break;
                case "LookupAlias":
                    ResultTextBox.Text = "";
                    PaymentEngine pe2 = new PaymentEngine();
                    string request2 = pe2.DeleteAliasGenerator(AliasBox.Text, Globals.Default.XWebID, Globals.Default.AuthKey, Globals.Default.TerminalID);
                    string result2 = pe2.CallGateway(request2, VariableHandler.TestDtGURL);
                    string[] success2 = ResultReader.IsSuccess(result2);

                    if (success2[0] == "005")
                    {
                        string[] results = ResultReader.GetResult(result2);
                        if (results != null)
                        {
                            string maskedCC = results[0].ToString();
                            string span = maskedCC.Substring(maskedCC.Length - 4);
                            ResultTextBox.Text = "SPAN: " + span + Environment.NewLine + "Expiration Date: " + results[1];
                        }

                    }
                    else
                    {
                        ResultTextBox.Text = "An Error Occured while Processing AliasLookupTransaction, Please Try Again." + Environment.NewLine + "Description: " + success2[1];
                    }
                    break;
            }
        }

        private void FunctionComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
    [Serializable, XmlRoot("GatewayResponse")]
    public class GatewayResponse
    {
        public string ResponseCode { get; set; }
        public string ResponseDescription { get; set; }
        public string TransactionID { get; set; }
        public string CardType { get; set; }
        public string CardBrand { get; set; }
        public string CardBrandShort { get; set; }
        public string MaskedAcctNum { get; set; }
        public string ExpDate { get; set; }
    }
    

    public static class ResultReader
    {
        public static string[] IsSuccess(string xml)
        {
            

            var resultData = DeserializeFromXml<GatewayResponse>(xml);
            List<string> resultList = new List<string>();
            resultList.Add(resultData.ResponseCode);
            resultList.Add(resultData.ResponseDescription);
            string[] results = resultList.ToArray();

            return results;
        }

        public static string[] GetResult(string xml)
        {
            var resultData = DeserializeFromXml<GatewayResponse>(xml);
            List<string> resultList = new List<string>();
            resultList.Add(resultData.MaskedAcctNum);
            resultList.Add(resultData.ExpDate);
            string[] results = resultList.ToArray();
            return results;
        }
        public static string[] GetResponseCode(string xml)
        {
            var resultData = DeserializeFromXml<GatewayResponse>(xml);
            List<string> resultList = new List<string>();
            resultList.Add(resultData.ResponseCode);
            resultList.Add(resultData.ResponseDescription);
            string[] results = resultList.ToArray();
            return results;
        }
        public static T DeserializeFromXml<T>(string xml)
        {
            T result;
            XmlSerializer ser = new XmlSerializer(typeof(T));
            using (TextReader tr = new StringReader(xml))
            {
                result = (T)ser.Deserialize(tr);
            }
            return result;
        }

    }
}

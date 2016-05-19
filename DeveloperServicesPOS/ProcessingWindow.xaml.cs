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
using System.Windows.Shapes;

namespace DeveloperServicesPOS
{
    /// <summary>
    /// Interaction logic for ProcessingWindow.xaml
    /// </summary>
    public partial class ProcessingWindow : Window
    {
        public ProcessingWindow(string PayPageUrl, string sentTransactionType, string orderID)
        {
            InitializeComponent();
            OEHPBrowser.Navigate(PayPageUrl);
            TransactionType = sentTransactionType;
            OrderID = orderID;
        }
        public string TransactionType { get; set; } 
        public string OrderID { get; set; }
        public string EndTransactionStatus { get; set; }

        private void OEHPBrowser_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void OEHPBrowser_LoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            string queryParameters;
            TransactionRequest tr = new TransactionRequest();
            OEHP.NET.GatewayRequest gr = new OEHP.NET.GatewayRequest();
            OEHP.NET.DataManipulation dm = new OEHP.NET.DataManipulation();
            string paymentFinishedSignal = PaymentFinishedSignal(GetPageContent(OEHPBrowser));

            if (paymentFinishedSignal == "done")
            {
                switch (TransactionType)
                {
                    case "CREDIT_CARD":
                        queryParameters = tr.DirectPostBuilder(Properties.Settings.Default.AccountToken, OrderID, TransactionType, "QUERY_PAYMENT");
                        string creditCardResult = gr.TestDirectPost(queryParameters);
                        char firstChar = creditCardResult[0];
                        if (firstChar.ToString() == "r")
                        {
                            string result = dm.QueryStringToJson(creditCardResult);
                            var qro = dm.QueryResultObject(result);
                            EndTransactionStatus = qro.response_code;
                            OKButton.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            //Do Nothing
                        }
                        break;

                    case "DEBIT_CARD":
                        queryParameters = tr.DirectPostBuilder(Properties.Settings.Default.AccountToken, OrderID, TransactionType, "QUERY_PURCHASE");
                        string debitCardResult = gr.TestDirectPost(queryParameters);
                        char firstDChar = debitCardResult[0];
                        if (firstDChar.ToString() == "r")
                        {
                            string result = dm.QueryStringToJson(debitCardResult);
                            var qro = dm.QueryResultObject(result);
                            EndTransactionStatus = qro.response_code;
                            OKButton.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            //Do Nothing
                        }
                        break;

                }
            }

        }

        public string PaymentFinishedSignal(string pageHTML)
        {
            if (pageHTML != null)
            {
                HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(pageHTML);
                try
                {
                    var value = doc.DocumentNode.SelectSingleNode("//input[@type='hidden' and @id='paymentFinishedSignal']").Attributes["value"].Value;
                    return value;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        public string GetPageContent(WebBrowser wb)
        {
            if (wb != null)
            {
                return ((mshtml.HTMLDocumentClass)wb.Document).body.innerHTML;
            }
            else
            {
                return null;
            }
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.GlobalVariables.LastTransactionResult = EndTransactionStatus;
            this.Close();
        }
    }
}

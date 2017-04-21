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
using System.Xml.Serialization;
using System.Xml;


namespace EdgeExpressDemoPOS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }   
        #region Parsers
        
        public T FromXml<T>(String xml)
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
        #region NonTransactionUIControls
        private void changeUser_Click(object sender, RoutedEventArgs e)
        {
            ChangeUserWindow ch = new ChangeUserWindow();
            ch.ShowDialog();
        }
        private void exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void settings_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow sw = new SettingsWindow();
            sw.ShowDialog();
        }
        private void createDB_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("This will delete the current database, do you want to continue?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                //do no stuff
            }
            else
            {
                DBFunctions.CreateDBFile();

            }
        }
        #region How do I hold on to all these lemons?
        private string GetTransactionIDForReturnVoid() //Displays Dialog for entering transactionID that is returned as string to caller
        {
            TransactionIDLookupWindow lookupTransaction = new TransactionIDLookupWindow();

            lookupTransaction.ShowDialog();
            string transactionID = lookupTransaction.TransactionIDBox.Text;

            return transactionID;

        }
        #endregion

        #endregion
        #region TransactionMethodsWithResultLogic
        /// <summary>
        /// UI Controls should use these methods.
        /// </summary>
        private bool SaleTransaction(string amount)
        {
            SaleResultXML result = SendSaleTransaction(amount);
            DBFunctions.InsertSaleTransaction(result);
            if (PaymentEngine.TransactionSuccessful(TranType.Sale, result, null, null, null) == true)
            {
                if (Globals.Default.SignatureCaptureEnabled == true)
                {                 
                    PromptSignatureResultXML signatureObject = SendSignatureAfterSaleTransaction(amount);
                    DBFunctions.InsertSignatureTransaction(signatureObject, result.TRANSACTIONID);
                }
                MessageBox.Show(result.RECEIPTTEXT);
                return true;
            }
            else
            {
                return false;
            }
        }
        private void DebitReturnTransaction(string amount)
        {
            DebitReturnResultXML result = SendDebitReturnTransaction(amount);
            DBFunctions.InsertDebitReturnTransaction(result);
            if (PaymentEngine.TransactionSuccessful(TranType.DebitReturn, null, result, null, null) == true)
            {
                MessageBox.Show(result.RECEIPTTEXT);
            }
            else
            {
                // TransactionSuccessful should have result in message, return to previous screen (do nothing) may change logic later.
            }

        }
        private void CreditReturnTransaction(string amount, bool dependent, string transactionID)
        {
            CreditReturnResultXML result = SendCreditReturnTransaction(amount, dependent, transactionID);
            DBFunctions.InsertCreditReturnTransaction(result);
            if (PaymentEngine.TransactionSuccessful(TranType.CreditReturn, null, null, result, null) == true)
            {
                MessageBox.Show(result.RECEIPTTEXT);
            }
            else
            {
                // TransactionSuccessful should have result in message, return to previous screen (do nothing) may change logic later.
            }
        }
        private void CreditVoidTransaction(string transactionID)
        {
            CreditVoidResultXML result = SendCreditVoidTransaction(transactionID);
            DBFunctions.InsertVoidTransaction(result);
            if (PaymentEngine.TransactionSuccessful(TranType.Void, null, null, null, result) == true)
            {
                MessageBox.Show(result.RECEIPTTEXT);
            }
            else
            {
                // TransactionSuccessful should have result in message, return to previous screen (do nothing) may change logic later.
            }
        }
        #endregion
        #region EdgeExpressRequestMethods
        /// <summary>
        /// These are not used For Interactions with UI Controls.
        /// </summary>
        private SaleResultXML SendSaleTransaction(string amount)
        {
            string parameters = PaymentEngine.BuildXMLSale(Globals.Default.XWebID, Globals.Default.XWebTerminalID, 
                Globals.Default.XWebAuthKey, amount, Globals.Default.ClerkID);
            SaleResultXML result = FromXml<SaleResultXML>(PaymentEngine.SendToEdgeExpress(parameters));

            return result;
        }
        private SaleResultXML SendSaleTransactionCloud(string amount)
        {
            string parameters = PaymentEngine.BuildXMLSale(Globals.Default.XWebID, Globals.Default.XWebTerminalID, Globals.Default.XWebAuthKey, amount, Globals.Default.ClerkID);
            SaleResultXML result = FromXml<SaleResultXML>(PaymentEngine.SendToEdgeExpress(parameters));
            return result;
        }
        private DebitReturnResultXML SendDebitReturnTransaction(string amount)
        {
            string parameters = PaymentEngine.BuildXMLDebitReturn(Globals.Default.XWebID, Globals.Default.XWebTerminalID, Globals.Default.XWebAuthKey, amount, Globals.Default.ClerkID);
            DebitReturnResultXML result = FromXml<DebitReturnResultXML>(PaymentEngine.SendToEdgeExpress(parameters));

            return result;
        }
        private CreditReturnResultXML SendCreditReturnTransaction(string amount, bool dependent, string transactionID)
        {
            string parameters = PaymentEngine.BuildXMLCreditReturn(Globals.Default.XWebID, Globals.Default.XWebTerminalID, Globals.Default.XWebAuthKey, amount, dependent, transactionID, Globals.Default.ClerkID);
            CreditReturnResultXML result = FromXml<CreditReturnResultXML>(PaymentEngine.SendToEdgeExpress(parameters));

            return result;
        }
        private CreditVoidResultXML SendCreditVoidTransaction(string transactionID)
        {
            string parameters = PaymentEngine.BuildXMLVoid(Globals.Default.XWebID, Globals.Default.XWebTerminalID, Globals.Default.XWebAuthKey, transactionID, Globals.Default.ClerkID);
            CreditVoidResultXML result = FromXml<CreditVoidResultXML>(PaymentEngine.SendToEdgeExpress(parameters));

            return result;
        }
        private PromptSignatureResultXML SendSignatureAfterSaleTransaction(string amount)
        {
            string parameters = PaymentEngine.BuildXMLSignaturePromptForPayment(amount);
            PromptSignatureResultXML result = FromXml<PromptSignatureResultXML>(PaymentEngine.SendToEdgeExpress(parameters));

            Globals.Default.TransactionCounter++;
            Globals.Default.Save();

            return result;
        }
        private PromptSignatureResultXML SendSignatureForOtherTransaction(string agreement)
        {
            string parameters = PaymentEngine.BuildXMLSignaturePromptForOther(agreement);
            PromptSignatureResultXML result = FromXml<PromptSignatureResultXML>(PaymentEngine.SendToEdgeExpress(parameters));

            return result;
        }
        #endregion
        #region TicketListControls
        private void currentTicketList_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ListView listView = sender as ListView;
            GridView gView = listView.View as GridView;

            var workingWidth = listView.ActualWidth - 10; // take into account vertical scrollbar
            var col1 = 0.60;
            var col2 = 0.40;

            gView.Columns[0].Width = workingWidth * col1;
            gView.Columns[1].Width = workingWidth * col2;
        }
        private void widgetButton_Click(object sender, RoutedEventArgs e) // Better implementation would be to add dynamically, but this works.
        {
            currentTicketList.Items.Add(new ItemController { Item = "Widget", Price = Globals.Default.WidgetPrice });

            double currentPrice = double.Parse(totalAmountBox.Text);
            totalAmountBox.Text = (currentPrice + Globals.Default.WidgetPrice).ToString();

        }

        private void betterWidgetButton_Click(object sender, RoutedEventArgs e)
        {
            currentTicketList.Items.Add(new ItemController { Item = "Better Widget", Price = Globals.Default.BetterWidgetPrice });

            double currentPrice = double.Parse(totalAmountBox.Text);
            totalAmountBox.Text = (currentPrice + Globals.Default.BetterWidgetPrice).ToString();
        }

        private void bestWidgetButton_Click(object sender, RoutedEventArgs e)
        {
            currentTicketList.Items.Add(new ItemController { Item = "Best Widget", Price = Globals.Default.BestWidgetPrice });

            double currentPrice = double.Parse(totalAmountBox.Text);
            totalAmountBox.Text = (currentPrice + Globals.Default.BestWidgetPrice).ToString();
        }
        private void clearTicketListButton_Click(object sender, RoutedEventArgs e)
        {
            ClearTicketList();
        }
        public void ClearTicketList()
        {
            currentTicketList.Items.Clear();
            totalAmountBox.Text = "0";
        }
        #endregion
        #region UIProcessingControls
        private void processSaleButton_Click(object sender, RoutedEventArgs e)
        {
            string amount = totalAmountBox.Text;
            bool successfulSale = SaleTransaction(amount);
            if (successfulSale == true)
            {
                ClearTicketList();
            }
            else
            {
                //Sale Unsuccessful, Leave current ticket list in place, Do nothing
            }
        }

        private void returnTransaction_Click(object sender, RoutedEventArgs e)
        {
            string transactionID = GetTransactionIDForReturnVoid();
            CardTypeAndAmount cardType = DBFunctions.GetTransactionType(transactionID);

            if (String.IsNullOrEmpty(transactionID) == false && String.IsNullOrEmpty(cardType.amount) == false)
            {
                RunReturnWindow returnWindow = new RunReturnWindow(cardType.amount);
                returnWindow.ShowDialog();
                if (returnWindow.ContinueWithTransaction == "Yes")
                {
                    switch (cardType.cardType)
                    {
                        case "Credit":
                            CreditReturnTransaction(cardType.amount, true, transactionID);
                            break;
                        case "Debit":
                            DebitReturnTransaction(cardType.amount);
                            break;
                        default:
                            MessageBox.Show("Undefined Card Type, Return must be run as Independent Credit");
                            break;
                    }
                }
                else
                {
                    //Did not want to Continue, Do not Proceed.
                }

            }
            else
            {
                // No TransactionID sent or TransactionID was invalid
                MessageBox.Show("Invalid Transaction ID");
            }

        }

        private void voidTransaction_Click(object sender, RoutedEventArgs e)
        {
            string transactionID = GetTransactionIDForReturnVoid();
            CardTypeAndAmount cardType = DBFunctions.GetTransactionType(transactionID);

            if (String.IsNullOrEmpty(transactionID) == false && String.IsNullOrEmpty(cardType.amount) == false)
            {
                if (MessageBox.Show("This will Void this transaction amounting: $" + cardType.amount, "Void", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    switch (cardType.cardType)
                    {
                        case "Credit":
                            CreditVoidTransaction(transactionID);
                            break;
                        case "Debit":
                            MessageBox.Show("Debit Transactions Cannot Be Voided, Please run a return.");
                            break;
                        default:
                            MessageBox.Show("Undefined Card Type, Transaction cannot be voided.");
                            break;
                    }
                }
                else
                {
                    //Did not want to Continue, Do not Proceed.
                }

            }
        }
        private void signatureLookup_Click(object sender, RoutedEventArgs e)
        {
            string transactionID = GetTransactionIDForReturnVoid();
            string sigImage = DBFunctions.GetSignatureString(transactionID);
            SignatureLookupDisplayWindow lookupDisplay = new SignatureLookupDisplayWindow(sigImage);
            lookupDisplay.ShowDialog();
        }
        #endregion


    }
}

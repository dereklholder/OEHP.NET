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
        }
        #region EdgeExpressRequestMethods
        private SaleResultXML SendSaleTransaction(string amount)
        {
            string parameters = PaymentEngine.BuildXMLSale(Globals.Default.XWebID, Globals.Default.XWebTerminalID, Globals.Default.XWebAuthKey, amount, Globals.Default.ClerkID);
            SaleResultXML result = FromXml<SaleResultXML>(PaymentEngine.SendToEdgeExpress(parameters));

            return result;
        }
        private PromptSignatureResultXML SendSignatureAfterSaleTransaction(string amount)
        {
            // Lots of Placeholder

            string parameters = PaymentEngine.BuildXMLSignaturePromptForPayment(amount);
            PromptSignatureResultXML result = FromXml<PromptSignatureResultXML>(PaymentEngine.SendToEdgeExpress(parameters));

            Globals.Default.TransactionCounter ++;
            Globals.Default.Save();

            return result;

        }
        private PromptSignatureResultXML SendSignatureForOtherTransaction(string agreement)
        {
            // Lots of Placeholder

            string parameters = PaymentEngine.BuildXMLSignaturePromptForOther(agreement);
            PromptSignatureResultXML result = FromXml<PromptSignatureResultXML>(PaymentEngine.SendToEdgeExpress(parameters));

            return result;
        }
        #endregion
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

            }
            return returnedXmlClass;
        }
        #endregion

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

        private void placeHolder_Click(object sender, RoutedEventArgs e)
        {
            SaleResultXML result = SendSaleTransaction("1.00");
            phText.Text = result.RESULTMSG;
            DBFunctions.InsertSaleTransaction(result);

            PromptSignatureResultXML signatureObject = SendSignatureAfterSaleTransaction("1.00");

            DBFunctions.InsertSignatureTransaction(signatureObject);

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
    }
}

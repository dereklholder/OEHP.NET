using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
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


namespace OEHP_Tester
{
    /// <summary>
    /// Interaction logic for MPDTransactions.xaml
    /// </summary>
    public partial class MPDTransactions : Window
    {
        public MPDTransactions()
        {
            InitializeComponent();

            DataTable dt = GeneralFunctions.GetPayerIDAndSpan();
            dataGrid.DataContext = dt.DefaultView;

            CreditCardChargeTypeCollection.Add("SALE");
            CreditCardChargeTypeCollection.Add("CREDIT");
            CreditCardChargeTypeCollection.Add("FORCE_SALE");
            CreditCardChargeTypeCollection.Add("DELETE_CUSTOMER");
            CreditCardChargeTypeCollection.Add("AUTH");

            ACHChargeTypeCollection.Add("DEBIT");
            ACHChargeTypeCollection.Add("CREDIT");
            ACHChargeTypeCollection.Add("DELETE_CUSTOMER");

            TransactionTypeCollection.Add("CREDIT_CARD");
            TransactionTypeCollection.Add("ACH");
            TransactionTypeBox.ItemsSource = TransactionTypeCollection;

            if (Globals.Default.QueryResponseMode == "Querystring")
            {
                QueryResponseQueryString.IsChecked = true;
                QueryResponseJSON.IsChecked = false;
            }
            if (Globals.Default.QueryResponseMode == "JSON")
            {
                QueryResponseQueryString.IsChecked = false;
                QueryResponseJSON.IsChecked = true;
            }

        }
        public List<TCCList> ACHorCCTCC()
        {
            List<TCCList> _TCCBoxItems = new List<TCCList>();
            if (TransactionTypeBox.SelectedItem.ToString() == "ACH")
            {
                _TCCBoxItems.Add(new TCCList { Header = "PPD", Value = "50" });
                _TCCBoxItems.Add(new TCCList { Header = "TEL", Value = "51" });
                _TCCBoxItems.Add(new TCCList { Header = "WEB", Value = "52" });
                _TCCBoxItems.Add(new TCCList { Header = "CCD", Value = "53" });
            }
            if (TransactionTypeBox.SelectedItem.ToString() == "CREDIT_CARD")
            {
                _TCCBoxItems.Add(new TCCList { Header = "Recurring", Value = "6" });
                _TCCBoxItems.Add(new TCCList { Header = "ECommerce", Value = "5" });
            }
            return _TCCBoxItems;
        }
        // Collections:
        public ObservableCollection<string> CreditCardChargeTypeCollection = new ObservableCollection<string>();
        public ObservableCollection<string> ACHChargeTypeCollection = new ObservableCollection<string>();
        public ObservableCollection<string> TransactionTypeCollection = new ObservableCollection<string>();
        public ObservableCollection<string> TCCCollection = new ObservableCollection<string>();

        private void PresetCanadianTesting_Click(object sender, RoutedEventArgs e)
        {
            AccountTokenBox.Text = Globals.Default.CanadianAccountToken;
        }

        private void PresetLoopBackTesting_Click(object sender, RoutedEventArgs e)
        {
            AccountTokenBox.Text = Globals.Default.LoopBackAccountToken;
        }

        private void PresetEMVTesting_Click(object sender, RoutedEventArgs e)
        {
            AccountTokenBox.Text = Globals.Default.DefaultAccountToken;
        }

        private void QueryResponseQueryString_Click(object sender, RoutedEventArgs e)
        {
            Globals.Default.QueryResponseMode = "Querystring";
            if (QueryResponseJSON.IsChecked == true)
            {
                QueryResponseJSON.IsChecked = false;
            }
            QueryResponseQueryString.IsChecked = true;
            Globals.Default.Save();
        }

        private void QueryResponseJSON_Click(object sender, RoutedEventArgs e)
        {
            Globals.Default.QueryResponseMode = "JSON";
            if (QueryResponseQueryString.IsChecked == true)
            {
                QueryResponseQueryString.IsChecked = false;
            }
            QueryResponseJSON.IsChecked = true;
            Globals.Default.Save();
        }

        private void SaveToken_Click(object sender, RoutedEventArgs e)
        {
            Globals.Default.AccountToken = AccountTokenBox.Text;
            Globals.Default.Save();
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            Globals.Default.AccountToken = Globals.Default.DefaultAccountToken;
            Globals.Default.Save();
            AccountTokenBox.Text = Globals.Default.AccountToken;
        }

        private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void File_Click(object sender, RoutedEventArgs e)
        {

        }

        private void PresetHelp_Click(object sender, RoutedEventArgs e)
        {
            PresetHelp ph = new PresetHelp();
            ph.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            ph.Topmost = true;
            ph.ShowDialog();
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            About window = new OEHP_Tester.About();
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.Topmost = true;
            window.ShowDialog();
        }

        private void Tools_Click(object sender, RoutedEventArgs e)
        {

        }

        private void PayerIDBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void TransactionTypeBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (TransactionTypeBox.SelectedItem.ToString())
            {
                case "CREDIT_CARD":
                    ChargeTypeBox.ItemsSource = CreditCardChargeTypeCollection;
                    TCCComboBox.ItemsSource = ACHorCCTCC();
                    break;
                case "ACH":
                    ChargeTypeBox.ItemsSource = ACHChargeTypeCollection;
                    TCCComboBox.ItemsSource = ACHorCCTCC();
                    break;
                default:
                    break;
            }
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                GeneralFunctions gf = new GeneralFunctions();
                TransactionRequest tr = new TransactionRequest();
                OEHP.NET.DataManipulation dm  = new OEHP.NET.DataManipulation();
                OEHP.NET.GatewayRequest gr = new OEHP.NET.GatewayRequest();
                string parameters;
                string result;
                switch (TransactionTypeBox.Text)
                {
                    case "CREDIT_CARD":
                        OrderIDBox.Text = tr.OrderIDRandom(8);
                        parameters = tr.MpdBuilder(AccountTokenBox.Text, OrderIDBox.Text, TransactionTypeBox.Text, ChargeTypeBox.Text, AmountBox.Text, PayerIDBox.Text, SpanBox.Text, TCCComboBox.SelectedValue.ToString(), CustomParametersBox.Text);
                        PostParametersBox.Text = parameters;
                        gf.WriteToLog(parameters);
                        result = gr.TestDirectPost(parameters);
                        gf.WriteToLog(result);
                        if (Globals.Default.QueryResponseMode == "Querystring")
                        {
                            HostPayBrowser.Text = result;
                        }
                        if (Globals.Default.QueryResponseMode == "JSON")
                        {
                            HostPayBrowser.Text = dm.QueryStringToJson(result);
                        }
                        break;
                    case "ACH":
                        OrderIDBox.Text = tr.OrderIDRandom(8);
                        parameters = tr.MPDCheckBuilder(AccountTokenBox.Text, OrderIDBox.Text, TransactionTypeBox.Text, ChargeTypeBox.Text, AmountBox.Text, PayerIDBox.Text, SpanBox.Text, TCCComboBox.SelectedValue.ToString(), CustomParametersBox.Text);
                        PostParametersBox.Text = parameters;
                        gf.WriteToLog(parameters);
                        HostPayBrowser.Text = gr.TestDirectPost(parameters);
                        result = gr.TestDirectPost(parameters);
                        gf.WriteToLog(result);
                        if (Globals.Default.QueryResponseMode == "Querystring")
                        {
                            HostPayBrowser.Text = result;
                        }
                        if (Globals.Default.QueryResponseMode == "JSON")
                        {
                            HostPayBrowser.Text = dm.QueryStringToJson(result);
                        }
                        break;
                    default:
                        HostPayBrowser.Text = "Check Transaction Request parameters";
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Invalid Transaction Parameters, please check your Parameters and try again.");
            }
        }
    }
    public class TCCList
    {
        public string Header { get; set; }
        public string Value { get; set; }
    }
}

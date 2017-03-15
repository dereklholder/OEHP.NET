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
        public void GetAliasList()
        {
            DataTable dt = GeneralFunctions.GetPayerIDAndSpan();
            dt.Columns["payer_identifier"].ColumnName = "Payer Identifier";
            dt.Columns["span"].ColumnName = "SPAN";
            dataGrid.DataContext = dt.DefaultView;
        }
        #region UI - MenuBar Functions 
        /// <summary>
        /// May Consolidate this later by taking all shared functions into GeneralFunctions and making method calls here. No functional improvement
        /// </summary>
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

        private void PresetHelp_Click(object sender, RoutedEventArgs e)
        {
            GeneralFunctions.PresetHelpWindowLauncher();
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            GeneralFunctions.AboutWindowLauncher();
        }
        private void Live_Click(object sender, RoutedEventArgs e)
        {
            LoginForLive login = new LoginForLive();
            if (login.ShowDialog().Value == false)
            {
                bool CorrectLogin = login.CorrectLogin;
                if (CorrectLogin == true)
                {
                    Globals.Default.ProcessingMode = "Live";
                    if (Test.IsChecked == true)
                    {
                        Test.IsChecked = false;
                    }
                    Live.IsChecked = true;
                }
                else
                {
                    MessageBox.Show("Live processing restricted to OpenEdge. Please contact your OpenEdge Representative.");
                }
            }
        }
        private void Test_Click(object sender, RoutedEventArgs e)
        {
            Globals.Default.ProcessingMode = "Test";
            if (Live.IsChecked == true)
            {
                Live.IsChecked = false;
            }
            Test.IsChecked = true;
        }
        private void CreateNewDB_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("This will delete the current database, do you want to continue?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                //do no stuff
            }
            else
            {
                GeneralFunctions gf = new GeneralFunctions();
                gf.CreateDBFile();

            }
        }
        private void GoToPortal_Click(object sender, RoutedEventArgs e)
        {
            GeneralFunctions.NavToDevPortal();
        }
        private void EmailDevServices_Click(object sender, RoutedEventArgs e)
        {
            GeneralFunctions.EmailDevServices();
        }

        private void DupCheckOn_Click(object sender, RoutedEventArgs e)
        {
            GeneralFunctions.SetDupModeOn();
        }
        private void DupCheckOff_Click(object sender, RoutedEventArgs e)
        {
            GeneralFunctions.SetDupModeOff();
        }
        #endregion
        #region UI - Visibility and General UI
        private void SetQueryResponseModeUIElements() //Sets MenuBar UI Elements for what QueryResponse mode is currently selected. Used only On application launch
        {
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
        private void SetProcessingModeUIElements() // Sets UI Elements for Processing Mode (Live vs Test)
        {
            if (Globals.Default.ProcessingMode == "Live")
            {
                Live.IsChecked = true;
            }
            if (Globals.Default.ProcessingMode == "Test")
            {
                Test.IsChecked = true;
            }
        }
        #endregion
        #region UI - Button Interaction
        private void SubmitButton_Click(object sender, RoutedEventArgs e) //Actions to submit transaction 
        {
            try
            {
                ResponseForWebBrowser response = new ResponseForWebBrowser();
                OEHP.NET.DataManipulation dm = new OEHP.NET.DataManipulation();
                switch (TransactionTypeBox.Text)
                {
                    case "CREDIT_CARD":
                        OrderIDBox.Text = TransactionRequest.OrderIDRandom(8);
                        PostParametersBox.Text = TransactionRequest.MpdBuilder(AccountTokenBox.Text, OrderIDBox.Text, TransactionTypeBox.Text, ChargeTypeBox.Text, AmountBox.Text, PayerIDBox.Text, SpanBox.Text, TCCComboBox.SelectedValue.ToString(), CustomParametersBox.Text);
                        GeneralFunctions.WriteToLog(PostParametersBox.Text);
                        response = PaymentEngine.SendToGateway(PostParametersBox.Text, "directPost");
                        if (Globals.Default.QueryResponseMode == "Querystring")
                        {
                            HostPayBrowser.Text = response.directPostResponse;
                        }
                        if (Globals.Default.QueryResponseMode == "JSON")
                        {
                            HostPayBrowser.Text = dm.QueryStringToJson(response.directPostResponse);
                        }
                        GetAliasList();
                        break;
                    case "ACH":
                        OrderIDBox.Text = TransactionRequest.OrderIDRandom(8);
                        PostParametersBox.Text = TransactionRequest.MPDCheckBuilder(AccountTokenBox.Text, OrderIDBox.Text, TransactionTypeBox.Text, ChargeTypeBox.Text, AmountBox.Text, PayerIDBox.Text, SpanBox.Text, TCCComboBox.SelectedValue.ToString(), CustomParametersBox.Text);
                        GeneralFunctions.WriteToLog(PostParametersBox.Text);
                        response = PaymentEngine.SendToGateway(PostParametersBox.Text, "directPost");
                        if (Globals.Default.QueryResponseMode == "Querystring")
                        {
                            HostPayBrowser.Text = response.directPostResponse;
                        }
                        if (Globals.Default.QueryResponseMode == "JSON")
                        {
                            HostPayBrowser.Text = dm.QueryStringToJson(response.directPostResponse);
                        }
                        GetAliasList();
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

        private void RefreshAliasList_Click(object sender, RoutedEventArgs e)
        {
            GetAliasList();
        }
        #endregion
        public MPDTransactions()
        {
            InitializeComponent();

            GetAliasList();
            TransactionTypeBox.ItemsSource = UICollections.MPDTransactionTypeValues();
            SetProcessingModeUIElements();
            SetQueryResponseModeUIElements();

        }     
        private void TransactionTypeBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TCCComboBox.ItemsSource = UICollections.ACHorCCTCC(TransactionTypeBox.SelectedItem.ToString());
            ChargeTypeBox.ItemsSource = UICollections.MPDChargeTypeValues(TransactionTypeBox.SelectedItem.ToString());
            //switch (TransactionTypeBox.SelectedItem.ToString())
            //{
            //    case "CREDIT_CARD":
            //        ChargeTypeBox.ItemsSource = CreditCardChargeTypeCollection;
            //        TCCComboBox.ItemsSource = ACHorCCTCC();
            //        break;
            //    case "ACH":
            //        ChargeTypeBox.ItemsSource = ACHChargeTypeCollection;
            //        TCCComboBox.ItemsSource = ACHorCCTCC();
            //        break;
            //    default:
            //        break;
            //}
        }
        
    }
}

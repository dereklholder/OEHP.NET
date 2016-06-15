using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
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

namespace OEHP_Tester
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {


            InitializeComponent();

            if (Properties.Settings.Default.IsFirstRun == "true")
            {
                GeneralFunctions gf = new GeneralFunctions();
                gf.CreateDBFile();
                Properties.Settings.Default.IsFirstRun = "false";
                Properties.Settings.Default.Save();
            }
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
            if (Globals.Default.ProcessingMode == "Live")
            {
                ProcessingModeLive.IsChecked = true;
            }
            if (Globals.Default.ProcessingMode == "Test")
            {
                ProcessingModeTest.IsChecked = true;
            }

            // AccountTokenBox.Text = Globals.Default.AccountToken; Replaced with Binding

            SubmitMethodBoxValues.Clear();
            SubmitMethodBoxValues.Add("PayPage Post");
            SubmitMethodBoxValues.Add("HTML Doc Post");
            SubmitMethodBox.ItemsSource = SubmitMethodBoxValues;

            ModeBoxValues.Clear();
            ModeBoxValues.Add("Test");
            ModeBoxValues.Add("Live");
            ModeComboBox.ItemsSource = ModeBoxValues;

            TransactionTypeValues.Clear();
            TransactionTypeValues.Add("CREDIT_CARD");
            TransactionTypeValues.Add("DEBIT_CARD");
            TransactionTypeValues.Add("ACH");
            TransactionTypeValues.Add("INTERAC");
            TransactionTypeBox.ItemsSource = TransactionTypeValues;

            TCCValues.Clear();
            TCCValues.Add("50");
            TCCValues.Add("51");
            TCCValues.Add("52");
            TCCValues.Add("53");
            TCCBox.ItemsSource = TCCValues;

            CreditChargeTypeValues.Clear();
            CreditChargeTypeValues.Add("SALE");
            CreditChargeTypeValues.Add("CREDIT");
            CreditChargeTypeValues.Add("VOID");
            CreditChargeTypeValues.Add("FORCE_SALE");
            CreditChargeTypeValues.Add("AUTH");
            CreditChargeTypeValues.Add("CAPTURE");
            CreditChargeTypeValues.Add("ADJUSTMENT");
            CreditChargeTypeValues.Add("SIGNATURE");

            DebitChargeTypeValues.Clear();
            DebitChargeTypeValues.Add("PURCHASE");
            DebitChargeTypeValues.Add("REFUND");

            ACHChargeTypeValues.Clear();
            ACHChargeTypeValues.Add("DEBIT");
            ACHChargeTypeValues.Add("CREDIT");

            AccountTypeValues.Clear();
            AccountTypeValues.Add("DEFAULT");
            AccountTypeValues.Add("CASH_BENEFIT");
            AccountTypeValues.Add("FOOD_STAMP");
            AccountTypeBox.ItemsSource = AccountTypeValues;

            CreditTypeValues.Clear();
            CreditTypeValues.Add("INDEPENDENT");
            CreditTypeValues.Add("DEPENDENT");
            CreditTypeBox.ItemsSource = CreditTypeValues;

            CreditEntryModeValues.Clear();
            CreditEntryModeValues.Add("KEYED");
            CreditEntryModeValues.Add("EMV");
            CreditEntryModeValues.Add("HID");
            CreditEntryModeValues.Add("AUTO");

            DebitEntryModeValues.Clear();
            DebitEntryModeValues.Add("EMV");
            DebitEntryModeValues.Add("HID");

            ACHEntryModeValues.Clear();
            ACHEntryModeValues.Add("KEYED");



        }
        //Collections for Combo Boxes
        public ObservableCollection<string> SubmitMethodBoxValues = new ObservableCollection<string>();
        public ObservableCollection<string> SubmitMethodBoxDirectPost = new ObservableCollection<string>();

        public ObservableCollection<string> ModeBoxValues = new ObservableCollection<string>();
        public ObservableCollection<string> TransactionTypeValues = new ObservableCollection<string>();

        public ObservableCollection<string> CreditEntryModeValues = new ObservableCollection<string>();
        public ObservableCollection<string> DebitEntryModeValues = new ObservableCollection<string>();
        public ObservableCollection<string> ACHEntryModeValues = new ObservableCollection<string>();

        public ObservableCollection<string> CreditChargeTypeValues = new ObservableCollection<string>();
        public ObservableCollection<string> DebitChargeTypeValues = new ObservableCollection<string>();
        public ObservableCollection<string> ACHChargeTypeValues = new ObservableCollection<string>();

        public ObservableCollection<string> CreditTypeValues = new ObservableCollection<string>();
        public ObservableCollection<string> AccountTypeValues = new ObservableCollection<string>();
        public ObservableCollection<string> TCCValues = new ObservableCollection<string>();


        private void submitButton_Click(object sender, RoutedEventArgs e)
        {

            //Create a new transaction Request Object
            TransactionRequest tr = new TransactionRequest();
            string transactionRequestString;

            //Create a new Gateway Request Object
            OEHP.NET.GatewayRequest gr = new OEHP.NET.GatewayRequest();
            //Create GeneralFunctions object
            GeneralFunctions gf = new GeneralFunctions();
            

            try
            {
                switch (ChargeTypeBox.Text)
                {
                    case "SALE":
                        OrderIDBox.Text = tr.OrderIDRandom(8);
                        transactionRequestString = tr.CreditCardParamBuilder(AccountTokenBox.Text, TransactionTypeBox.SelectedItem.ToString(), ChargeTypeBox.SelectedItem.ToString(), EntryModeBox.SelectedItem.ToString(),
                                                    OrderIDBox.Text, AmountBox.Text, CustomParametersBox.Text);
                        gf.WriteToLog(transactionRequestString);
                        PostParameterBox.Text = transactionRequestString;

                        switch (SubmitMethodBox.SelectedItem.ToString())
                        {
                            case "PayPage Post":
                                if (Globals.Default.ProcessingMode == "Test")
                                {
                                    OEHPWebBrowser.Navigate(gr.TestPayPagePost(transactionRequestString));

                                }
                                if (Globals.Default.ProcessingMode == "Live")
                                {
                                    OEHPWebBrowser.Navigate(gr.LivePayPagePost(transactionRequestString));
                                }
                                break;
                            case "HTML Doc Post":
                                if (Globals.Default.ProcessingMode == "Test")
                                {
                                    OEHPWebBrowser.NavigateToString(gr.TestHtmlDocPost(transactionRequestString));
                                }
                                else
                                {
                                    OEHPWebBrowser.NavigateToString(gr.LiveHtmlDocumentPost(transactionRequestString));
                                }
                                break;
                            case "Direct Post":
                            default:
                                OEHPWebBrowser.NavigateToString("No Submit Method Selected. Transaction Failed");
                                break;

                        }
                        break;

                    case "AUTH":
                        OrderIDBox.Text = tr.OrderIDRandom(8);
                        transactionRequestString = tr.CreditCardParamBuilder(AccountTokenBox.Text, TransactionTypeBox.SelectedItem.ToString(), ChargeTypeBox.SelectedItem.ToString(), EntryModeBox.SelectedItem.ToString(),
                                                    OrderIDBox.Text, AmountBox.Text, CustomParametersBox.Text);
                        
                        gf.WriteToLog(transactionRequestString);
                        PostParameterBox.Text = transactionRequestString;

                        switch (SubmitMethodBox.SelectedItem.ToString())
                        {
                            case "PayPage Post":
                                if (Globals.Default.ProcessingMode == "Test")
                                {
                                    OEHPWebBrowser.Navigate(gr.TestPayPagePost(transactionRequestString));

                                }
                                if (Globals.Default.ProcessingMode == "Live")
                                {
                                    ;
                                    OEHPWebBrowser.Navigate(gr.LivePayPagePost(transactionRequestString));
                                }
                                break;
                            case "HTML Doc Post":
                                if (Globals.Default.ProcessingMode == "Test")
                                {
                                    OEHPWebBrowser.NavigateToString(gr.TestHtmlDocPost(transactionRequestString));
                                }
                                else
                                {
                                    OEHPWebBrowser.NavigateToString(gr.LiveHtmlDocumentPost(transactionRequestString));
                                }
                                break;
                            case "Direct Post":
                            default:
                                OEHPWebBrowser.NavigateToString("No Submit Method Selected. Transaction Failed");
                                break;

                        }
                        break;

                        //need to Implement Signature IMage handling
                    case "SIGNATURE":
                        OrderIDBox.Text = tr.OrderIDRandom(8);
                        transactionRequestString = tr.CreditCardParamBuilder(AccountTokenBox.Text, TransactionTypeBox.SelectedItem.ToString(), ChargeTypeBox.SelectedItem.ToString(), EntryModeBox.SelectedItem.ToString(),
                                                    OrderIDBox.Text, AmountBox.Text, CustomParametersBox.Text);
                        gf.WriteToLog(transactionRequestString);
                        PostParameterBox.Text = transactionRequestString;
                        switch (SubmitMethodBox.SelectedItem.ToString())
                        {
                            case "PayPage Post":
                                if (Globals.Default.ProcessingMode == "Test")
                                {
                                    OEHPWebBrowser.Navigate(gr.TestPayPagePost(transactionRequestString));

                                }
                                if (Globals.Default.ProcessingMode == "Live")
                                {
                                    OEHPWebBrowser.Navigate(gr.LivePayPagePost(transactionRequestString));
                                }
                                break;
                            case "HTML Doc Post":
                                if (Globals.Default.ProcessingMode == "Test")
                                {
                                    OEHPWebBrowser.NavigateToString(gr.TestHtmlDocPost(transactionRequestString));
                                }
                                else
                                {
                                    OEHPWebBrowser.NavigateToString(gr.LiveHtmlDocumentPost(transactionRequestString));
                                }
                                break;
                            case "Direct Post":
                            default:
                                OEHPWebBrowser.NavigateToString("No Submit Method Selected. Transaction Failed");
                                break;

                        }
                        break;

                    case "PURCHASE":
                        OrderIDBox.Text = tr.OrderIDRandom(8);
                        transactionRequestString = tr.DebitCardParamBuilder(AccountTokenBox.Text, TransactionTypeBox.SelectedItem.ToString(), ChargeTypeBox.SelectedItem.ToString(), EntryModeBox.SelectedItem.ToString(),
                                                        OrderIDBox.Text, AmountBox.Text, AccountTypeBox.SelectedItem.ToString(), CustomParametersBox.Text);
                        gf.WriteToLog(transactionRequestString);
                        PostParameterBox.Text = transactionRequestString;
                        switch (SubmitMethodBox.SelectedItem.ToString())
                        {
                            case "PayPage Post":
                                if (Globals.Default.ProcessingMode == "Test")
                                {
                                    OEHPWebBrowser.Navigate(gr.TestPayPagePost(transactionRequestString));

                                }
                                if (Globals.Default.ProcessingMode == "Live")
                                {
                                    OEHPWebBrowser.Navigate(gr.LivePayPagePost(transactionRequestString));
                                }
                                break;
                            case "HTML Doc Post":
                                if (Globals.Default.ProcessingMode == "Test")
                                {
                                    OEHPWebBrowser.NavigateToString(gr.TestHtmlDocPost(transactionRequestString));
                                }
                                else
                                {
                                    OEHPWebBrowser.NavigateToString(gr.LiveHtmlDocumentPost(transactionRequestString));
                                }
                                break;
                            case "Direct Post":
                            default:
                                OEHPWebBrowser.NavigateToString("No Submit Method Selected. Transaction Failed");
                                break;

                        }
                        break;

                    case "REFUND":
                        OrderIDBox.Text = tr.OrderIDRandom(8);
                        transactionRequestString = tr.DebitCardParamBuilder(AccountTokenBox.Text, TransactionTypeBox.SelectedItem.ToString(), ChargeTypeBox.SelectedItem.ToString(), EntryModeBox.SelectedItem.ToString(),
                                                        OrderIDBox.Text, AmountBox.Text, AccountTypeBox.SelectedItem.ToString(), CustomParametersBox.Text);
                        gf.WriteToLog(transactionRequestString);
                        PostParameterBox.Text = transactionRequestString;
                        switch (SubmitMethodBox.SelectedItem.ToString())
                        {
                            case "PayPage Post":
                                if (Globals.Default.ProcessingMode == "Test")
                                {
                                    OEHPWebBrowser.Navigate(gr.TestPayPagePost(transactionRequestString));

                                }
                                if (Globals.Default.ProcessingMode == "Live")
                                {
                                    OEHPWebBrowser.Navigate(gr.LivePayPagePost(transactionRequestString));
                                }
                                break;
                            case "HTML Doc Post":
                                if (Globals.Default.ProcessingMode == "Test")
                                {
                                    OEHPWebBrowser.NavigateToString(gr.TestHtmlDocPost(transactionRequestString));
                                }
                                else
                                {
                                    OEHPWebBrowser.NavigateToString(gr.LiveHtmlDocumentPost(transactionRequestString));
                                }
                                break;
                            case "Direct Post":
                            default:
                                OEHPWebBrowser.NavigateToString("No Submit Method Selected. Transaction Failed");
                                break;

                        }
                        break;

                    case "DEBIT":
                        OrderIDBox.Text = tr.OrderIDRandom(8);
                        transactionRequestString = tr.ACHParamBuilder(AccountTokenBox.Text, TransactionTypeBox.SelectedItem.ToString(), ChargeTypeBox.SelectedItem.ToString(), EntryModeBox.SelectedItem.ToString(),
                                                                        OrderIDBox.Text, AmountBox.Text, TCCBox.SelectedItem.ToString(), CustomParametersBox.Text);
                        gf.WriteToLog(transactionRequestString);
                        PostParameterBox.Text = transactionRequestString;
                        switch (SubmitMethodBox.SelectedItem.ToString())
                        {
                            case "PayPage Post":
                                if (Globals.Default.ProcessingMode == "Test")
                                {
                                    OEHPWebBrowser.Navigate(gr.TestPayPagePost(transactionRequestString));

                                }
                                if (Globals.Default.ProcessingMode == "Live")
                                {
                                    OEHPWebBrowser.Navigate(gr.LivePayPagePost(transactionRequestString));
                                }
                                break;
                            case "HTML Doc Post":
                                if (Globals.Default.ProcessingMode == "Test")
                                {
                                    OEHPWebBrowser.NavigateToString(gr.TestHtmlDocPost(transactionRequestString));
                                }
                                else
                                {
                                    OEHPWebBrowser.NavigateToString(gr.LiveHtmlDocumentPost(transactionRequestString));
                                }
                                break;
                            case "Direct Post":
                            default:
                                OEHPWebBrowser.NavigateToString("No Submit Method Selected. Transaction Failed");
                                break;

                        }
                        break;

                    case "VOID":
                        transactionRequestString = tr.DirectPostBuilder(AccountTokenBox.Text, OrderIDBox.Text, TransactionTypeBox.SelectionBoxItem.ToString(), ChargeTypeBox.SelectedItem.ToString());
                        gf.WriteToLog(transactionRequestString);
                        PostParameterBox.Text = transactionRequestString;
                        
                        if (Globals.Default.ProcessingMode == "Test")
                        {
                            OEHPWebBrowser.NavigateToString(gr.TestDirectPost(transactionRequestString));
                        }
                        else
                        {
                            OEHPWebBrowser.NavigateToString(gr.LiveDirectPost(transactionRequestString));
                        }
                        break;

                    case "FORCE_SALE":
                        OrderIDBox.Text = tr.OrderIDRandom(8);
                        transactionRequestString = tr.CreditForceParamBuilder(AccountTokenBox.Text, TransactionTypeBox.SelectedItem.ToString(), ChargeTypeBox.SelectedItem.ToString(),
                                                                                EntryModeBox.SelectedItem.ToString(), OrderIDBox.Text, AmountBox.Text, ApprovalCodeBox.Text, CustomParametersBox.Text);
                        gf.WriteToLog(transactionRequestString);
                        PostParameterBox.Text = transactionRequestString;
                        switch (SubmitMethodBox.SelectedItem.ToString())
                        {
                            case "PayPage Post":
                                if (Globals.Default.ProcessingMode == "Test")
                                {
                                    OEHPWebBrowser.Navigate(gr.TestPayPagePost(transactionRequestString));

                                }
                                if (Globals.Default.ProcessingMode == "Live")
                                {
                                    OEHPWebBrowser.Navigate(gr.LivePayPagePost(transactionRequestString));
                                }
                                break;
                            case "HTML Doc Post":
                                if (Globals.Default.ProcessingMode == "Test")
                                {
                                    OEHPWebBrowser.NavigateToString(gr.TestHtmlDocPost(transactionRequestString)); 
                                }
                                else
                                {
                                    OEHPWebBrowser.NavigateToString(gr.LiveHtmlDocumentPost(transactionRequestString));
                                }
                                break;
                            case "Direct Post":
                            default:
                                OEHPWebBrowser.NavigateToString("No Submit Method Selected. Transaction Failed");
                                break;

                        }
                        break;

                    case "CAPTURE":
                        transactionRequestString = tr.CreditCardParamBuilder(AccountTokenBox.Text, TransactionTypeBox.SelectedItem.ToString(), ChargeTypeBox.SelectedItem.ToString(), EntryModeBox.SelectedItem.ToString(), OrderIDBox.Text, AmountBox.Text, CustomParametersBox.Text);
                        gf.WriteToLog(transactionRequestString);
                        PostParameterBox.Text = transactionRequestString;
                        if (Globals.Default.ProcessingMode == "Test")
                        {
                            OEHPWebBrowser.NavigateToString(gr.TestDirectPost(transactionRequestString));
                        }
                        else
                        {
                            OEHPWebBrowser.NavigateToString(gr.LiveDirectPost(transactionRequestString));
                        }
                        break;

                    case "ADJUSTMENT":
                        transactionRequestString = tr.CreditCardParamBuilder(AccountTokenBox.Text, TransactionTypeBox.SelectedItem.ToString(), ChargeTypeBox.SelectedItem.ToString(), EntryModeBox.SelectedItem.ToString(), OrderIDBox.Text, AmountBox.Text, CustomParametersBox.Text);
                        gf.WriteToLog(transactionRequestString);
                        PostParameterBox.Text = transactionRequestString;
                        if (Globals.Default.ProcessingMode == "Test")
                        {
                            OEHPWebBrowser.NavigateToString(gr.TestDirectPost(transactionRequestString));
                        }
                        else
                        {
                            OEHPWebBrowser.NavigateToString(gr.LiveDirectPost(transactionRequestString));
                        }
                        break;


                        //May need to Redo ACH Dependent Return
                    case "CREDIT":
                        if (TransactionTypeBox.SelectedItem.ToString() == "CREDIT_CARD")
                        {
                            if (CreditTypeBox.SelectedItem.ToString() == "INDEPENDENT")
                            {
                                OrderIDBox.Text = tr.OrderIDRandom(8);
                                transactionRequestString = tr.CreditCardParamBuilder(AccountTokenBox.Text, TransactionTypeBox.SelectedItem.ToString(), ChargeTypeBox.SelectedItem.ToString(),
                                                                                        EntryModeBox.SelectedItem.ToString(), OrderIDBox.Text, AmountBox.Text, CustomParametersBox.Text);
                                PostParameterBox.Text = (transactionRequestString);
                                switch (SubmitMethodBox.SelectedItem.ToString())
                                {
                                    case "PayPage Post":
                                        if (Globals.Default.ProcessingMode == "Test")
                                        {
                                            OEHPWebBrowser.Navigate(gr.TestPayPagePost(transactionRequestString));

                                        }
                                        if (Globals.Default.ProcessingMode == "Live")
                                        {
                                            OEHPWebBrowser.Navigate(gr.LivePayPagePost(transactionRequestString));
                                        }
                                        break;
                                    case "HTML Doc Post":
                                        if (Globals.Default.ProcessingMode == "Test")
                                        {
                                            OEHPWebBrowser.NavigateToString(gr.TestHtmlDocPost(transactionRequestString));
                                        }
                                        else
                                        {
                                            OEHPWebBrowser.NavigateToString(gr.LiveHtmlDocumentPost(transactionRequestString));
                                        }
                                        break;
                                    case "Direct Post":
                                    default:
                                        OEHPWebBrowser.NavigateToString("No Submit Method Selected. Transaction Failed");
                                        break;

                                }
                            }
                            if (CreditTypeBox.SelectedItem.ToString() == "DEPENDENT")
                            {
                                transactionRequestString = tr.CreditCardParamBuilder(AccountTokenBox.Text, TransactionTypeBox.SelectedItem.ToString(), ChargeTypeBox.SelectedItem.ToString(),
                                                                                        EntryModeBox.SelectedItem.ToString(), OrderIDBox.Text, AmountBox.Text, CustomParametersBox.Text);
                                gf.WriteToLog(transactionRequestString);
                                PostParameterBox.Text = transactionRequestString;
                                if (Globals.Default.ProcessingMode == "Test")
                                {
                                    OEHPWebBrowser.NavigateToString(gr.TestDirectPost(transactionRequestString));
                                }
                                else
                                {
                                    OEHPWebBrowser.NavigateToString(gr.LiveDirectPost(transactionRequestString));
                                }
                            }
                        }
                        if (TransactionTypeBox.SelectedItem.ToString() == "ACH")
                        {
                            if (CreditTypeBox.SelectedItem.ToString() == "INDEPENDENT")
                            {
                                OrderIDBox.Text = tr.OrderIDRandom(8);
                                transactionRequestString = tr.ACHParamBuilder(AccountTokenBox.Text, TransactionTypeBox.SelectedItem.ToString(), ChargeTypeBox.SelectedItem.ToString(), EntryModeBox.SelectedItem.ToString(),
                                                                                    OrderIDBox.Text, AmountBox.Text, TCCBox.SelectedItem.ToString(), CustomParametersBox.Text);
                                PostParameterBox.Text = (transactionRequestString);
                                switch (SubmitMethodBox.SelectedItem.ToString())
                                {
                                    case "PayPage Post":
                                        if (Globals.Default.ProcessingMode == "Test")
                                        {
                                            OEHPWebBrowser.Navigate(gr.TestPayPagePost(transactionRequestString));

                                        }
                                        if (Globals.Default.ProcessingMode == "Live")
                                        {
                                            OEHPWebBrowser.Navigate(gr.LivePayPagePost(transactionRequestString));
                                        }
                                        break;
                                    case "HTML Doc Post":
                                        if (Globals.Default.ProcessingMode == "Test")
                                        {
                                            OEHPWebBrowser.NavigateToString(gr.TestHtmlDocPost(transactionRequestString));
                                        }
                                        else
                                        {
                                            OEHPWebBrowser.NavigateToString(gr.LiveHtmlDocumentPost(transactionRequestString));
                                        }
                                        break;
                                    case "Direct Post":
                                    default:
                                        OEHPWebBrowser.NavigateToString("No Submit Method Selected. Transaction Failed");
                                        break;

                                }
                            }
                            if (CreditTypeBox.SelectedItem.ToString() == "DEPENDENT")
                            {
                                transactionRequestString = tr.ACHParamBuilder(AccountTokenBox.Text, TransactionTypeBox.SelectedItem.ToString(), ChargeTypeBox.SelectedItem.ToString(), EntryModeBox.SelectedItem.ToString(),
                                                                                    OrderIDBox.Text, AmountBox.Text, TCCBox.SelectedItem.ToString(), CustomParametersBox.Text);
                                gf.WriteToLog(transactionRequestString);
                                PostParameterBox.Text = transactionRequestString;
                                if (Globals.Default.ProcessingMode == "Test")
                                {
                                    OEHPWebBrowser.NavigateToString(gr.TestDirectPost(transactionRequestString));
                                }
                                else
                                {
                                    OEHPWebBrowser.NavigateToString(gr.LiveDirectPost(transactionRequestString));
                                }
                            }
                        }
                        break;
                }
            }
            catch (Exception ex)
            {

                OEHPWebBrowser.NavigateToString("An Exception Occured. Please Check Parameters");
                gf.WriteToLog(ex.ToString());
            }



        }

        private void ChargeTypeBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                switch (ChargeTypeBox.SelectedItem.ToString())
                {
                    case "CREDIT":
                        OrderIDBox.IsReadOnly = true;
                        OrderIDBox.Text = "";

                        CreditTypeBox.SelectedIndex = -1;
                        CreditTypeBox.Visibility = Visibility.Visible;
                        CreditTypeLabel.Visibility = Visibility.Visible;

                        ApprovalCodeBox.Visibility = Visibility.Hidden;
                        ApprovalCodeLabel.Visibility = Visibility.Hidden;

                        break;

                    case "FORCE_SALE":
                        OrderIDBox.IsReadOnly = true;
                        OrderIDBox.Text = "";

                        CreditTypeBox.Visibility = Visibility.Hidden;
                        CreditTypeLabel.Visibility = Visibility.Hidden;

                        ApprovalCodeBox.Visibility = Visibility.Visible;
                        ApprovalCodeLabel.Visibility = Visibility.Visible;

                        break;

                    case "ADJUSTMENT":
                        OrderIDBox.IsReadOnly = false;

                        CreditTypeBox.Visibility = Visibility.Hidden;
                        CreditTypeLabel.Visibility = Visibility.Hidden;

                        ApprovalCodeBox.Visibility = Visibility.Hidden;
                        ApprovalCodeLabel.Visibility = Visibility.Hidden;

                        break;

                    case "CAPTURE":
                        OrderIDBox.IsReadOnly = false;

                        CreditTypeBox.Visibility = Visibility.Hidden;
                        CreditTypeLabel.Visibility = Visibility.Hidden;

                        ApprovalCodeBox.Visibility = Visibility.Hidden;
                        ApprovalCodeLabel.Visibility = Visibility.Hidden;

                        break;

                    case "VOID":
                        OrderIDBox.IsReadOnly = false;

                        CreditTypeBox.Visibility = Visibility.Hidden;
                        CreditTypeLabel.Visibility = Visibility.Hidden;

                        ApprovalCodeBox.Visibility = Visibility.Hidden;
                        ApprovalCodeLabel.Visibility = Visibility.Hidden;

                        break;

                    default:
                        OrderIDBox.IsReadOnly = true;
                        OrderIDBox.Text = "";

                        CreditTypeBox.Visibility = Visibility.Hidden;
                        CreditTypeLabel.Visibility = Visibility.Hidden;

                        ApprovalCodeBox.Visibility = Visibility.Hidden;
                        ApprovalCodeLabel.Visibility = Visibility.Hidden;

                        break;

                }
            }
            catch (Exception ex)
            {
                GeneralFunctions gf = new GeneralFunctions();
                gf.WriteToLog(ex.ToString());
            }
        }

        private void TransactionTypeBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                switch (TransactionTypeBox.SelectedItem.ToString())
                {
                    case "CREDIT_CARD":

                        AccountTypeBox.Visibility = Visibility.Hidden;
                        AccountTypeLabel.Visibility = Visibility.Hidden;
                        CreditTypeLabel.Visibility = Visibility.Hidden;
                        CreditTypeBox.Visibility = Visibility.Hidden;
                        TCCBox.Visibility = Visibility.Hidden;
                        TCCLabel.Visibility = Visibility.Hidden;
                        ApprovalCodeBox.Visibility = Visibility.Hidden;
                        ApprovalCodeLabel.Visibility = Visibility.Hidden;

                        EntryModeBox.ItemsSource = CreditEntryModeValues;
                        EntryModeBox.SelectedIndex = 0;

                        ChargeTypeBox.ItemsSource = CreditChargeTypeValues;
                        ChargeTypeBox.SelectedIndex = 0;

                        CreditTypeBox.SelectedIndex = -1;
                        TCCBox.SelectedIndex = -1;
                        break;

                    case "DEBIT_CARD":

                        AccountTypeBox.Visibility = Visibility.Visible;
                        AccountTypeLabel.Visibility = Visibility.Visible;
                        CreditTypeLabel.Visibility = Visibility.Hidden;
                        CreditTypeBox.Visibility = Visibility.Hidden;
                        TCCBox.Visibility = Visibility.Hidden;
                        TCCLabel.Visibility = Visibility.Hidden;
                        ApprovalCodeBox.Visibility = Visibility.Hidden;
                        ApprovalCodeLabel.Visibility = Visibility.Hidden;

                        EntryModeBox.ItemsSource = DebitEntryModeValues;
                        EntryModeBox.SelectedIndex = 0;

                        AccountTypeBox.SelectedIndex = 0;

                        ChargeTypeBox.ItemsSource = DebitChargeTypeValues;
                        ChargeTypeBox.SelectedIndex = 0;

                        CreditTypeBox.SelectedIndex = -1;
                        TCCBox.SelectedIndex = -1;

                        break;

                    case "ACH":

                        AccountTypeBox.Visibility = Visibility.Hidden;
                        AccountTypeLabel.Visibility = Visibility.Hidden;
                        CreditTypeLabel.Visibility = Visibility.Hidden;
                        CreditTypeBox.Visibility = Visibility.Hidden;
                        TCCBox.Visibility = Visibility.Visible;
                        TCCLabel.Visibility = Visibility.Visible;
                        ApprovalCodeBox.Visibility = Visibility.Hidden;
                        ApprovalCodeLabel.Visibility = Visibility.Hidden;

                        EntryModeBox.ItemsSource = ACHEntryModeValues;
                        EntryModeBox.SelectedIndex = 0;

                        ChargeTypeBox.ItemsSource = ACHChargeTypeValues;
                        ChargeTypeBox.SelectedIndex = 0;

                        CreditTypeBox.SelectedIndex = -1;
                        TCCBox.SelectedIndex = 0;

                        break;

                    case "INTERAC":

                        AccountTypeBox.Visibility = Visibility.Hidden;
                        AccountTypeLabel.Visibility = Visibility.Hidden;
                        CreditTypeLabel.Visibility = Visibility.Hidden;
                        CreditTypeBox.Visibility = Visibility.Hidden;
                        TCCBox.Visibility = Visibility.Hidden;
                        TCCLabel.Visibility = Visibility.Hidden;
                        ApprovalCodeBox.Visibility = Visibility.Hidden;
                        ApprovalCodeLabel.Visibility = Visibility.Hidden;

                        AccountTypeBox.SelectedIndex = 0;

                        EntryModeBox.ItemsSource = DebitEntryModeValues;
                        EntryModeBox.SelectedIndex = 0;

                        ChargeTypeBox.ItemsSource = DebitChargeTypeValues;
                        ChargeTypeBox.SelectedIndex = 0;

                        CreditTypeBox.SelectedIndex = -1;
                        TCCBox.SelectedIndex = -1;

                        break;
                }
            }
            catch (Exception ex)
            {
                GeneralFunctions gf = new GeneralFunctions();
                gf.WriteToLog(ex.ToString());
            }
        }

        private void CustomParametersBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Globals.Default.CustomParameters = CustomParametersBox.Text;
        }

        public void RCMStatus()
        {
            try
            {
                if (SubmitMethodBox.SelectedItem.ToString() == "PayPage Post")
                {
                    string ssp = OEHP.NET.SSP.SessionToken;

                    if (Globals.Default.ProcessingMode == "Test")
                    {
                        WebRequest wr = WebRequest.Create(OEHP.NET.VariableHandler.TestRcmStatusURL + OEHP.NET.VariableHandler.SessionToken);
                        wr.Method = "GET";

                        Stream objStream;
                        objStream = wr.GetResponse().GetResponseStream();

                        StreamReader sr = new StreamReader(objStream);

                        string rcmStatus = sr.ReadToEnd();
                        RCMStatusBox.Text = rcmStatus;
                    }
                    else
                    {
                        WebRequest wr = WebRequest.Create(OEHP.NET.VariableHandler.LiveRcmStatusURL + OEHP.NET.VariableHandler.SessionToken);
                        wr.Method = "GET";

                        Stream objStream;
                        objStream = wr.GetResponse().GetResponseStream();

                        StreamReader sr = new StreamReader(objStream);

                        string rcmStatus = sr.ReadToEnd();
                        RCMStatusBox.Text = rcmStatus;
                    }
                }
                else
                {
                    GeneralFunctions gf = new GeneralFunctions();
                    string pageHTML = gf.GetPageContent(OEHPWebBrowser);
                    string rcmText = gf.RCMStatusFromWebPage(pageHTML);
                    RCMStatusBox.Text = rcmText;
                }


            }
            catch (Exception ex)
            {
                GeneralFunctions gf = new GeneralFunctions();
                gf.WriteToLog(ex.ToString());
            }
        }

        private void OEHPWebBrowser_LoadCompleted(object sender, NavigationEventArgs e)
        {
            string queryParameters;
            RCMStatus();
            GeneralFunctions gf = new GeneralFunctions();
            TransactionRequest tr = new TransactionRequest();
            OEHP.NET.DataManipulation dm = new OEHP.NET.DataManipulation();
            OEHP.NET.GatewayRequest gr = new OEHP.NET.GatewayRequest();
            string paymentFinishedSignal = gf.PaymentFinishedSignal(gf.GetPageContent(OEHPWebBrowser));

            if (paymentFinishedSignal == "done")
            {
                switch (TransactionTypeBox.SelectedItem.ToString())
                {
                    case "CREDIT_CARD":
                        queryParameters = tr.DirectPostBuilder(AccountTokenBox.Text, OrderIDBox.Text, TransactionTypeBox.SelectedItem.ToString(), "QUERY_PAYMENT");
                        QueryParametersBox.Text = queryParameters;
                        gf.WriteToLog(queryParameters);
                        if (Globals.Default.ProcessingMode == "Test")
                        {
                            QueryPaymentBox.Text = gr.TestDirectPost(queryParameters);
                            gf.WriteToLog(QueryPaymentBox.Text);

                            //Converts QueryString to JSON in Query Response if mode is set.
                            if (Globals.Default.QueryResponseMode == "JSON")
                            {
                                string result = dm.QueryStringToJson(QueryPaymentBox.Text);
                                QueryPaymentBox.Text = result;
                            }

                            //Writes transaction data to DB
                            char firstChar = QueryPaymentBox.Text[0];
                            if (firstChar.ToString() == "r")
                            {
                                
                                string result = dm.QueryStringToJson(QueryPaymentBox.Text);
                                var qro = dm.QueryResultObject(result);
                                object[] args = new object[] { qro.response_code, qro.response_code_text, qro.secondary_response_code, qro.secondary_response_code_text, qro.time_stamp, qro.retry_recommended, qro.authorized_amount, qro.bin, qro.captured_amount, qro.original_authorized_amount, qro.requested_amount, qro.time_stamp_created, qro.original_response_code, qro.original_response_code_text, qro.time_stamp_updated, qro.state, qro.bank_approval_code, qro.expire_month, qro.expire_year, qro.order_id, qro.payer_identifier, qro.reference_id, qro.span, qro.card_brand, qro.batch_id, qro.receipt_application_cryptogram, qro.receipt_application_identifier, qro.receipt_application_preferred_name, qro.receipt_application_transaction_counter, qro.receipt_approval_code, qro.receipt_authorization_response_code, qro.receipt_card_number, qro.receipt_card_type, qro.receipt_entry_legend, qro.receipt_entry_method, qro.receipt_line_items, qro.receipt_merchant_id, qro.receipt_order_id, qro.receipt_signature_text, qro.receipt_terminal_verification_results, qro.receipt_transaction_date_time, qro.receipt_transaction_id, qro.receipt_transaction_reference_number, qro.receipt_transaction_type, qro.receipt_validation_code, qro.receipt_verbiage};
                                string sqlInsert = string.Format("INSERT INTO TRANSACTIONDB (response_code, response_code_text, secondary_response_code, secondary_response_code_text, time_stamp, retry_recommended, authorized_amount, bin, captured_amount, original_authorized_amount, requested_amount, time_stamp_created, original_response_code, original_response_code_text, time_stamp_updated, state, bank_approval_code, expire_month, expire_year, order_id, payer_identifier, reference_id, span, card_brand, batch_id, receipt_application_cryptogram, receipt_application_identifier, receipt_application_preferred_name, receipt_application_transaction_counter, receipt_approval_code, receipt_authorization_reseponse_code, receipt_card_number, receipt_card_type, receipt_entry_legend, receipt_entry_method, receipt_line_items, receipt_merchant_id, receipt_order_id, receipt_signature_text, receipt_terminal_verificiation_results, receipt_transaction_date_time, receipt_transaction_id, receipt_transaction_reference_number, receipt_transaction_type, receipt_validation_code, receipt_verbiage) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}', '{13}', '{14}', '{15}', '{16}', '{17}', '{18}', '{19}', '{20}', '{21}', '{22}', '{23}', '{24}', '{25}', '{26}', '{27}', '{28}', '{29}', '{30}', '{31}', '{32}', '{33}', '{34}', '{35}', '{36}', '{37}', '{38}', '{39}', '{40}', '{41}', '{42}', '{43}', '{44}', '{45}')", args);                 
                                gf.InsertTransactionData(sqlInsert);

                            }
                            else
                            {
                                //Do Nothing
                            }

                        }
                        else
                        {
                            QueryPaymentBox.Text = gr.LiveDirectPost(queryParameters);
                            gf.WriteToLog(QueryPaymentBox.Text);

                            //Writes transaction data to DB
                            char firstChar = QueryPaymentBox.Text[0];
                            if (firstChar.ToString() == "r")
                            {

                                string result = dm.QueryStringToJson(QueryPaymentBox.Text);
                                var qro = dm.QueryResultObject(result);
                                object[] args = new object[] { qro.response_code, qro.response_code_text, qro.secondary_response_code, qro.secondary_response_code_text, qro.time_stamp, qro.retry_recommended, qro.authorized_amount, qro.bin, qro.captured_amount, qro.original_authorized_amount, qro.requested_amount, qro.time_stamp_created, qro.original_response_code, qro.original_response_code_text, qro.time_stamp_updated, qro.state, qro.bank_approval_code, qro.expire_month, qro.expire_year, qro.order_id, qro.payer_identifier, qro.reference_id, qro.span, qro.card_brand, qro.batch_id, qro.receipt_application_cryptogram, qro.receipt_application_identifier, qro.receipt_application_preferred_name, qro.receipt_application_transaction_counter, qro.receipt_approval_code, qro.receipt_authorization_response_code, qro.receipt_card_number, qro.receipt_card_type, qro.receipt_entry_legend, qro.receipt_entry_method, qro.receipt_line_items, qro.receipt_merchant_id, qro.receipt_order_id, qro.receipt_signature_text, qro.receipt_terminal_verification_results, qro.receipt_transaction_date_time, qro.receipt_transaction_id, qro.receipt_transaction_reference_number, qro.receipt_transaction_type, qro.receipt_validation_code, qro.receipt_verbiage };
                                string sqlInsert = string.Format("INSERT INTO TRANSACTIONDB (response_code, response_code_text, secondary_response_code, secondary_response_code_text, time_stamp, retry_recommended, authorized_amount, bin, captured_amount, original_authorized_amount, requested_amount, time_stamp_created, original_response_code, original_response_code_text, time_stamp_updated, state, bank_approval_code, expire_month, expire_year, order_id, payer_identifier, reference_id, span, card_brand, batch_id, receipt_application_cryptogram, receipt_application_identifier, receipt_application_preferred_name, receipt_application_transaction_counter, receipt_approval_code, receipt_authorization_reseponse_code, receipt_card_number, receipt_card_type, receipt_entry_legend, receipt_entry_method, receipt_line_items, receipt_merchant_id, receipt_order_id, receipt_signature_text, receipt_terminal_verificiation_results, receipt_transaction_date_time, receipt_transaction_id, receipt_transaction_reference_number, receipt_transaction_type, receipt_validation_code, receipt_verbiage) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}', '{13}', '{14}', '{15}', '{16}', '{17}', '{18}', '{19}', '{20}', '{21}', '{22}', '{23}', '{24}', '{25}', '{26}', '{27}', '{28}', '{29}', '{30}', '{31}', '{32}', '{33}', '{34}', '{35}', '{36}', '{37}', '{38}', '{39}', '{40}', '{41}', '{42}', '{43}', '{44}', '{45}')", args);
                                gf.InsertTransactionData(sqlInsert);

                            }
                            else
                            {
                                //Do Nothing
                            }
                        }

                        //Signature Image
                        try
                        {
                            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                            doc.LoadHtml(gf.GetPageContent(OEHPWebBrowser));

                            string value = doc.DocumentNode.SelectSingleNode("//input[@type='hidden' and @id='signatureImage' and @name='signatureImage']").Attributes["value"].Value;

                            if (null != value)
                            {
                                SignatureImage.Source =  gf.DecodeBase64Image(value);
                            }
                            else
                            {
                                gf.WriteToLog("No Signature Image to Display");
                            }

                        }
                        catch (Exception ex)
                        {
                            gf.WriteToLog(ex.ToString());
                        }
                        break;

                    case "DEBIT_CARD":
                        queryParameters = tr.DirectPostBuilder(AccountTokenBox.Text, OrderIDBox.Text, TransactionTypeBox.SelectedItem.ToString(), "QUERY_PURCHASE");
                        QueryParametersBox.Text = queryParameters;
                        gf.WriteToLog(queryParameters);
                        if (Globals.Default.ProcessingMode == "Test")
                        {
                            QueryPaymentBox.Text = gr.TestDirectPost(queryParameters);
                            gf.WriteToLog(QueryPaymentBox.Text);
                            //Converts QueryString to JSON in Query Response if mode is set.
                            if (Globals.Default.QueryResponseMode == "JSON")
                            {
                                string result = dm.QueryStringToJson(QueryPaymentBox.Text);
                                QueryPaymentBox.Text = result;
                            }

                            //Writes transaction data to DB
                            char firstChar = QueryPaymentBox.Text[0];
                            if (firstChar.ToString() == "r")
                            {

                                string result = dm.QueryStringToJson(QueryPaymentBox.Text);
                                var qro = dm.QueryResultObject(result);
                                object[] args = new object[] { qro.response_code, qro.response_code_text, qro.secondary_response_code, qro.secondary_response_code_text, qro.time_stamp, qro.retry_recommended, qro.authorized_amount, qro.bin, qro.captured_amount, qro.original_authorized_amount, qro.requested_amount, qro.time_stamp_created, qro.original_response_code, qro.original_response_code_text, qro.time_stamp_updated, qro.state, qro.bank_approval_code, qro.expire_month, qro.expire_year, qro.order_id, qro.payer_identifier, qro.reference_id, qro.span, qro.card_brand, qro.batch_id, qro.receipt_application_cryptogram, qro.receipt_application_identifier, qro.receipt_application_preferred_name, qro.receipt_application_transaction_counter, qro.receipt_approval_code, qro.receipt_authorization_response_code, qro.receipt_card_number, qro.receipt_card_type, qro.receipt_entry_legend, qro.receipt_entry_method, qro.receipt_line_items, qro.receipt_merchant_id, qro.receipt_order_id, qro.receipt_signature_text, qro.receipt_terminal_verification_results, qro.receipt_transaction_date_time, qro.receipt_transaction_id, qro.receipt_transaction_reference_number, qro.receipt_transaction_type, qro.receipt_validation_code, qro.receipt_verbiage };
                                string sqlInsert = string.Format("INSERT INTO TRANSACTIONDB (response_code, response_code_text, secondary_response_code, secondary_response_code_text, time_stamp, retry_recommended, authorized_amount, bin, captured_amount, original_authorized_amount, requested_amount, time_stamp_created, original_response_code, original_response_code_text, time_stamp_updated, state, bank_approval_code, expire_month, expire_year, order_id, payer_identifier, reference_id, span, card_brand, batch_id, receipt_application_cryptogram, receipt_application_identifier, receipt_application_preferred_name, receipt_application_transaction_counter, receipt_approval_code, receipt_authorization_reseponse_code, receipt_card_number, receipt_card_type, receipt_entry_legend, receipt_entry_method, receipt_line_items, receipt_merchant_id, receipt_order_id, receipt_signature_text, receipt_terminal_verificiation_results, receipt_transaction_date_time, receipt_transaction_id, receipt_transaction_reference_number, receipt_transaction_type, receipt_validation_code, receipt_verbiage) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}', '{13}', '{14}', '{15}', '{16}', '{17}', '{18}', '{19}', '{20}', '{21}', '{22}', '{23}', '{24}', '{25}', '{26}', '{27}', '{28}', '{29}', '{30}', '{31}', '{32}', '{33}', '{34}', '{35}', '{36}', '{37}', '{38}', '{39}', '{40}', '{41}', '{42}', '{43}', '{44}', '{45}')", args);
                                gf.InsertTransactionData(sqlInsert);

                            }
                            else
                            {
                                //Do Nothing
                            }

                        }
                        else
                        {
                            QueryPaymentBox.Text = gr.LiveDirectPost(queryParameters);
                            gf.WriteToLog(QueryPaymentBox.Text);

                            //Writes transaction data to DB
                            char firstChar = QueryPaymentBox.Text[0];
                            if (firstChar.ToString() == "r")
                            {

                                string result = dm.QueryStringToJson(QueryPaymentBox.Text);
                                var qro = dm.QueryResultObject(result);
                                object[] args = new object[] { qro.response_code, qro.response_code_text, qro.secondary_response_code, qro.secondary_response_code_text, qro.time_stamp, qro.retry_recommended, qro.authorized_amount, qro.bin, qro.captured_amount, qro.original_authorized_amount, qro.requested_amount, qro.time_stamp_created, qro.original_response_code, qro.original_response_code_text, qro.time_stamp_updated, qro.state, qro.bank_approval_code, qro.expire_month, qro.expire_year, qro.order_id, qro.payer_identifier, qro.reference_id, qro.span, qro.card_brand, qro.batch_id, qro.receipt_application_cryptogram, qro.receipt_application_identifier, qro.receipt_application_preferred_name, qro.receipt_application_transaction_counter, qro.receipt_approval_code, qro.receipt_authorization_response_code, qro.receipt_card_number, qro.receipt_card_type, qro.receipt_entry_legend, qro.receipt_entry_method, qro.receipt_line_items, qro.receipt_merchant_id, qro.receipt_order_id, qro.receipt_signature_text, qro.receipt_terminal_verification_results, qro.receipt_transaction_date_time, qro.receipt_transaction_id, qro.receipt_transaction_reference_number, qro.receipt_transaction_type, qro.receipt_validation_code, qro.receipt_verbiage };
                                string sqlInsert = string.Format("INSERT INTO TRANSACTIONDB (response_code, response_code_text, secondary_response_code, secondary_response_code_text, time_stamp, retry_recommended, authorized_amount, bin, captured_amount, original_authorized_amount, requested_amount, time_stamp_created, original_response_code, original_response_code_text, time_stamp_updated, state, bank_approval_code, expire_month, expire_year, order_id, payer_identifier, reference_id, span, card_brand, batch_id, receipt_application_cryptogram, receipt_application_identifier, receipt_application_preferred_name, receipt_application_transaction_counter, receipt_approval_code, receipt_authorization_reseponse_code, receipt_card_number, receipt_card_type, receipt_entry_legend, receipt_entry_method, receipt_line_items, receipt_merchant_id, receipt_order_id, receipt_signature_text, receipt_terminal_verificiation_results, receipt_transaction_date_time, receipt_transaction_id, receipt_transaction_reference_number, receipt_transaction_type, receipt_validation_code, receipt_verbiage) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}', '{13}', '{14}', '{15}', '{16}', '{17}', '{18}', '{19}', '{20}', '{21}', '{22}', '{23}', '{24}', '{25}', '{26}', '{27}', '{28}', '{29}', '{30}', '{31}', '{32}', '{33}', '{34}', '{35}', '{36}', '{37}', '{38}', '{39}', '{40}', '{41}', '{42}', '{43}', '{44}', '{45}')", args);
                                gf.InsertTransactionData(sqlInsert);

                            }
                            else
                            {
                                //Do Nothing
                            }

                        }
                        break;

                    case "ACH":
                        queryParameters = tr.DirectPostBuilder(AccountTokenBox.Text, OrderIDBox.Text, TransactionTypeBox.SelectedItem.ToString(), "QUERY");
                        QueryParametersBox.Text = queryParameters;
                        gf.WriteToLog(queryParameters);
                        if (Globals.Default.ProcessingMode == "Test")
                        {
                            QueryPaymentBox.Text = gr.TestDirectPost(queryParameters);
                            gf.WriteToLog(QueryPaymentBox.Text);

                            //Converts QueryString to JSON in Query Response if mode is set.
                            if (Globals.Default.QueryResponseMode == "JSON")
                            {
                                string result = dm.QueryStringToJson(QueryPaymentBox.Text);
                                QueryPaymentBox.Text = result;
                            }

                            //Writes transaction data to DB
                            char firstChar = QueryPaymentBox.Text[0];
                            if (firstChar.ToString() == "r")
                            {

                                string result = dm.QueryStringToJson(QueryPaymentBox.Text);
                                var qro = dm.QueryResultObject(result);
                                object[] args = new object[] { qro.response_code, qro.response_code_text, qro.secondary_response_code, qro.secondary_response_code_text, qro.time_stamp, qro.retry_recommended, qro.authorized_amount, qro.bin, qro.captured_amount, qro.original_authorized_amount, qro.requested_amount, qro.time_stamp_created, qro.original_response_code, qro.original_response_code_text, qro.time_stamp_updated, qro.state, qro.bank_approval_code, qro.expire_month, qro.expire_year, qro.order_id, qro.payer_identifier, qro.reference_id, qro.span, qro.card_brand, qro.batch_id, qro.receipt_application_cryptogram, qro.receipt_application_identifier, qro.receipt_application_preferred_name, qro.receipt_application_transaction_counter, qro.receipt_approval_code, qro.receipt_authorization_response_code, qro.receipt_card_number, qro.receipt_card_type, qro.receipt_entry_legend, qro.receipt_entry_method, qro.receipt_line_items, qro.receipt_merchant_id, qro.receipt_order_id, qro.receipt_signature_text, qro.receipt_terminal_verification_results, qro.receipt_transaction_date_time, qro.receipt_transaction_id, qro.receipt_transaction_reference_number, qro.receipt_transaction_type, qro.receipt_validation_code, qro.receipt_verbiage };
                                string sqlInsert = string.Format("INSERT INTO TRANSACTIONDB (response_code, response_code_text, secondary_response_code, secondary_response_code_text, time_stamp, retry_recommended, authorized_amount, bin, captured_amount, original_authorized_amount, requested_amount, time_stamp_created, original_response_code, original_response_code_text, time_stamp_updated, state, bank_approval_code, expire_month, expire_year, order_id, payer_identifier, reference_id, span, card_brand, batch_id, receipt_application_cryptogram, receipt_application_identifier, receipt_application_preferred_name, receipt_application_transaction_counter, receipt_approval_code, receipt_authorization_reseponse_code, receipt_card_number, receipt_card_type, receipt_entry_legend, receipt_entry_method, receipt_line_items, receipt_merchant_id, receipt_order_id, receipt_signature_text, receipt_terminal_verificiation_results, receipt_transaction_date_time, receipt_transaction_id, receipt_transaction_reference_number, receipt_transaction_type, receipt_validation_code, receipt_verbiage) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}', '{13}', '{14}', '{15}', '{16}', '{17}', '{18}', '{19}', '{20}', '{21}', '{22}', '{23}', '{24}', '{25}', '{26}', '{27}', '{28}', '{29}', '{30}', '{31}', '{32}', '{33}', '{34}', '{35}', '{36}', '{37}', '{38}', '{39}', '{40}', '{41}', '{42}', '{43}', '{44}', '{45}')", args);
                                gf.InsertTransactionData(sqlInsert);

                            }
                            else
                            {
                                //Do Nothing
                            }

                        }
                        else
                        {
                            QueryPaymentBox.Text = gr.LiveDirectPost(queryParameters);
                            gf.WriteToLog(QueryPaymentBox.Text);

                            //Writes transaction data to DB
                            char firstChar = QueryPaymentBox.Text[0];
                            if (firstChar.ToString() == "r")
                            {

                                string result = dm.QueryStringToJson(QueryPaymentBox.Text);
                                var qro = dm.QueryResultObject(result);
                                object[] args = new object[] { qro.response_code, qro.response_code_text, qro.secondary_response_code, qro.secondary_response_code_text, qro.time_stamp, qro.retry_recommended, qro.authorized_amount, qro.bin, qro.captured_amount, qro.original_authorized_amount, qro.requested_amount, qro.time_stamp_created, qro.original_response_code, qro.original_response_code_text, qro.time_stamp_updated, qro.state, qro.bank_approval_code, qro.expire_month, qro.expire_year, qro.order_id, qro.payer_identifier, qro.reference_id, qro.span, qro.card_brand, qro.batch_id, qro.receipt_application_cryptogram, qro.receipt_application_identifier, qro.receipt_application_preferred_name, qro.receipt_application_transaction_counter, qro.receipt_approval_code, qro.receipt_authorization_response_code, qro.receipt_card_number, qro.receipt_card_type, qro.receipt_entry_legend, qro.receipt_entry_method, qro.receipt_line_items, qro.receipt_merchant_id, qro.receipt_order_id, qro.receipt_signature_text, qro.receipt_terminal_verification_results, qro.receipt_transaction_date_time, qro.receipt_transaction_id, qro.receipt_transaction_reference_number, qro.receipt_transaction_type, qro.receipt_validation_code, qro.receipt_verbiage };
                                string sqlInsert = string.Format("INSERT INTO TRANSACTIONDB (response_code, response_code_text, secondary_response_code, secondary_response_code_text, time_stamp, retry_recommended, authorized_amount, bin, captured_amount, original_authorized_amount, requested_amount, time_stamp_created, original_response_code, original_response_code_text, time_stamp_updated, state, bank_approval_code, expire_month, expire_year, order_id, payer_identifier, reference_id, span, card_brand, batch_id, receipt_application_cryptogram, receipt_application_identifier, receipt_application_preferred_name, receipt_application_transaction_counter, receipt_approval_code, receipt_authorization_reseponse_code, receipt_card_number, receipt_card_type, receipt_entry_legend, receipt_entry_method, receipt_line_items, receipt_merchant_id, receipt_order_id, receipt_signature_text, receipt_terminal_verificiation_results, receipt_transaction_date_time, receipt_transaction_id, receipt_transaction_reference_number, receipt_transaction_type, receipt_validation_code, receipt_verbiage) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}', '{13}', '{14}', '{15}', '{16}', '{17}', '{18}', '{19}', '{20}', '{21}', '{22}', '{23}', '{24}', '{25}', '{26}', '{27}', '{28}', '{29}', '{30}', '{31}', '{32}', '{33}', '{34}', '{35}', '{36}', '{37}', '{38}', '{39}', '{40}', '{41}', '{42}', '{43}', '{44}', '{45}')", args);
                                gf.InsertTransactionData(sqlInsert);

                            }
                            else
                            {
                                //Do Nothing
                            }

                        }
                        break;

                    case "INTERAC":
                        queryParameters = tr.DirectPostBuilder(AccountTokenBox.Text, OrderIDBox.Text, TransactionTypeBox.SelectedItem.ToString(), "QUERY_PURCHASE");
                        QueryParametersBox.Text = queryParameters;
                        gf.WriteToLog(queryParameters);
                        if (Globals.Default.ProcessingMode == "Test")
                        {
                            QueryPaymentBox.Text = gr.TestDirectPost(queryParameters);
                            gf.WriteToLog(QueryPaymentBox.Text);

                            //Converts QueryString to JSON in Query Response if mode is set.
                            if (Globals.Default.QueryResponseMode == "JSON")
                            {
                                string result = dm.QueryStringToJson(QueryPaymentBox.Text);
                                QueryPaymentBox.Text = result;
                            }

                            //Writes transaction data to DB
                            char firstChar = QueryPaymentBox.Text[0];
                            if (firstChar.ToString() == "r")
                            {

                                string result = dm.QueryStringToJson(QueryPaymentBox.Text);
                                var qro = dm.QueryResultObject(result);
                                object[] args = new object[] { qro.response_code, qro.response_code_text, qro.secondary_response_code, qro.secondary_response_code_text, qro.time_stamp, qro.retry_recommended, qro.authorized_amount, qro.bin, qro.captured_amount, qro.original_authorized_amount, qro.requested_amount, qro.time_stamp_created, qro.original_response_code, qro.original_response_code_text, qro.time_stamp_updated, qro.state, qro.bank_approval_code, qro.expire_month, qro.expire_year, qro.order_id, qro.payer_identifier, qro.reference_id, qro.span, qro.card_brand, qro.batch_id, qro.receipt_application_cryptogram, qro.receipt_application_identifier, qro.receipt_application_preferred_name, qro.receipt_application_transaction_counter, qro.receipt_approval_code, qro.receipt_authorization_response_code, qro.receipt_card_number, qro.receipt_card_type, qro.receipt_entry_legend, qro.receipt_entry_method, qro.receipt_line_items, qro.receipt_merchant_id, qro.receipt_order_id, qro.receipt_signature_text, qro.receipt_terminal_verification_results, qro.receipt_transaction_date_time, qro.receipt_transaction_id, qro.receipt_transaction_reference_number, qro.receipt_transaction_type, qro.receipt_validation_code, qro.receipt_verbiage };
                                string sqlInsert = string.Format("INSERT INTO TRANSACTIONDB (response_code, response_code_text, secondary_response_code, secondary_response_code_text, time_stamp, retry_recommended, authorized_amount, bin, captured_amount, original_authorized_amount, requested_amount, time_stamp_created, original_response_code, original_response_code_text, time_stamp_updated, state, bank_approval_code, expire_month, expire_year, order_id, payer_identifier, reference_id, span, card_brand, batch_id, receipt_application_cryptogram, receipt_application_identifier, receipt_application_preferred_name, receipt_application_transaction_counter, receipt_approval_code, receipt_authorization_reseponse_code, receipt_card_number, receipt_card_type, receipt_entry_legend, receipt_entry_method, receipt_line_items, receipt_merchant_id, receipt_order_id, receipt_signature_text, receipt_terminal_verificiation_results, receipt_transaction_date_time, receipt_transaction_id, receipt_transaction_reference_number, receipt_transaction_type, receipt_validation_code, receipt_verbiage) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}', '{13}', '{14}', '{15}', '{16}', '{17}', '{18}', '{19}', '{20}', '{21}', '{22}', '{23}', '{24}', '{25}', '{26}', '{27}', '{28}', '{29}', '{30}', '{31}', '{32}', '{33}', '{34}', '{35}', '{36}', '{37}', '{38}', '{39}', '{40}', '{41}', '{42}', '{43}', '{44}', '{45}')", args);
                                gf.InsertTransactionData(sqlInsert);

                            }
                            else
                            {
                                //Do Nothing
                            }

                        }
                        else
                        {
                            QueryPaymentBox.Text = gr.LiveDirectPost(queryParameters);
                            gf.WriteToLog(QueryPaymentBox.Text);

                            //Writes transaction data to DB
                            char firstChar = QueryPaymentBox.Text[0];
                            if (firstChar.ToString() == "r")
                            {

                                string result = dm.QueryStringToJson(QueryPaymentBox.Text);
                                var qro = dm.QueryResultObject(result);
                                object[] args = new object[] { qro.response_code, qro.response_code_text, qro.secondary_response_code, qro.secondary_response_code_text, qro.time_stamp, qro.retry_recommended, qro.authorized_amount, qro.bin, qro.captured_amount, qro.original_authorized_amount, qro.requested_amount, qro.time_stamp_created, qro.original_response_code, qro.original_response_code_text, qro.time_stamp_updated, qro.state, qro.bank_approval_code, qro.expire_month, qro.expire_year, qro.order_id, qro.payer_identifier, qro.reference_id, qro.span, qro.card_brand, qro.batch_id, qro.receipt_application_cryptogram, qro.receipt_application_identifier, qro.receipt_application_preferred_name, qro.receipt_application_transaction_counter, qro.receipt_approval_code, qro.receipt_authorization_response_code, qro.receipt_card_number, qro.receipt_card_type, qro.receipt_entry_legend, qro.receipt_entry_method, qro.receipt_line_items, qro.receipt_merchant_id, qro.receipt_order_id, qro.receipt_signature_text, qro.receipt_terminal_verification_results, qro.receipt_transaction_date_time, qro.receipt_transaction_id, qro.receipt_transaction_reference_number, qro.receipt_transaction_type, qro.receipt_validation_code, qro.receipt_verbiage };
                                string sqlInsert = string.Format("INSERT INTO TRANSACTIONDB (response_code, response_code_text, secondary_response_code, secondary_response_code_text, time_stamp, retry_recommended, authorized_amount, bin, captured_amount, original_authorized_amount, requested_amount, time_stamp_created, original_response_code, original_response_code_text, time_stamp_updated, state, bank_approval_code, expire_month, expire_year, order_id, payer_identifier, reference_id, span, card_brand, batch_id, receipt_application_cryptogram, receipt_application_identifier, receipt_application_preferred_name, receipt_application_transaction_counter, receipt_approval_code, receipt_authorization_reseponse_code, receipt_card_number, receipt_card_type, receipt_entry_legend, receipt_entry_method, receipt_line_items, receipt_merchant_id, receipt_order_id, receipt_signature_text, receipt_terminal_verificiation_results, receipt_transaction_date_time, receipt_transaction_id, receipt_transaction_reference_number, receipt_transaction_type, receipt_validation_code, receipt_verbiage) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}', '{13}', '{14}', '{15}', '{16}', '{17}', '{18}', '{19}', '{20}', '{21}', '{22}', '{23}', '{24}', '{25}', '{26}', '{27}', '{28}', '{29}', '{30}', '{31}', '{32}', '{33}', '{34}', '{35}', '{36}', '{37}', '{38}', '{39}', '{40}', '{41}', '{42}', '{43}', '{44}', '{45}')", args);
                                gf.InsertTransactionData(sqlInsert);

                            }
                            else
                            {
                                //Do Nothing
                            }

                        }
                        break;

                    default:
                        gf.WriteToLog("Improper Selection for Query Payment");
                        break;
                }
            }
        }

        private void CreditTypeBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                switch (CreditTypeBox.SelectedItem.ToString())
                {
                    case "INDEPENDENT":
                        OrderIDBox.IsReadOnly = true;
                        break;

                    case "DEPENDENT":
                        OrderIDBox.IsReadOnly = false;
                        break;

                    default:
                        OrderIDBox.IsReadOnly = true;
                        break;
                }
            }
            catch (Exception ex)
            {
                GeneralFunctions gf = new GeneralFunctions();
                gf.WriteToLog(ex.ToString());
            }
        }

        private void QueryToJsonButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string queryString = QueryPaymentBox.Text;
                char firstChar = queryString[0];
                if (firstChar.ToString() == "r")
                {
                    OEHP.NET.DataManipulation dm = new OEHP.NET.DataManipulation();
                    
                    string result = dm.QueryStringToJson(queryString);
                    QueryPaymentBox.Text = result;
                }
                else
                {
                    MessageBox.Show("Data in Query Payment Cannot Be Converted.");
                }
            }
            catch (Exception ex)
            {
                GeneralFunctions gf = new GeneralFunctions();
                gf.WriteToLog(ex.ToString());
            }
        }

        private void SaveToken_Click(object sender, RoutedEventArgs e)
        {
            Globals.Default.AccountToken = AccountTokenBox.Text;
            Globals.Default.Save();

        }

        private void AccountTokenBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            Globals.Default.AccountToken = Globals.Default.DefaultAccountToken;
            Globals.Default.Save();
            AccountTokenBox.Text = Globals.Default.AccountToken;
        }

        private void File_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void PresetEMVTesting_Click(object sender, RoutedEventArgs e)
        {
            AccountTokenBox.Text = Globals.Default.DefaultAccountToken;
        }

        private void PresetCanadianTesting_Click(object sender, RoutedEventArgs e)
        {
            AccountTokenBox.Text = Globals.Default.CanadianAccountToken;
        }

        private void PresetLoopBackTesting_Click(object sender, RoutedEventArgs e)
        {
            AccountTokenBox.Text = Globals.Default.LoopBackAccountToken;
        }

        private void PresetHelp_Click(object sender, RoutedEventArgs e)
        {
            PresetHelp ph = new PresetHelp();
            ph.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            ph.ShowDialog();
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

        private void ProcessingModeTest_Click(object sender, RoutedEventArgs e)
        {
            Globals.Default.ProcessingMode = "Test";
            if (ProcessingModeLive.IsChecked == true)
            {
                ProcessingModeLive.IsChecked = false;
            }
            ProcessingModeTest.IsChecked = true;
        }

        private void ProcessingModeLive_Click(object sender, RoutedEventArgs e)
        {
            Globals.Default.ProcessingMode = "Live";
            if (ProcessingModeTest.IsChecked == true)
            {
                ProcessingModeTest.IsChecked = false;
            }
            ProcessingModeLive.IsChecked = true;
        }

        private void PayPageFields_Click(object sender, RoutedEventArgs e)
        {
            PayPageCustomization ppf = new PayPageCustomization();
            ppf.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            ppf.ShowDialog();

        }

        private void PayPageBranding_Click(object sender, RoutedEventArgs e)
        {

        }
    }
    
}

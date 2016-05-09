using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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

            SubmitMethodBoxValues.Clear();
            SubmitMethodBoxValues.Add("PayPage Post");
            SubmitMethodBoxValues.Add("HTML Doc Post");
            SubmitMethodBoxValues.Add("Direct Post");
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

        public object HttpUtility { get; private set; }

        private void submitButton_Click(object sender, RoutedEventArgs e)
        {

            //Create a new transaction Request Object
            TransactionRequest tr = new TransactionRequest();
            //string TestTransactionRequestString = tr.CreditCardParamBuilder("04173F8DCE65520D3580E5FF8555A961CECF249E46B5C2FAEFA04E248CD95FEA9D55BB581758D0591B", "CREDIT_CARD", "SALE", "KEYED", tr.OrderIDRandom(8), "1", ""); // Test Value used for Validation during debug.
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
                                if (ModeComboBox.SelectedItem.ToString() == "Test")
                                {
                                    OEHPWebBrowser.Navigate(gr.TestPayPagePost(transactionRequestString));

                                }
                                if (ModeComboBox.SelectedItem.ToString() == "Live")
                                {
                                    OEHPWebBrowser.Navigate(gr.LivePayPagePost(transactionRequestString));
                                }
                                break;
                            case "HTML Doc Post":
                                if (ModeComboBox.SelectedItem.ToString() == "Test")
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
                                if (ModeComboBox.SelectedItem.ToString() == "Test")
                                {
                                    OEHPWebBrowser.Navigate(gr.TestPayPagePost(transactionRequestString));

                                }
                                if (ModeComboBox.SelectedItem.ToString() == "Live")
                                {
                                    ;
                                    OEHPWebBrowser.Navigate(gr.LivePayPagePost(transactionRequestString));
                                }
                                break;
                            case "HTML Doc Post":
                                if (ModeComboBox.SelectedItem.ToString() == "Test")
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
                                if (ModeComboBox.SelectedItem.ToString() == "Test")
                                {
                                    OEHPWebBrowser.Navigate(gr.TestPayPagePost(transactionRequestString));

                                }
                                if (ModeComboBox.SelectedItem.ToString() == "Live")
                                {
                                    OEHPWebBrowser.Navigate(gr.LivePayPagePost(transactionRequestString));
                                }
                                break;
                            case "HTML Doc Post":
                                if (ModeComboBox.SelectedItem.ToString() == "Test")
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
                                if (ModeComboBox.SelectedItem.ToString() == "Test")
                                {
                                    OEHPWebBrowser.Navigate(gr.TestPayPagePost(transactionRequestString));

                                }
                                if (ModeComboBox.SelectedItem.ToString() == "Live")
                                {
                                    OEHPWebBrowser.Navigate(gr.LivePayPagePost(transactionRequestString));
                                }
                                break;
                            case "HTML Doc Post":
                                if (ModeComboBox.SelectedItem.ToString() == "Test")
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
                                if (ModeComboBox.SelectedItem.ToString() == "Test")
                                {
                                    OEHPWebBrowser.Navigate(gr.TestPayPagePost(transactionRequestString));

                                }
                                if (ModeComboBox.SelectedItem.ToString() == "Live")
                                {
                                    OEHPWebBrowser.Navigate(gr.LivePayPagePost(transactionRequestString));
                                }
                                break;
                            case "HTML Doc Post":
                                if (ModeComboBox.SelectedItem.ToString() == "Test")
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
                                if (ModeComboBox.SelectedItem.ToString() == "Test")
                                {
                                    OEHPWebBrowser.Navigate(gr.TestPayPagePost(transactionRequestString));

                                }
                                if (ModeComboBox.SelectedItem.ToString() == "Live")
                                {
                                    OEHPWebBrowser.Navigate(gr.LivePayPagePost(transactionRequestString));
                                }
                                break;
                            case "HTML Doc Post":
                                if (ModeComboBox.SelectedItem.ToString() == "Test")
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
                        
                        if (ModeComboBox.SelectedItem.ToString() == "Test")
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
                                if (ModeComboBox.SelectedItem.ToString() == "Test")
                                {
                                    OEHPWebBrowser.Navigate(gr.TestPayPagePost(transactionRequestString));

                                }
                                if (ModeComboBox.SelectedItem.ToString() == "Live")
                                {
                                    OEHPWebBrowser.Navigate(gr.LivePayPagePost(transactionRequestString));
                                }
                                break;
                            case "HTML Doc Post":
                                if (ModeComboBox.SelectedItem.ToString() == "Test")
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
                        transactionRequestString = tr.DirectPostBuilder(AccountTokenBox.Text, OrderIDBox.Text, TransactionTypeBox.SelectedItem.ToString(), ChargeTypeBox.SelectedItem.ToString());
                        gf.WriteToLog(transactionRequestString);
                        PostParameterBox.Text = transactionRequestString;
                        if (ModeComboBox.SelectedItem.ToString() == "Test")
                        {
                            OEHPWebBrowser.NavigateToString(gr.TestDirectPost(transactionRequestString));
                        }
                        else
                        {
                            OEHPWebBrowser.NavigateToString(gr.LiveDirectPost(transactionRequestString));
                        }
                        break;

                    case "ADJUSTMENT":
                        transactionRequestString = tr.DirectPostBuilder(AccountTokenBox.Text, OrderIDBox.Text, TransactionTypeBox.SelectedItem.ToString(), ChargeTypeBox.SelectedItem.ToString());
                        gf.WriteToLog(transactionRequestString);
                        PostParameterBox.Text = transactionRequestString;
                        if (ModeComboBox.SelectedItem.ToString() == "Test")
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
                                        if (ModeComboBox.SelectedItem.ToString() == "Test")
                                        {
                                            OEHPWebBrowser.Navigate(gr.TestPayPagePost(transactionRequestString));

                                        }
                                        if (ModeComboBox.SelectedItem.ToString() == "Live")
                                        {
                                            OEHPWebBrowser.Navigate(gr.LivePayPagePost(transactionRequestString));
                                        }
                                        break;
                                    case "HTML Doc Post":
                                        if (ModeComboBox.SelectedItem.ToString() == "Test")
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
                                transactionRequestString = tr.DirectPostBuilder(AccountTokenBox.Text, OrderIDBox.Text, TransactionTypeBox.SelectedItem.ToString(), ChargeTypeBox.SelectedItem.ToString());
                                gf.WriteToLog(transactionRequestString);
                                PostParameterBox.Text = transactionRequestString;
                                if (ModeComboBox.SelectedItem.ToString() == "Test")
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
                                        if (ModeComboBox.SelectedItem.ToString() == "Test")
                                        {
                                            OEHPWebBrowser.Navigate(gr.TestPayPagePost(transactionRequestString));

                                        }
                                        if (ModeComboBox.SelectedItem.ToString() == "Live")
                                        {
                                            OEHPWebBrowser.Navigate(gr.LivePayPagePost(transactionRequestString));
                                        }
                                        break;
                                    case "HTML Doc Post":
                                        if (ModeComboBox.SelectedItem.ToString() == "Test")
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
                                transactionRequestString = tr.DirectPostBuilder(AccountTokenBox.Text, OrderIDBox.Text, TransactionTypeBox.SelectedItem.ToString(), ChargeTypeBox.SelectedItem.ToString());
                                gf.WriteToLog(transactionRequestString);
                                PostParameterBox.Text = transactionRequestString;
                                if (ModeComboBox.SelectedItem.ToString() == "Test")
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




            
            /* Testing Code
            try
            {
                switch (SubmitMethodBox.SelectedItem.ToString())
                {
                    case "PayPage Post":
                        if (ModeComboBox.SelectedItem.ToString() == "Test")
                        {
                            OEHPWebBrowser.Navigate(gr.TestPayPagePost(TestTransactionRequestString));
                            
                        }
                        if (ModeComboBox.SelectedItem.ToString() == "Live")
                        {
                            OEHPWebBrowser.Navigate(gr.LivePayPagePost(TestTransactionRequestString));
                        }
                        break;
                    case "HTML Doc Post":
                        OEHPWebBrowser.NavigateToString(gr.TestHtmlDocPost(TestTransactionRequestString));
                        break;
                    case "Direct Post":
                    default:
                        OEHPWebBrowser.NavigateToString("No Submit Method Selected. Transaction Failed");
                        break;

                }
            }
            catch (Exception ex)
            {

                OEHPWebBrowser.NavigateToString("An Exception Occured. Please Check Parameters");
            }
            */

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

        }

        public void RCMStatus()
        {
            try
            {
                string ssp = OEHP.NET.SSP.SessionToken;
                
                if (ModeComboBox.SelectedItem.ToString() == "Test")
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
                        if (ModeComboBox.SelectedItem.ToString() == "Test")
                        {
                            QueryPaymentBox.Text = gr.TestDirectPost(queryParameters);
                            gf.WriteToLog(QueryParametersBox.Text);
                        }
                        else
                        {
                            QueryPaymentBox.Text = gr.LiveDirectPost(queryParameters);
                            gf.WriteToLog(QueryParametersBox.Text);
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
                        if (ModeComboBox.SelectedItem.ToString() == "Test")
                        {
                            QueryPaymentBox.Text = gr.TestDirectPost(queryParameters);
                            gf.WriteToLog(QueryParametersBox.Text);
                        }
                        else
                        {
                            QueryPaymentBox.Text = gr.LiveDirectPost(queryParameters);
                            gf.WriteToLog(QueryParametersBox.Text);
                        }
                        break;

                    case "ACH":
                        queryParameters = tr.DirectPostBuilder(AccountTokenBox.Text, OrderIDBox.Text, TransactionTypeBox.SelectedItem.ToString(), "QUERY");
                        QueryParametersBox.Text = queryParameters;
                        gf.WriteToLog(queryParameters);
                        if (ModeComboBox.SelectedItem.ToString() == "Test")
                        {
                            QueryPaymentBox.Text = gr.TestDirectPost(queryParameters);
                            gf.WriteToLog(QueryParametersBox.Text);
                        }
                        else
                        {
                            QueryPaymentBox.Text = gr.LiveDirectPost(queryParameters);
                            gf.WriteToLog(QueryParametersBox.Text);
                        }
                        break;

                    case "INTERAC":
                        queryParameters = tr.DirectPostBuilder(AccountTokenBox.Text, OrderIDBox.Text, TransactionTypeBox.SelectedItem.ToString(), "QUERY_PURCHASE");
                        QueryParametersBox.Text = queryParameters;
                        gf.WriteToLog(queryParameters);
                        if (ModeComboBox.SelectedItem.ToString() == "Test")
                        {
                            QueryPaymentBox.Text = gr.TestDirectPost(queryParameters);
                            gf.WriteToLog(QueryParametersBox.Text);
                        }
                        else
                        {
                            QueryPaymentBox.Text = gr.LiveDirectPost(queryParameters);
                            gf.WriteToLog(QueryParametersBox.Text);
                        }
                        break;

                    default:
                        gf.WriteToLog("Improper Selection for Query Payment");
                        break;
                }
            }
        }
    }
}

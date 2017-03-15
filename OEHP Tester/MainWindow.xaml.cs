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
using OEHP.NET;

namespace OEHP_Tester
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {
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
                CheckForLive();
            }
            if (Globals.Default.ProcessingMode == "Test")
            {
                ProcessingModeTest.IsChecked = true;
            }
        }
        private void SubmitModeConfigurationUIElements() // Sets mode for sending transactions, whether its a One-Button submmission or it first Builds a Post then sends the parameters to teh gateway
        {
            if (Globals.Default.SubmitMode == "OneButton")
            {
                SubmitOptionsLabel.Visibility = Visibility.Hidden;
                BuildSubmitMethodSelectorComboBox.Visibility = Visibility.Hidden;
                buildPostButton.Visibility = Visibility.Hidden;
                submitPostButton.Visibility = Visibility.Hidden;
                OneButton.IsChecked = true;
                BuildThenSubmit.IsChecked = false;

                submitButton.Visibility = Visibility.Visible;
                SubmitMethodBox.Visibility = Visibility.Visible;
                SubmitMethodLabel.Visibility = Visibility.Visible;
            }
            if (Globals.Default.SubmitMode == "BuildThenSubmit")
            {
                SubmitOptionsLabel.Visibility = Visibility.Visible;
                BuildSubmitMethodSelectorComboBox.Visibility = Visibility.Visible;
                buildPostButton.Visibility = Visibility.Visible;
                submitPostButton.Visibility = Visibility.Visible;
                BuildThenSubmit.IsChecked = true;
                OneButton.IsChecked = false;

                submitButton.Visibility = Visibility.Hidden;
                SubmitMethodBox.Visibility = Visibility.Hidden;
                SubmitMethodLabel.Visibility = Visibility.Hidden;

            }
        }
        private void SetCommonComboBoxCollections()//Sets the ItemsSource for the Comboboxes that are not determined by TransactionType or ChargeType
        {
            SubmitMethodBox.ItemsSource = UICollections.SubmitMethodBoxValues();
            ModeComboBox.ItemsSource = UICollections.ModeBoxValues();
            TransactionTypeBox.ItemsSource = UICollections.TransactionTypeValues();
            TCCBox.ItemsSource = UICollections.TCCValues();
            AccountTypeBox.ItemsSource = UICollections.AccountTypeValues();
            CreditTypeBox.ItemsSource = UICollections.CreditTypeValues();
        } 
        private void CheckForLive() //Checks if Live mode is Enabled, if it is asks for Username and Password to validate.
        {

            MessageBox.Show("Live Mode Detected, please re-enter Username and password.");


            LoginForLive login = new LoginForLive();
            if (login.ShowDialog().Value == false)
            {
                bool CorrectLogin = login.CorrectLogin;
                if (CorrectLogin == true)
                {
                    Globals.Default.ProcessingMode = "Live";
                    if (ProcessingModeTest.IsChecked == true)
                    {
                        ProcessingModeTest.IsChecked = false;
                    }
                    ProcessingModeLive.IsChecked = true;
                }
                else
                {
                    MessageBox.Show("Live processing restricted to OpenEdge. Please contact your OpenEdge Representative.");
                }
            }
        }
        private void RenderBrowser(ResponseForWebBrowser responseToRender) //Renders PayPage in the UI
        {
            switch (responseToRender.submitMethodUsed)
            {
                case "payPagePost":
                    OEHPWebBrowser.Navigate(responseToRender.actionURL + responseToRender.sealedSetupParameters);
                    break;
                case "directPost":
                    OEHPWebBrowser.NavigateToString(responseToRender.directPostResponse);
                    break;
                case "htmlDocPost":
                    OEHPWebBrowser.NavigateToString(responseToRender.htmlDoc);
                    break;
                case "error":
                    OEHPWebBrowser.NavigateToString(responseToRender.errorMessage);
                    break;
                default:
                    OEHPWebBrowser.NavigateToString("Error Occured, please check all parameters");
                    break;
            }
        }
        #endregion
        #region UI - ComboBox Methods
        private void CreditTypeBox_SelectionChanged(object sender, SelectionChangedEventArgs e)//Sets ReadOnly Characterists of ORderID field based on Selection of Credit Type (Dependent Independent)
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
                GeneralFunctions.WriteToLog(ex.ToString());
            }
        }
        private void ChargeTypeBox_SelectionChanged(object sender, SelectionChangedEventArgs e) //Sets Secondary ComboBox Visibility and OrderID ReadOnly state for fields based on Charge Type
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

                    case "QUERY_PAYMENT":
                        OrderIDBox.IsReadOnly = false;

                        CreditTypeBox.Visibility = Visibility.Hidden;
                        CreditTypeLabel.Visibility = Visibility.Hidden;

                        ApprovalCodeBox.Visibility = Visibility.Hidden;
                        ApprovalCodeLabel.Visibility = Visibility.Hidden;

                        break;

                    case "QUERY_PURCHASE":
                        OrderIDBox.IsReadOnly = false;

                        CreditTypeBox.Visibility = Visibility.Hidden;
                        CreditTypeLabel.Visibility = Visibility.Hidden;

                        ApprovalCodeBox.Visibility = Visibility.Hidden;
                        ApprovalCodeLabel.Visibility = Visibility.Hidden;

                        break;

                    case "QUERY":
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
                GeneralFunctions.WriteToLog(ex.ToString());
            }
        }
        private void TransactionTypeBox_SelectionChanged(object sender, SelectionChangedEventArgs e)// Sets ChargeType, EntryMode, and other ComboBOx Visibility based on Transaction Type
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

                        EntryModeBox.ItemsSource = UICollections.CreditEntryModeValues();
                        EntryModeBox.SelectedIndex = 0;

                        ChargeTypeBox.ItemsSource = UICollections.CreditChargeTypeValues();
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

                        EntryModeBox.ItemsSource = UICollections.DebitEntryModeValues();
                        EntryModeBox.SelectedIndex = 0;

                        AccountTypeBox.SelectedIndex = 0;

                        ChargeTypeBox.ItemsSource = UICollections.DebitChargeTypeValues();
                        ChargeTypeBox.SelectedIndex = 0;

                        CreditTypeBox.SelectedIndex = -1;
                        TCCBox.SelectedIndex = -1;

                        break;

                    case "CREDIT_DEBIT_CARD":

                        AccountTypeBox.Visibility = Visibility.Visible;
                        AccountTypeLabel.Visibility = Visibility.Visible;
                        CreditTypeLabel.Visibility = Visibility.Hidden;
                        CreditTypeBox.Visibility = Visibility.Hidden;
                        TCCBox.Visibility = Visibility.Hidden;
                        TCCLabel.Visibility = Visibility.Hidden;
                        ApprovalCodeBox.Visibility = Visibility.Hidden;
                        ApprovalCodeLabel.Visibility = Visibility.Hidden;

                        EntryModeBox.ItemsSource = UICollections.DebitEntryModeValues(); //Credit_Debit_Card does not have unique values, but inherits the most retrictive Entry mode 
                        EntryModeBox.SelectedIndex = 0;

                        AccountTypeBox.SelectedIndex = 0;

                        ChargeTypeBox.ItemsSource = UICollections.CreditDebitChargeTypeValues();
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

                        EntryModeBox.ItemsSource = UICollections.ACHEntryModeValues();
                        EntryModeBox.SelectedIndex = 0;

                        ChargeTypeBox.ItemsSource = UICollections.ACHChargeTypeValues();
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

                        EntryModeBox.ItemsSource = UICollections.DebitEntryModeValues();
                        EntryModeBox.SelectedIndex = 0;

                        ChargeTypeBox.ItemsSource = UICollections.DebitChargeTypeValues();
                        ChargeTypeBox.SelectedIndex = 0;

                        CreditTypeBox.SelectedIndex = -1;
                        TCCBox.SelectedIndex = -1;

                        break;
                }
            }
            catch (Exception ex)
            {
                GeneralFunctions.WriteToLog(ex.ToString());
            }
        }
        #endregion
        #region UI - Button Interaction
        private void submitButton_Click(object sender, RoutedEventArgs e) 
        {
            ResponseForWebBrowser response = new ResponseForWebBrowser();
            try
            {
                switch (ChargeTypeBox.Text)
                {
                    case "SALE": //Credit_Card Sale
                        OrderIDBox.Text = TransactionRequest.OrderIDRandom(8);
                        response = PaymentEngine.PerformCreditSaleTransaction(AccountTokenBox.Text, TransactionTypeBox.Text, ChargeTypeBox.Text, EntryModeBox.Text, OrderIDBox.Text, AmountBox.Text, CustomParametersBox.Text, SubmitMethodBox.Text);
                        RenderBrowser(response);
                        break;

                    case "AUTH": //Credit_Card AUTH
                        OrderIDBox.Text = TransactionRequest.OrderIDRandom(8);
                        response = PaymentEngine.PerformCreditSaleTransaction(AccountTokenBox.Text, TransactionTypeBox.Text, ChargeTypeBox.Text, EntryModeBox.Text, OrderIDBox.Text, AmountBox.Text, CustomParametersBox.Text, SubmitMethodBox.Text);
                        RenderBrowser(response);
                        break;
                    case "SIGNATURE"://Credit_Card Signature
                        OrderIDBox.Text = TransactionRequest.OrderIDRandom(8);
                        response = PaymentEngine.PerformCreditSaleTransaction(AccountTokenBox.Text, TransactionTypeBox.Text, ChargeTypeBox.Text, EntryModeBox.Text, OrderIDBox.Text, AmountBox.Text, CustomParametersBox.Text, SubmitMethodBox.Text);
                        RenderBrowser(response);
                        break;

                    case "PURCHASE": //Debit Purchase
                        OrderIDBox.Text = TransactionRequest.OrderIDRandom(8);
                        response = PaymentEngine.PerformDebitPurchaseTransaction(AccountTokenBox.Text, TransactionTypeBox.Text, ChargeTypeBox.Text, EntryModeBox.Text,
                                                        OrderIDBox.Text, AmountBox.Text, AccountTypeBox.Text, CustomParametersBox.Text, SubmitMethodBox.Text);
                        RenderBrowser(response);
                        break;

                    case "REFUND": // Debit Refund
                        OrderIDBox.Text = TransactionRequest.OrderIDRandom(8);
                        response = PaymentEngine.PerformDebitRefundTransaction(AccountTokenBox.Text, TransactionTypeBox.Text, ChargeTypeBox.Text, EntryModeBox.Text,
                                                        OrderIDBox.Text, AmountBox.Text, AccountTypeBox.Text, CustomParametersBox.Text, SubmitMethodBox.Text);
                        RenderBrowser(response);
                        break;

                    case "DEBIT": //ACH Debit(Sale)
                        OrderIDBox.Text = TransactionRequest.OrderIDRandom(8);
                        response = PaymentEngine.PerformACHDebitTransaction(AccountTokenBox.Text, TransactionTypeBox.Text, ChargeTypeBox.Text, EntryModeBox.Text,
                                                                        OrderIDBox.Text, AmountBox.Text, TCCBox.Text, CustomParametersBox.Text, SubmitMethodBox.Text);
                        RenderBrowser(response);
                        break;

                    case "VOID":  //Credit Void Transaction
                        response = PaymentEngine.PerformCreditVoidTransaction(AccountTokenBox.Text, TransactionTypeBox.Text, ChargeTypeBox.Text, OrderIDBox.Text);
                        RenderBrowser(response);
                        break;

                    case "FORCE_SALE": //Credit Force_Sale transaction (offline Capture)
                        OrderIDBox.Text = TransactionRequest.OrderIDRandom(8);
                        response = PaymentEngine.PerformCreditForceTransaction(AccountTokenBox.Text, TransactionTypeBox.Text, ChargeTypeBox.Text, EntryModeBox.Text, OrderIDBox.Text, AmountBox.Text, ApprovalCodeBox.Text, CustomParametersBox.Text, SubmitMethodBox.Text);
                        RenderBrowser(response);
                        break;

                    case "CAPTURE": //Credit Capture
                        response = PaymentEngine.PerformCreditCaptureTransaction(AccountTokenBox.Text, TransactionTypeBox.Text, ChargeTypeBox.Text, EntryModeBox.Text, OrderIDBox.Text, AmountBox.Text, CustomParametersBox.Text);
                        RenderBrowser(response);
                        break;

                    case "QUERY_PAYMENT": //Credit Query
                        response = PaymentEngine.PerformCreditQueryPaymentTransaction(AccountTokenBox.Text, TransactionTypeBox.Text, ChargeTypeBox.Text, EntryModeBox.Text, OrderIDBox.Text, CustomParametersBox.Text);
                        RenderBrowser(response);
                        break;

                    case "QUERY_PURCHASE": //Debit Query
                        response = PaymentEngine.PerformDebitQueryTransaction(AccountTokenBox.Text, TransactionTypeBox.Text, ChargeTypeBox.Text, EntryModeBox.Text, OrderIDBox.Text, "", CustomParametersBox.Text);
                        RenderBrowser(response);
                        break;

                    case "QUERY": //ACH Query
                        response = PaymentEngine.PerformACHQueryTransaction(AccountTokenBox.Text, TransactionTypeBox.Text, ChargeTypeBox.Text, EntryModeBox.Text, OrderIDBox.Text, "", CustomParametersBox.Text);
                        RenderBrowser(response);
                        break;

                    case "ADJUSTMENT": //Credit Adjustment
                        response = PaymentEngine.PerformCreditAdjustmentTransaction(AccountTokenBox.Text, TransactionTypeBox.Text, ChargeTypeBox.Text, EntryModeBox.Text, OrderIDBox.Text, AmountBox.Text, CustomParametersBox.Text);
                        RenderBrowser(response);
                        break;
                    case "CREDIT":  //Credit Transaction, Used in ACH, Credit, and Credit_Debit_Card
                        if (TransactionTypeBox.Text == "CREDIT_CARD" || TransactionTypeBox.Text == "CREDIT_DEBIT_CARD")
                        {
                            if (CreditTypeBox.SelectedItem.ToString() == "INDEPENDENT")
                            {
                                OrderIDBox.Text = TransactionRequest.OrderIDRandom(8);
                                response = PaymentEngine.PerformCreditIndependentCreditTransaction(AccountTokenBox.Text, TransactionTypeBox.Text, ChargeTypeBox.Text, EntryModeBox.Text, OrderIDBox.Text, AmountBox.Text, CustomParametersBox.Text, SubmitMethodBox.Text);
                                RenderBrowser(response);

                            }
                            if (CreditTypeBox.SelectedItem.ToString() == "DEPENDENT")
                            {
                                response = PaymentEngine.PerformCreditDependentCreditTransaction(AccountTokenBox.Text, TransactionTypeBox.Text, ChargeTypeBox.Text, EntryModeBox.Text, OrderIDBox.Text, AmountBox.Text, CustomParametersBox.Text);
                                RenderBrowser(response);
                            }
                        }
                        if (TransactionTypeBox.SelectedItem.ToString() == "ACH")
                        {
                            if (CreditTypeBox.SelectedItem.ToString() == "INDEPENDENT")
                            {
                                OrderIDBox.Text = TransactionRequest.OrderIDRandom(8);
                                response = PaymentEngine.PerformACHCreditTransaction(AccountTokenBox.Text, TransactionTypeBox.Text, ChargeTypeBox.Text, EntryModeBox.Text, OrderIDBox.Text, AmountBox.Text, TCCBox.Text, CustomParametersBox.Text, SubmitMethodBox.Text);
                                RenderBrowser(response);
                            }
                            if (CreditTypeBox.SelectedItem.ToString() == "DEPENDENT") //Doesn't actually work at this time through API, but logic should flow as follows.
                            {
                                response = PaymentEngine.PerformACHDependentCreditTransaction(AccountTokenBox.Text, TransactionTypeBox.Text, ChargeTypeBox.Text, EntryModeBox.Text, OrderIDBox.Text, AmountBox.Text, CustomParametersBox.Text, SubmitMethodBox.Text);
                                RenderBrowser(response);
                            }
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                // Legacy Implementation was here...
            }
        }// OneButton Submit Button Actions
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
                GeneralFunctions.WriteToLog(ex.ToString());
            }
        }//Converts Data in QueryPayment box to JSON for Readability
        private void buildPostButton_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                switch (ChargeTypeBox.Text)
                {
                    case "SALE":
                        OrderIDBox.Text = TransactionRequest.OrderIDRandom(8);
                        PostParameterBox.Text = TransactionRequest.CreditCardParamBuilder(AccountTokenBox.Text, TransactionTypeBox.Text, ChargeTypeBox.Text, EntryModeBox.Text, OrderIDBox.Text, AmountBox.Text, CustomParametersBox.Text);
                        GeneralFunctions.WriteToLog(PostParameterBox.Text);
                        break;

                    case "AUTH":
                        OrderIDBox.Text = TransactionRequest.OrderIDRandom(8);
                        PostParameterBox.Text = TransactionRequest.CreditCardParamBuilder(AccountTokenBox.Text, TransactionTypeBox.Text, ChargeTypeBox.Text, EntryModeBox.Text, OrderIDBox.Text, AmountBox.Text, CustomParametersBox.Text);
                        GeneralFunctions.WriteToLog(PostParameterBox.Text);
                        break;

                    //need to Implement Signature IMage handling
                    case "SIGNATURE":
                        OrderIDBox.Text = TransactionRequest.OrderIDRandom(8);
                        PostParameterBox.Text = TransactionRequest.CreditCardParamBuilder(AccountTokenBox.Text, TransactionTypeBox.Text, ChargeTypeBox.Text, EntryModeBox.Text, OrderIDBox.Text, AmountBox.Text, CustomParametersBox.Text);
                        GeneralFunctions.WriteToLog(PostParameterBox.Text);
                        break;

                    case "PURCHASE":
                        OrderIDBox.Text = TransactionRequest.OrderIDRandom(8);
                        PostParameterBox.Text = TransactionRequest.DebitCardParamBuilder(AccountTokenBox.Text, TransactionTypeBox.Text, ChargeTypeBox.Text, EntryModeBox.Text,
                                                        OrderIDBox.Text, AmountBox.Text, AccountTypeBox.Text, CustomParametersBox.Text);
                        GeneralFunctions.WriteToLog(PostParameterBox.Text);
                        break;

                    case "REFUND":
                        OrderIDBox.Text = TransactionRequest.OrderIDRandom(8);
                        PostParameterBox.Text = TransactionRequest.DebitCardParamBuilder(AccountTokenBox.Text, TransactionTypeBox.Text, ChargeTypeBox.Text, EntryModeBox.Text,
                                                        OrderIDBox.Text, AmountBox.Text, AccountTypeBox.Text, CustomParametersBox.Text);
                        GeneralFunctions.WriteToLog(PostParameterBox.Text);
                        break;

                    case "DEBIT":
                        OrderIDBox.Text = TransactionRequest.OrderIDRandom(8);
                        PostParameterBox.Text = TransactionRequest.ACHParamBuilder(AccountTokenBox.Text, TransactionTypeBox.Text, ChargeTypeBox.Text, EntryModeBox.Text,
                                                                        OrderIDBox.Text, AmountBox.Text, TCCBox.Text, CustomParametersBox.Text);
                        GeneralFunctions.WriteToLog(PostParameterBox.Text);
                        break;

                    case "VOID":
                        PostParameterBox.Text = TransactionRequest.DirectPostBuilder(AccountTokenBox.Text, OrderIDBox.Text, TransactionTypeBox.Text, ChargeTypeBox.Text);
                        GeneralFunctions.WriteToLog(PostParameterBox.Text);
                        break;

                    case "FORCE_SALE":
                        OrderIDBox.Text = TransactionRequest.OrderIDRandom(8);
                        PostParameterBox.Text = TransactionRequest.CreditForceParamBuilder(AccountTokenBox.Text, TransactionTypeBox.Text, ChargeTypeBox.Text,
                                                                                EntryModeBox.Text, OrderIDBox.Text, AmountBox.Text, ApprovalCodeBox.Text, CustomParametersBox.Text);
                        GeneralFunctions.WriteToLog(PostParameterBox.Text);
                        break;

                    case "CAPTURE":
                        PostParameterBox.Text = TransactionRequest.CreditCardParamBuilder(AccountTokenBox.Text, TransactionTypeBox.Text, ChargeTypeBox.Text, EntryModeBox.Text, OrderIDBox.Text, AmountBox.Text, CustomParametersBox.Text);
                        GeneralFunctions.WriteToLog(PostParameterBox.Text);
                        break;
                    //Query Logic
                    case "QUERY_PAYMENT":
                        PostParameterBox.Text = TransactionRequest.CreditCardParamBuilder(AccountTokenBox.Text, TransactionTypeBox.Text, ChargeTypeBox.Text, EntryModeBox.Text, OrderIDBox.Text, AmountBox.Text, CustomParametersBox.Text);
                        GeneralFunctions.WriteToLog(PostParameterBox.Text);
                        break;

                    case "QUERY_PURCHASE":
                        PostParameterBox.Text = TransactionRequest.CreditCardParamBuilder(AccountTokenBox.Text, TransactionTypeBox.Text, ChargeTypeBox.Text, EntryModeBox.Text, OrderIDBox.Text, AmountBox.Text, CustomParametersBox.Text);
                        GeneralFunctions.WriteToLog(PostParameterBox.Text);
                        break;

                    case "QUERY":
                        PostParameterBox.Text = TransactionRequest.CreditCardParamBuilder(AccountTokenBox.Text, TransactionTypeBox.Text, ChargeTypeBox.Text, EntryModeBox.Text, OrderIDBox.Text, AmountBox.Text, CustomParametersBox.Text);
                        GeneralFunctions.WriteToLog(PostParameterBox.Text);
                        break;

                    case "ADJUSTMENT":
                        PostParameterBox.Text = TransactionRequest.CreditCardParamBuilder(AccountTokenBox.Text, TransactionTypeBox.Text, ChargeTypeBox.Text, EntryModeBox.Text, OrderIDBox.Text, AmountBox.Text, CustomParametersBox.Text);
                        GeneralFunctions.WriteToLog(PostParameterBox.Text);
                        break;

                    //May need to Redo ACH Dependent Return
                    case "CREDIT":
                        if (TransactionTypeBox.SelectedItem.ToString() == "CREDIT_CARD" || TransactionTypeBox.SelectedItem.ToString() == "CREDIT_DEBIT_CARD")
                        {
                            if (CreditTypeBox.SelectedItem.ToString() == "INDEPENDENT")
                            {
                                OrderIDBox.Text = TransactionRequest.OrderIDRandom(8);
                                PostParameterBox.Text = TransactionRequest.CreditCardParamBuilder(AccountTokenBox.Text, TransactionTypeBox.Text, ChargeTypeBox.Text,
                                                                                        EntryModeBox.Text, OrderIDBox.Text, AmountBox.Text, CustomParametersBox.Text);
                            }
                            if (CreditTypeBox.SelectedItem.ToString() == "DEPENDENT")
                            {
                                PostParameterBox.Text = TransactionRequest.CreditCardParamBuilder(AccountTokenBox.Text, TransactionTypeBox.Text, ChargeTypeBox.Text,
                                                                                        EntryModeBox.Text, OrderIDBox.Text, AmountBox.Text, CustomParametersBox.Text);
                                GeneralFunctions.WriteToLog(PostParameterBox.Text);
                            }
                        }
                        if (TransactionTypeBox.SelectedItem.ToString() == "ACH")
                        {
                            if (CreditTypeBox.SelectedItem.ToString() == "INDEPENDENT")
                            {
                                OrderIDBox.Text = TransactionRequest.OrderIDRandom(8);
                                PostParameterBox.Text = TransactionRequest.ACHParamBuilder(AccountTokenBox.Text, TransactionTypeBox.Text, ChargeTypeBox.Text, EntryModeBox.Text,
                                                                                    OrderIDBox.Text, AmountBox.Text, TCCBox.SelectedItem.ToString(), CustomParametersBox.Text);
                            }
                            if (CreditTypeBox.SelectedItem.ToString() == "DEPENDENT")
                            {
                                PostParameterBox.Text = TransactionRequest.ACHParamBuilder(AccountTokenBox.Text, TransactionTypeBox.Text, ChargeTypeBox.Text, EntryModeBox.Text,
                                                                                    OrderIDBox.Text, AmountBox.Text, TCCBox.SelectedItem.ToString(), CustomParametersBox.Text);
                                GeneralFunctions.WriteToLog(PostParameterBox.Text);
                            }
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                OEHPWebBrowser.NavigateToString("An Exception Occured.");
                GeneralFunctions.WriteToLog(ex.ToString());
            }
        }//Builds a Transaction Post in Build-then-Submit mode
        private void submitPostButton_Click(object sender, RoutedEventArgs e) //Submits The data in PostParameters to the Gateway 
        {
            ResponseForWebBrowser response = new ResponseForWebBrowser();
            switch (((ComboBoxItem)BuildSubmitMethodSelectorComboBox.SelectedItem).Name)
            {
                case "PayPagePost":
                    try
                    {
                        response = PaymentEngine.SendToGateway(PostParameterBox.Text, SubmitMethodBox.Text);
                        RenderBrowser(response);
                    }
                    catch
                    {
                        ///Hm
                    }
                    break;
                case "DirectPost":
                    response = PaymentEngine.SendToGateway(PostParameterBox.Text, "directPost");
                    OEHPWebBrowser.NavigateToString(response.directPostResponse);
                    break;
                default:
                    break;
            }
        }
        #endregion
        #region UI - Menubar Interaction
        private void GetRCMPortRCM_Click(object sender, RoutedEventArgs e) //NonFunctional At this time, may be abel to be called with a later Enhancement to API
        {
            string rcmParameters = "https://localsystem.paygateway.com:21113/RcmService.svc/Initialize?callback=jsonpResponse&xl2Parameters=/TRANSACTIONTYPE:GETCURRENTUSERPORT";
            MessageBox.Show("This Won't work :(", "Error");
        }
        private void ResetButton_Click(object sender, RoutedEventArgs e) //Resets AccountToken to HardCoded default.
        {
            Globals.Default.AccountToken = Globals.Default.DefaultAccountToken;
            Globals.Default.Save();
            AccountTokenBox.Text = Globals.Default.AccountToken;
        }
        private void ExitMenuItem_Click(object sender, RoutedEventArgs e) //Closes Program
        {
            this.Close();
        }
        private void PresetEMVTesting_Click(object sender, RoutedEventArgs e) //Sets AccountToken to a working EMV Test Account Token
        {
            AccountTokenBox.Text = Globals.Default.DefaultAccountToken;
        }
        private void PresetCanadianTesting_Click(object sender, RoutedEventArgs e) // Sets AccountToken to working Canadian Test Account Token
        {
            AccountTokenBox.Text = Globals.Default.CanadianAccountToken;
        }
        private void PresetLoopBackTesting_Click(object sender, RoutedEventArgs e) //Sets AccountToken to LoopBack account Token for Failure Testing.
        {
            AccountTokenBox.Text = Globals.Default.LoopBackAccountToken;
        }
        private void PresetHelp_Click(object sender, RoutedEventArgs e) //Opens Menu That describes the various Preset Functions
        {
            GeneralFunctions.PresetHelpWindowLauncher();
        }
        private void QueryResponseQueryString_Click(object sender, RoutedEventArgs e) //Sets the QueryResponse Mode
        {
            Globals.Default.QueryResponseMode = "Querystring";
            if (QueryResponseJSON.IsChecked == true)
            {
                QueryResponseJSON.IsChecked = false;
            }
            QueryResponseQueryString.IsChecked = true;
            Globals.Default.Save();
        }
        private void QueryResponseJSON_Click(object sender, RoutedEventArgs e) //Sets the QueryResponse mode to Json
        {
            Globals.Default.QueryResponseMode = "JSON";
            if (QueryResponseQueryString.IsChecked == true)
            {
                QueryResponseQueryString.IsChecked = false;
            }
            QueryResponseJSON.IsChecked = true;
            Globals.Default.Save();
        }
        private void ProcessingModeTest_Click(object sender, RoutedEventArgs e) //Sets Processing Environment to Test
        {
            Globals.Default.ProcessingMode = "Test";
            if (ProcessingModeLive.IsChecked == true)
            {
                ProcessingModeLive.IsChecked = false;
            }
            ProcessingModeTest.IsChecked = true;
        }
        private void ProcessingModeLive_Click(object sender, RoutedEventArgs e)// Sets Processing Environment to Live, and Checks for UN/PW for live processing.
        {
            LoginForLive login = new LoginForLive();
            if (login.ShowDialog().Value == false)
            {
                bool CorrectLogin = login.CorrectLogin;
                if (CorrectLogin == true)
                {
                    Globals.Default.ProcessingMode = "Live";
                    if (ProcessingModeTest.IsChecked == true)
                    {
                        ProcessingModeTest.IsChecked = false;
                    }
                    ProcessingModeLive.IsChecked = true;
                }
                else
                {
                    MessageBox.Show("Live processing restricted to OpenEdge. Please contact your OpenEdge Representative.");
                }
            }
        }
        private void PayPageFields_Click(object sender, RoutedEventArgs e) //Opens Window for customization of Paypage Fields, Needs Improvement and Revamp.
        {
            PayPageCustomization ppf = new PayPageCustomization();
            ppf.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            ppf.ShowDialog();

        }
        private void PayPageBranding_Click(object sender, RoutedEventArgs e) //Opens Window for customization of Paypage Fields, Needs Improvement and Revamp
        {
            PayPageBranding ppb = new PayPageBranding();
            ppb.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            ppb.ShowDialog();
        }
        private void MPDTransactions_Click(object sender, RoutedEventArgs e) //Opens window for MPD Transactions
        {
            MPDTransactions mpd = new MPDTransactions();
            mpd.ShowDialog();
        }
        private void About_Click(object sender, RoutedEventArgs e) //Opens About WIndow
        {
            GeneralFunctions.AboutWindowLauncher();
        }
        private void TipReceipt_Click(object sender, RoutedEventArgs e) //Generates a Receipt with TIP Line
        {
            OEHP.NET.DataManipulation dm = new OEHP.NET.DataManipulation();
            ReceiptFormatter rf = new ReceiptFormatter();
            if (Globals.Default.QueryResponseMode == "JSON")
            {
                var RDO = dm.ReceiptDataObject(QueryPaymentBox.Text);
                string receiptData = rf.TipReceipt(RDO);
                Receipt jr = new Receipt(receiptData);
                jr.ShowDialog();

            }
            else
            {
                string json = dm.QueryStringToJson(QueryPaymentBox.Text);
                var RDO = dm.ReceiptDataObject(json);
                string receiptData = rf.TipReceipt(RDO);
                Receipt qr = new Receipt(receiptData);
                qr.ShowDialog();
            }
        }
        private void StandardReceipt_Click(object sender, RoutedEventArgs e) //Generates a standard Receipt
        {
            OEHP.NET.DataManipulation dm = new OEHP.NET.DataManipulation();
            ReceiptFormatter rf = new ReceiptFormatter();
            if (Globals.Default.QueryResponseMode == "JSON")
            {
                var RDO = dm.ReceiptDataObject(QueryPaymentBox.Text);
                string receiptData = rf.GenericReceipt(RDO);
                Receipt r = new Receipt(receiptData);
                r.ShowDialog();
            }
            else
            {
                string json = dm.QueryStringToJson(QueryPaymentBox.Text);
                var RDO = dm.ReceiptDataObject(json);
                string receiptData = rf.GenericReceipt(RDO);
                Receipt r = new Receipt(receiptData);
                r.ShowDialog();
            }
        }
        private void SaveToken_Click(object sender, RoutedEventArgs e) //Saves Current Accoutn Token
        {
            Globals.Default.AccountToken = AccountTokenBox.Text;
            Globals.Default.Save();

        }
        private void DupCheckOn_Click(object sender, RoutedEventArgs e) //Sets Duplicate Checkign mode ON
        {
            GeneralFunctions.SetDupModeOn();
        }
        private void DupCheckOff_Click(object sender, RoutedEventArgs e) //Sets Duplicate Checking mode OFF
        {
            GeneralFunctions.SetDupModeOff();
        }
        private void CreateNewDB_Click(object sender, RoutedEventArgs e) //Prompts to Create new database
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
        private bool IsThereADB() //Checks if a Database file currently exists
        {
            if (System.IO.File.Exists(AppDomain.CurrentDomain.BaseDirectory + "tran.oehp") == true)
            {

                return true;
            }
            else
            {
                return false;
            }
        }
        private void GoToPortal_Click(object sender, RoutedEventArgs e) //opens the OpenEdgeDeveloper Portal
        {
            GeneralFunctions.NavToDevPortal();
        }
        private void EmailDevServices_Click(object sender, RoutedEventArgs e) //opens E-mail Client to Email DevServices
        {
            GeneralFunctions.EmailDevServices();
        }
        private void GetRCMPortXL2_Click(object sender, RoutedEventArgs e) //Calls EdgeExpress to get the CUrrentusers RCM Port
        {

            string parameters = "<XLINKEMVREQUEST><TRANSACTIONTYPE>GETRCMCURRENTUSERPORT</TRANSACTIONTYPE></XLINKEMVREQUEST>";
            string response;
            XCharge.XpressLink2.XLEmv xl2 = new XCharge.XpressLink2.XLEmv();
            xl2.Execute(parameters, out response);

            MessageBox.Show(response, "Current User RCM Port");
        }
        private void PPDApplyDeviceConfiguration_Click(object sender, RoutedEventArgs e) //opens window for Configration of PPD through XL2/EdgeExpress calls
        {
            PPDApplyDeviceConfiguration ppd = new OEHP_Tester.PPDApplyDeviceConfiguration();
            ppd.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            ppd.ShowDialog();
        }
        private void OneButton_Click(object sender, RoutedEventArgs e) //Sets Current Submit Method to the OneButton method
        {

            Globals.Default.SubmitMode = "OneButton";
            Globals.Default.Save();

            SubmitModeConfigurationUIElements();
        }
        private void BuildThenSubmit_Click(object sender, RoutedEventArgs e)  //Sets current Submit Method tot he BuildThenSubmit method
        {
            Globals.Default.SubmitMode = "BuildThenSubmit";
            Globals.Default.Save();

            SubmitModeConfigurationUIElements();
        }
        #endregion
        #region RCM Status Calls
        private void getRCMStatusWithSessionToken(string mode) //Calls the HTTPS GET to get the current RCM Status
        {
            if (mode == "Test")
            {
                WebRequest wr = WebRequest.Create(OEHP.NET.VariableHandler.TestRcmStatusURL + Globals.Default.SessionToken);
                wr.Method = "GET";

                Stream objStream;
                objStream = wr.GetResponse().GetResponseStream();

                StreamReader sr = new StreamReader(objStream);

                Globals.Default.RCMStatus = sr.ReadToEnd();
                //RCMStatusBox.Text = rcmStatus;
            }
            if (mode == "Live")
            {
                WebRequest wr = WebRequest.Create(OEHP.NET.VariableHandler.LiveRcmStatusURL + Globals.Default.SessionToken);
                wr.Method = "GET";

                Stream objStream;
                objStream = wr.GetResponse().GetResponseStream();

                StreamReader sr = new StreamReader(objStream);

                Globals.Default.RCMStatus = sr.ReadToEnd();
                //RCMStatusBox.Text = rcmStatus;
            }
        }
        public void RCMStatus() //General RCM Status Method that determines whether to use HTTPS GET or to scrape the HTML for the current RCM Status
        {
            try
            {
                if (SubmitMethodBox.Text == "PayPage Post")
                {
                    getRCMStatusWithSessionToken(Globals.Default.ProcessingMode);
                }
                else
                {
                    string pageHTML = GeneralFunctions.GetPageContent(OEHPWebBrowser);
                    Globals.Default.RCMStatus = GeneralFunctions.RCMStatusFromWebPage(pageHTML);
                }
            }
            catch (Exception ex)
            {
                GeneralFunctions.WriteToLog(ex.ToString());
            }
        }
        #endregion
        #region Methods used for and during Query logic.
        private void sendQuery() //Sends Query request to the gateway
        {
            ResponseForWebBrowser response = new ResponseForWebBrowser();
            DataManipulation dm = new DataManipulation();
            response = PaymentEngine.PerformCreditQueryPaymentTransaction(AccountTokenBox.Text, TransactionTypeBox.Text, "QUERY", EntryModeBox.Text, OrderIDBox.Text, "");
            QueryPaymentBox.Text = response.directPostResponse;
            Globals.Default.QueryResponse = response.directPostResponse;
            if (Globals.Default.QueryResponseMode == "JSON")
            {
                Globals.Default.QueryResponse = dm.QueryStringToJson(Globals.Default.QueryResponse);
            }
        }
        private void parseQueryResultAndSaveToDB() // Pares what object to serialize the query_result data to, then inserts it into the SQLite database
        {
            DataManipulation dm = new DataManipulation();
            char firstChar = QueryPaymentBox.Text[0];
            if (firstChar == 'r')
            {

                string result = dm.QueryStringToJson(Globals.Default.QueryResponse);
                var qro = dm.QueryResultObject(result);
                object[] args = new object[] { qro.response_code, qro.response_code_text, qro.secondary_response_code, qro.secondary_response_code_text, qro.time_stamp, qro.retry_recommended, qro.authorized_amount, qro.bin, qro.captured_amount, qro.original_authorized_amount, qro.requested_amount, qro.time_stamp_created, qro.original_response_code, qro.original_response_code_text, qro.time_stamp_updated, qro.state, qro.bank_approval_code, qro.expire_month, qro.expire_year, qro.order_id, qro.payer_identifier, qro.reference_id, qro.span, qro.card_brand, qro.batch_id, qro.receipt_application_cryptogram, qro.receipt_application_identifier, qro.receipt_application_preferred_name, qro.receipt_application_transaction_counter, qro.receipt_approval_code, qro.receipt_authorization_response_code, qro.receipt_card_number, qro.receipt_card_type, qro.receipt_entry_legend, qro.receipt_entry_method, qro.receipt_line_items, qro.receipt_merchant_id, qro.receipt_order_id, qro.receipt_signature_text, qro.receipt_terminal_verification_results, qro.receipt_transaction_date_time, qro.receipt_transaction_id, qro.receipt_transaction_reference_number, qro.receipt_transaction_type, qro.receipt_validation_code, qro.receipt_verbiage };
                string sqlInsert = string.Format("INSERT INTO TRANSACTIONDB (response_code, response_code_text, secondary_response_code, secondary_response_code_text, time_stamp, retry_recommended, authorized_amount, bin, captured_amount, original_authorized_amount, requested_amount, time_stamp_created, original_response_code, original_response_code_text, time_stamp_updated, state, bank_approval_code, expire_month, expire_year, order_id, payer_identifier, reference_id, span, card_brand, batch_id, receipt_application_cryptogram, receipt_application_identifier, receipt_application_preferred_name, receipt_application_transaction_counter, receipt_approval_code, receipt_authorization_reseponse_code, receipt_card_number, receipt_card_type, receipt_entry_legend, receipt_entry_method, receipt_line_items, receipt_merchant_id, receipt_order_id, receipt_signature_text, receipt_terminal_verificiation_results, receipt_transaction_date_time, receipt_transaction_id, receipt_transaction_reference_number, receipt_transaction_type, receipt_validation_code, receipt_verbiage) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}', '{13}', '{14}', '{15}', '{16}', '{17}', '{18}', '{19}', '{20}', '{21}', '{22}', '{23}', '{24}', '{25}', '{26}', '{27}', '{28}', '{29}', '{30}', '{31}', '{32}', '{33}', '{34}', '{35}', '{36}', '{37}', '{38}', '{39}', '{40}', '{41}', '{42}', '{43}', '{44}', '{45}')", args);
                GeneralFunctions.InsertTransactionData(sqlInsert);

            }
            if (firstChar == '{')
            {
                var qro = dm.QueryResultObject(Globals.Default.QueryResponse);
                object[] args = new object[] { qro.response_code, qro.response_code_text, qro.secondary_response_code, qro.secondary_response_code_text, qro.time_stamp, qro.retry_recommended, qro.authorized_amount, qro.bin, qro.captured_amount, qro.original_authorized_amount, qro.requested_amount, qro.time_stamp_created, qro.original_response_code, qro.original_response_code_text, qro.time_stamp_updated, qro.state, qro.bank_approval_code, qro.expire_month, qro.expire_year, qro.order_id, qro.payer_identifier, qro.reference_id, qro.span, qro.card_brand, qro.batch_id, qro.receipt_application_cryptogram, qro.receipt_application_identifier, qro.receipt_application_preferred_name, qro.receipt_application_transaction_counter, qro.receipt_approval_code, qro.receipt_authorization_response_code, qro.receipt_card_number, qro.receipt_card_type, qro.receipt_entry_legend, qro.receipt_entry_method, qro.receipt_line_items, qro.receipt_merchant_id, qro.receipt_order_id, qro.receipt_signature_text, qro.receipt_terminal_verification_results, qro.receipt_transaction_date_time, qro.receipt_transaction_id, qro.receipt_transaction_reference_number, qro.receipt_transaction_type, qro.receipt_validation_code, qro.receipt_verbiage };
                string sqlInsert = string.Format("INSERT INTO TRANSACTIONDB (response_code, response_code_text, secondary_response_code, secondary_response_code_text, time_stamp, retry_recommended, authorized_amount, bin, captured_amount, original_authorized_amount, requested_amount, time_stamp_created, original_response_code, original_response_code_text, time_stamp_updated, state, bank_approval_code, expire_month, expire_year, order_id, payer_identifier, reference_id, span, card_brand, batch_id, receipt_application_cryptogram, receipt_application_identifier, receipt_application_preferred_name, receipt_application_transaction_counter, receipt_approval_code, receipt_authorization_reseponse_code, receipt_card_number, receipt_card_type, receipt_entry_legend, receipt_entry_method, receipt_line_items, receipt_merchant_id, receipt_order_id, receipt_signature_text, receipt_terminal_verificiation_results, receipt_transaction_date_time, receipt_transaction_id, receipt_transaction_reference_number, receipt_transaction_type, receipt_validation_code, receipt_verbiage) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}', '{13}', '{14}', '{15}', '{16}', '{17}', '{18}', '{19}', '{20}', '{21}', '{22}', '{23}', '{24}', '{25}', '{26}', '{27}', '{28}', '{29}', '{30}', '{31}', '{32}', '{33}', '{34}', '{35}', '{36}', '{37}', '{38}', '{39}', '{40}', '{41}', '{42}', '{43}', '{44}', '{45}')", args);
                GeneralFunctions.InsertTransactionData(sqlInsert);
            }
            else
            {
                //Do Nothing
            }
        }
        private void parseSignatureStringThenDecode() //PArses Signature from Scraped HTML
        {
            try
            {
                HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(GeneralFunctions.GetPageContent(OEHPWebBrowser));

                string value = doc.DocumentNode.SelectSingleNode("//input[@type='hidden' and @id='signatureImage' and @name='signatureImage']").Attributes["value"].Value;

                if (null != value)
                {
                    SignatureImage.Source = GeneralFunctions.DecodeBase64Image(value);
                }
                else
                {
                    GeneralFunctions.WriteToLog("No Signature Image to Display");
                }

            }
            catch (Exception ex)
            {
                GeneralFunctions.WriteToLog(ex.ToString());
            }
        }
        #endregion  
        public MainWindow()
        {
            InitializeComponent();
            bool DBExists = IsThereADB();
            if (DBExists == false)
            {
                GeneralFunctions gf = new GeneralFunctions();
                gf.CreateDBFile();
            }
            SubmitModeConfigurationUIElements();
            SetQueryResponseModeUIElements();
            SetProcessingModeUIElements();
            SetCommonComboBoxCollections();
        }
        private void CustomParametersBox_TextChanged(object sender, TextChangedEventArgs e) //Sets CustomParameters Variable whenever text is changed
        {
            Globals.Default.CustomParameters = CustomParametersBox.Text;
        }
        private void OEHPWebBrowser_LoadCompleted(object sender, NavigationEventArgs e) // Controls Firing off a Query to the gateway and settign the query Parameters in UI
        {
            RCMStatus();
            string paymentFinishedSignal = GeneralFunctions.PaymentFinishedSignal(GeneralFunctions.GetPageContent(OEHPWebBrowser));
            if (ChargeTypeBox.Text == "VOID" || ChargeTypeBox.Text == "CAPTURE" || ChargeTypeBox.Text == "AUTH" || ChargeTypeBox.Text == "ADJUSTMENT")
            {
                sendQuery();
            }
            if (ChargeTypeBox.Text == "CREDIT" && CreditTypeBox.Text == "DEPENDENT")
            {
                sendQuery();
            }
            if (paymentFinishedSignal == "done")  //Based on Transaction Type logic flows for QUERY may change, so broken up into a Switch/Case for more modularity
            {
                switch (TransactionTypeBox.SelectedItem.ToString())
                {
                    case "CREDIT_CARD":
                        Globals.Default.QueryParameters = TransactionRequest.DirectPostBuilder(AccountTokenBox.Text, OrderIDBox.Text, TransactionTypeBox.SelectedItem.ToString(), "QUERY_PAYMENT");
                        GeneralFunctions.WriteToLog(Globals.Default.QueryParameters);
                        sendQuery();
                        parseQueryResultAndSaveToDB();
                        parseSignatureStringThenDecode();
                        
                        break;

                    case "CREDIT_DEBIT_CARD":
                        Globals.Default.QueryParameters = TransactionRequest.DirectPostBuilder(AccountTokenBox.Text, OrderIDBox.Text, TransactionTypeBox.SelectedItem.ToString(), "QUERY_PAYMENT");
                        GeneralFunctions.WriteToLog(Globals.Default.QueryParameters);
                        sendQuery();
                        parseQueryResultAndSaveToDB();
                        parseSignatureStringThenDecode();
                        break;

                    case "DEBIT_CARD":
                        Globals.Default.QueryParameters = TransactionRequest.DirectPostBuilder(AccountTokenBox.Text, OrderIDBox.Text, TransactionTypeBox.SelectedItem.ToString(), "QUERY_PURCHASE");
                        GeneralFunctions.WriteToLog(Globals.Default.QueryParameters);
                        sendQuery();
                        parseQueryResultAndSaveToDB();
                        break;

                    case "ACH":
                        Globals.Default.QueryParameters = TransactionRequest.DirectPostBuilder(AccountTokenBox.Text, OrderIDBox.Text, TransactionTypeBox.SelectedItem.ToString(), "QUERY");
                        GeneralFunctions.WriteToLog(Globals.Default.QueryParameters);
                        sendQuery();
                        parseQueryResultAndSaveToDB();
                        break;

                    case "INTERAC":
                        Globals.Default.QueryParameters = TransactionRequest.DirectPostBuilder(AccountTokenBox.Text, OrderIDBox.Text, TransactionTypeBox.SelectedItem.ToString(), "QUERY_PURCHASE");
                        GeneralFunctions.WriteToLog(Globals.Default.QueryParameters);
                        sendQuery();
                        parseQueryResultAndSaveToDB();
                        break;

                    default:
                        GeneralFunctions.WriteToLog("Improper Selection for Query Payment");
                        break;
                }
            }
        } 
    }
    
}

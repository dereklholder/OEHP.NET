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
using System.Collections.Specialized;
using System.Web;
using System.Net;

namespace OEHP_Tester
{
    /// <summary>
    /// Interaction logic for PayPageCustomization.xaml
    /// </summary>
    public partial class PayPageCustomization : Window
    {
        public PayPageCustomization()
        {
            InitializeComponent();

            ParseQueryStringFromCustomParameters();
        }
        public void ParseQueryStringFromCustomParameters()
        {
            //TO IMPLENT: Boolean Binding of KeyPairs and IsChecked events.
            string queryString = Globals.Default.CustomParameters;
            NameValueCollection keyPairs = HttpUtility.ParseQueryString(queryString);
                   
            PayPageCosmetic.Default.bill_customer_title_label = keyPairs.Get("bill_customer_title_label");
            PayPageCosmetic.Default.bill_customer_title_visible = Convert.ToBoolean(keyPairs.Get("bill_customer_title_visible"));
            PayPageCosmetic.Default.bill_customer_title_edit = Convert.ToBoolean(keyPairs.Get("bill_customter_title_edit"));
            PayPageCosmetic.Default.bill_customer_title_mandatory = Convert.ToBoolean(keyPairs.Get("bill_customer_title_mandatory"));

            PayPageCosmetic.Default.bill_first_name_label = keyPairs.Get("bill_first_name_label");
            PayPageCosmetic.Default.bill_first_name_visible = Convert.ToBoolean(keyPairs.Get("bill_first_name_visible"));
            PayPageCosmetic.Default.bill_first_name_edit = Convert.ToBoolean(keyPairs.Get("bill_first_name_edit"));
            PayPageCosmetic.Default.bill_first_name_mandatory = Convert.ToBoolean(keyPairs.Get("bill_first_name_mandatory"));

            PayPageCosmetic.Default.bill_company_label = keyPairs.Get("bill_company_label");
            PayPageCosmetic.Default.bill_company_visible = Convert.ToBoolean(keyPairs.Get("bill_company_visible"));
            PayPageCosmetic.Default.bill_company_edit = Convert.ToBoolean(keyPairs.Get("bill_company_edit"));
            PayPageCosmetic.Default.bill_company_mandatory = Convert.ToBoolean(keyPairs.Get("bill_company_mandatory"));

            PayPageCosmetic.Default.bill_middle_name_label = keyPairs.Get("bill_middle_name_label");
            PayPageCosmetic.Default.bill_middle_name_visible = Convert.ToBoolean(keyPairs.Get("bill_middle_name_visible"));
            PayPageCosmetic.Default.bill_middle_name_edit = Convert.ToBoolean(keyPairs.Get("bill_middle_name_edit"));
            PayPageCosmetic.Default.bill_middle_name_mandatory = Convert.ToBoolean(keyPairs.Get("bill_middle_name_mandatory"));

            PayPageCosmetic.Default.bill_last_name_label = keyPairs.Get("bill_last_name_label");
            PayPageCosmetic.Default.bill_last_name_visible = Convert.ToBoolean(keyPairs.Get("bill_last_name_label"));
            PayPageCosmetic.Default.bill_last_name_edit = Convert.ToBoolean(keyPairs.Get("bill_last_name_edit"));
            PayPageCosmetic.Default.bill_last_name_mandatory = Convert.ToBoolean(keyPairs.Get("bill_last_name_edit"));

            PayPageCosmetic.Default.bill_address_one_label = keyPairs.Get("bill_address_one_label");
            PayPageCosmetic.Default.bill_address_one_visible = Convert.ToBoolean(keyPairs.Get("bill_address_one_visible"));
            PayPageCosmetic.Default.bill_address_one_edit = Convert.ToBoolean(keyPairs.Get("bill_address_one_edit"));
            PayPageCosmetic.Default.bill_address_one_mandatory = Convert.ToBoolean(keyPairs.Get("bill_address_one_mandatory"));

            PayPageCosmetic.Default.bill_address_two_label = keyPairs.Get("bill_address_two_label");
            PayPageCosmetic.Default.bill_address_two_visible = Convert.ToBoolean(keyPairs.Get("bill_address_two_visible"));
            PayPageCosmetic.Default.bill_address_two_edit = Convert.ToBoolean(keyPairs.Get("bill_address_two_edit"));
            PayPageCosmetic.Default.bill_address_two_mandatory = Convert.ToBoolean(keyPairs.Get("bill_address_two_mandatory"));

            PayPageCosmetic.Default.bill_city_label = keyPairs.Get("bill_city_label");
            PayPageCosmetic.Default.bill_city_visible = Convert.ToBoolean(keyPairs.Get("bill_city_visible"));
            PayPageCosmetic.Default.bill_city_edit = Convert.ToBoolean(keyPairs.Get("bill_city_edit"));
            PayPageCosmetic.Default.bill_city_mandatory = Convert.ToBoolean(keyPairs.Get("bil_city_mandatory"));

            PayPageCosmetic.Default.bill_state_or_province_label = keyPairs.Get("bill_state_or_province_label");
            PayPageCosmetic.Default.bill_state_or_province_visible = Convert.ToBoolean(keyPairs.Get("bill_state_or_province_visible"));
            PayPageCosmetic.Default.bill_state_or_province_edit = Convert.ToBoolean(keyPairs.Get("bill_state_or_province_edit"));
            PayPageCosmetic.Default.bill_state_or_province_mandatory = Convert.ToBoolean(keyPairs.Get("bill_state_or_province_mandatory"));

            PayPageCosmetic.Default.bill_country_code_label = keyPairs.Get("bill_country_code_label");
            PayPageCosmetic.Default.bill_country_code_visible = Convert.ToBoolean(keyPairs.Get("bill_country_code_visible"));
            PayPageCosmetic.Default.bill_country_code_edit = Convert.ToBoolean(keyPairs.Get("bill_country_code_edit"));
            PayPageCosmetic.Default.bill_country_code_mandatory = Convert.ToBoolean(keyPairs.Get("bill_country_code_mandatory"));

            PayPageCosmetic.Default.bill_postal_code_label = keyPairs.Get("bill_postal_code_label");
            PayPageCosmetic.Default.bill_postal_code_visible = Convert.ToBoolean(keyPairs.Get("bill_postal_code_visible"));
            PayPageCosmetic.Default.bill_postal_code_edit = Convert.ToBoolean(keyPairs.Get("bill_postal_code_edit"));
            PayPageCosmetic.Default.bill_postal_code_mandatory = Convert.ToBoolean(keyPairs.Get("bill_postal_code_mandatory"));

            PayPageCosmetic.Default.invoice_number_label = keyPairs.Get("invoice_number_label");
            PayPageCosmetic.Default.invoice_number_visible = Convert.ToBoolean(keyPairs.Get("invoice_number_visible"));
            PayPageCosmetic.Default.invoice_number_edit = Convert.ToBoolean(keyPairs.Get("invoice_number_edit"));
            PayPageCosmetic.Default.invoice_number_mandatory = Convert.ToBoolean(keyPairs.Get("invoice_number_mandatory"));

            PayPageCosmetic.Default.purchase_order_number_label = keyPairs.Get("purchase_order_number_label");
            PayPageCosmetic.Default.purchase_order_number_visible = Convert.ToBoolean(keyPairs.Get("purchase_order_number_visible"));
            PayPageCosmetic.Default.purchase_order_number_edit = Convert.ToBoolean(keyPairs.Get("purchase_order_number_edit"));
            PayPageCosmetic.Default.purchase_order_number_mandatory = Convert.ToBoolean(keyPairs.Get("purchase_order_number_edit"));

            PayPageCosmetic.Default.credit_card_verification_number_label = keyPairs.Get("credit_card_verification_number_label");
            PayPageCosmetic.Default.credit_card_verification_number_visible = Convert.ToBoolean(keyPairs.Get("credit_card_verification_number_visible"));
            PayPageCosmetic.Default.credit_card_verification_number_edit = Convert.ToBoolean(keyPairs.Get("credit_card_verification_number_edit"));
            PayPageCosmetic.Default.credit_card_verification_number_mandatory = Convert.ToBoolean(keyPairs.Get("credit_card_verification_number_mandatory"));

            PayPageCosmetic.Default.customer_information_label = keyPairs.Get("customer_information_label");
            PayPageCosmetic.Default.customer_information_visible = Convert.ToBoolean(keyPairs.Get("customer_information_visible"));

            PayPageCosmetic.Default.order_information_label = keyPairs.Get("order_information_label");
            PayPageCosmetic.Default.order_information_visible = Convert.ToBoolean(keyPairs.Get("order_information_visible"));

            PayPageCosmetic.Default.card_information_label = keyPairs.Get("card_information_label");
            PayPageCosmetic.Default.card_information_visible = Convert.ToBoolean(keyPairs.Get("card_information_visible"));
            PayPageCosmetic.Default.Save();
            
        }
        public string CustomizationBuilder()
        {
            StringBuilder sb = new StringBuilder();

            //Billing Customer Title
            if (BillCustomerLabel.Text != "")
            {
                sb.Append("&bill_customer_title_label=");
                sb.Append(BillCustomerLabel.Text);
            }
            if (BillCustomerVisible.IsChecked == false)
            {
                sb.Append("&bill_customer_title_visible=false");
            }
            if (BillCustomerEditable.IsChecked == false)
            {
                sb.Append("&bill_customer_title_edit=false");
            }
            if (BillCustomerMandatory.IsChecked == true)
            {
                sb.Append("&bill_customer_title_mandatory=true");
            }
            //Billing First Name
            if (BillingFirstNameLabel.Text != "")
            {
                sb.Append("&bill_first_name_label=");
                sb.Append(BillingFirstNameLabel.Text);
            }
            if (BillingFirstNameVisible.IsChecked == false)
            {
                sb.Append("&bill_first_name_visible=false");
            }
            if (BillingFirstNameEditable.IsChecked == false)
            {
                sb.Append("&bill_first_name_edit=false");
            }
            if (BillingFirstNameMandatory.IsChecked == true)
            {
                sb.Append("&bill_first_name_mandatory=true");
            }
            //Billing Middle Name
            if (BillingMiddleNameLabel.Text != "")
            {
                sb.Append("&bill_middle_name_label=");
                sb.Append(BillingMiddleNameLabel.Text);
            }
            if (BillingMiddleNameVisible.IsChecked == false)
            {
                sb.Append("&bill_middle_name_visible=false");
            }
            if (BillingMiddleNameEditable.IsChecked == false)
            {
                sb.Append("&bill_middle_name_edita=false");
            }
            if (BillingMiddleNameMandatory.IsChecked == true)
            {
                sb.Append("&bill_middle_name_mandatory=true");
            }
            //Billing Last Name
            if (BillingLastNameLabel.Text != "")
            {
                sb.Append("&bill_last_name_label=");
                sb.Append(BillingLastNameLabel.Text);
            }
            if (BillingLastNameVisible.IsChecked == false)
            {
                sb.Append("&bill_last_name_visible=false");
            }
            if (BillingLastNameEditable.IsChecked == false)
            {
                sb.Append("&bill_last_name_edit=false");
            }
            if (BillingLastNameMandatory.IsChecked == true)
            {
                sb.Append("&bill_last_name_mandatory=true");
            }
            //Billing Company
            if (BillingCompanyLabel.Text != "")
            {
                sb.Append("&bill_company_label=");
                sb.Append(BillingCompanyLabel.Text);
            }
            if (BillingCompanyVisible.IsChecked == false)
            {
                sb.Append("&bill_company_visible=false");
            }
            if (BillingCompanyEditable.IsChecked == false)
            {
                sb.Append("&bill_company_edit=false");
            }
            if (BillingCompanyMandatory.IsChecked == true)
            {
                sb.Append("&bill_company_mandatory=true");
            }
            //Billing Address One
            if (BillingAddressOneLabel.Text != "")
            {
                sb.Append("&bill_address_one_label=");
                sb.Append(BillingAddressOneLabel.Text);
            }
            if (BillingAddressOneVisible.IsChecked == false)
            {
                sb.Append("&bill_address_one_visible=false");
            }
            if (BillingAddressOneEditable.IsChecked == false)
            {
                sb.Append("&bill_address_one_edit=false");
            }
            if (BillingAddressOneMandatory.IsChecked == true)
            {
                sb.Append("&bill_address_one_mandatory=true");
            }
            //Bill Address two
            if (BillingAddressTwoLabel.Text != "")
            {
                sb.Append("&bill_address_two_label=");
                sb.Append(BillingAddressTwoLabel.Text);
            }
            if (BillingAddressTwoVisible.IsChecked == false)
            {
                sb.Append("&bill_address_two_visible=false");
            }
            if (BillingAddressTwoEditable.IsChecked == false)
            {
                sb.Append("&bill_address_two_edit=false");
            }
            if (BillingAddressTwoMandatory.IsChecked == true)
            {
                sb.Append("&bill_address_two_mandatory=true");
            }
            //Billing City
            if (BillingCityLabel.Text != "")
            {
                sb.Append("&bill_city_label=");
                sb.Append(BillingCityLabel.Text);
            }
            if (BillingCityVisible.IsChecked == false)
            {
                sb.Append("&bill_city_visible=false");
            }
            if (BillingCityEditable.IsChecked == false)
            {
                sb.Append("&bill_city_edit=false");
            }
            if (BillingCityMandatory.IsChecked == true)
            {
                sb.Append("&bill_city_mandatory=true");
            }
            //Billing State/Province
            if (BillingStateProvinceLabel.Text != "")
            {
                sb.Append("&bill_state_or_province_label=");
                sb.Append(BillingStateProvinceLabel.Text);
            }
            if (BillingStateProvinceVisible.IsChecked == false)
            {
                sb.Append("&bill_state_or_province_visible=false");
            }
            if (BillingStateProvinceEditable.IsChecked == false)
            {
                sb.Append("&bill_state_or_province_edit=false");
            }
            if (BillingStateProvinceMandatory.IsChecked == true)
            {
                sb.Append("&bill_state_or_province_mandatory=true");
            }
            //Billing Country Code
            if (BillingCountryCodeLabel.Text != "")
            {
                sb.Append("&bill_country_code_label=");
                sb.Append(BillingCountryCodeLabel.Text);
            }
            if (BillingCountryCodeVisible.IsChecked == false)
            {
                sb.Append("&bill_country_code_visible=false");
            }
            if (BillingCountryCodeEditable.IsChecked == false)
            {
                sb.Append("&bill_country_code_edit=false");
            }
            if (BillingCountryCodeMandatory.IsChecked == true)
            {
                sb.Append("&bill_country_code_mandatory=true");
            }
            //Billing Postal Code
            if (BillingPostalCodeLabel.Text != "")
            {
                sb.Append("&bill_postal_code_label=");
                sb.Append(BillingPostalCodeLabel.Text);
            }
            if (BillingPostalCodeVisible.IsChecked == false)
            {
                sb.Append("&bill_postal_code_visible=false");
            }
            if (BillingPostalCodeEditable.IsChecked == false)
            {
                sb.Append("&bill_postal_code_edit=false");
            }
            if (BillingPostalCodeMandatory.IsChecked == true)
            {
                sb.Append("&bill_postal_code_mandatory=true");
            }
            //Invoice Number
            if (InvoiceNumberLabel.Text != "")
            {
                sb.Append("&invoice_number_label=");
                sb.Append(InvoiceNumberLabel.Text);
            }
            if (InvoiceNumberVisible.IsChecked == true)
            {
                sb.Append("&invoice_number_visible=true");
            }
            if (InvoiceNumberEditable.IsChecked == false)
            {
                sb.Append("&invoice_number_edit=false");
            }
            if (InvoiceNumberMandatory.IsChecked == true)
            {
                sb.Append("&invoice_number_mandatory=true");
            }
            //Purchase Order Number
            if (PONumberLabel.Text != "")
            {
                sb.Append("&purchase_order_number_label=");
                sb.Append(PONumberLabel.Text);
            }
            if (PONumberVisible.IsChecked == true)
            {
                sb.Append("&purchase_order_number_visible=true");
            }
            if (PONumberEditable.IsChecked == false)
            {
                sb.Append("&purchase_order_number_edit=false");
            }
            if (PONumberMandatory.IsChecked == true)
            {
                sb.Append("&urchase_order_number_mandatory=true");
            }
            //CVV Number
            if (CVVNumberLabel.Text != "")
            {
                sb.Append("&credit_card_verification_number_label=");
                sb.Append(CVVNumberLabel.Text);
            }
            if (CVVNumberVisible.IsChecked == false)
            {
                sb.Append("&credit_card_verification_number_visible=false");
            }
            if (CVVNumberEditable.IsChecked == false)
            {
                sb.Append("&credit_card_verification_number_editable=false");
            }
            if (CVVNumberMandatory.IsChecked == true)
            {
                sb.Append("&credit_card_verification_number_mandatory=true");
            }
            //Order Information
            if (OrderInformationLabel.Text != "")
            {
                sb.Append("&order_information_label=");
                sb.Append(OrderInformationLabel.Text);
            }
            if (OrderInformationVisible.IsChecked == false)
            {
                sb.Append("&order_information_visible=false");
            }
            //CardInformation
            if (CardInformationLabel.Text != "")
            {
                sb.Append("&card_information_label=");
                sb.Append(CardInformationLabel.Text);
            }
            if (CardInformationVisible.IsChecked == false)
            {
                sb.Append("&card_information_visible=false");
            }
            //Customer Information
            if (CustomerInformationLabel.Text != "")
            {
                sb.Append("&customer_information_label=");
                sb.Append(CustomerInformationLabel.Text);
            }
            if (CustomerInformationVisible.IsChecked == false)
            {
                sb.Append("&customer_information_visible=false");
            }



            return sb.ToString();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder combiner = new StringBuilder();
            combiner.Append(Globals.Default.CustomParameters);
            combiner.Append(CustomizationBuilder());
            Globals.Default.CustomParameters = "";
            Globals.Default.CustomParameters = combiner.ToString();
            Globals.Default.Save();
            PayPageCosmetic.Default.Save();
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BillCustomerVisible_Checked(object sender, RoutedEventArgs e)
        {
            PayPageCosmetic.Default.bill_customer_title_visible = BillCustomerVisible.IsChecked.Value;

        }

        private void BillCustomerVisible_Unchecked(object sender, RoutedEventArgs e)
        {
            PayPageCosmetic.Default.bill_customer_title_visible = BillCustomerVisible.IsChecked.Value;
        }
    }
}

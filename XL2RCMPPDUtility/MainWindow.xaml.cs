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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace XL2RCMPPDUtility
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            ppdXmlInputLabel.Visibility = Visibility.Hidden;
            PPDXMLText.Visibility = Visibility.Hidden;
        }

        public static void SendCommandToXL2(string parameters) //Function Sends Commands to XL2 Dll, references in Project
        {

            string response;
            XCharge.XpressLink2.XLEmv xl2 = new XCharge.XpressLink2.XLEmv();
            xl2.Execute(parameters, out response);

            MessageBox.Show(response, "Response from PPD");
        }

        private void FunctionTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            if (((ComboBoxItem)FunctionTypeComboBox.SelectedItem).Name.ToString() == "GETRCMCURRENTUSERPORT")
            {
                PPDXMLText.Visibility = Visibility.Hidden;
                ppdXmlInputLabel.Visibility = Visibility.Hidden;

            }
            else
            {
                PPDXMLText.Visibility = Visibility.Visible;
                ppdXmlInputLabel.Visibility = Visibility.Visible;
            }
        }

        private void submitToXl2Button_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                if (((ComboBoxItem)FunctionTypeComboBox.SelectedItem).Name.ToString() == "GETRCMCURRENTUSERPORT")
                {
                    string parameters = "/TRANSACTIONTYPE:GETRCMCURRENTUSERPORT";
                    SendCommandToXL2(parameters);
                }
                else
                {
                    string parameters = "/TRANSACTIONTYPE:PPDAPPLYDEVICECONFIGURATION \"/PPDXMLINPUT:" + PPDXMLText.Text + "\"";
                    SendCommandToXL2(parameters);
                }
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Please Select a Transaction Type", "Error!");
            }

        }
	
    }
}

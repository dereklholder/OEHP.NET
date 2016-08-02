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

namespace DM_Tester
{
    /// <summary>
    /// Interaction logic for Credentials.xaml
    /// </summary>
    public partial class Credentials : Window
    {
        public Credentials()
        {
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            PaymentEngine pe = new PaymentEngine();
            string inquiry = pe.BatchInquiryGenerator(Globals.Default.XWebID, Globals.Default.AuthKey, Globals.Default.TerminalID);
            string result = pe.CallGateway(inquiry, VariableHandler.TestReportURL);
            string[] responseCode = ResultReader.GetResponseCode(result);
            if (responseCode[0] == "004")
            {
                Globals.Default.Save();
                this.Close();
            }
            else
            {
                MessageBox.Show("Error Communicating with Gateway. Please check Credentials." + Environment.NewLine + "Description: " + responseCode[1]);
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

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

namespace EdgeExpressDemoPOS
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;


        }
        public static string XWebIDCurrent = Globals.Default.XWebID;
        public static string XWebAuthKeyCurrent = Globals.Default.XWebAuthKey;
        public static string XWebTerminalIDCurrent = Globals.Default.XWebTerminalID;

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            Globals.Default.Save();
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Globals.Default.XWebID = XWebIDCurrent;
            Globals.Default.XWebAuthKey = XWebAuthKeyCurrent;
            Globals.Default.XWebTerminalID = XWebTerminalIDCurrent;
            Globals.Default.Save();
            this.Close();
        }

        private void sigCapCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            string parameters = PaymentEngine.BuildXMLSupportSignature();
            GetSignatureSupportedResultXML signatureSupported = PaymentEngine.FromXml<GetSignatureSupportedResultXML>(PaymentEngine.SendToEdgeExpress(parameters));
            if (signatureSupported.SUPPORTED != "True")
            {
                MessageBox.Show("Signature not supported by your device.", "Error");
                Globals.Default.SignatureCaptureEnabled = false;
            }
        }
    }
}

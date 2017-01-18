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
using System.Xml;

namespace OEHP_Tester
{
    /// <summary>
    /// Interaction logic for PPDApplyDeviceConfiguration.xaml
    /// </summary>
    public partial class PPDApplyDeviceConfiguration : Window
    {
        public PPDApplyDeviceConfiguration()
        {
            InitializeComponent();

            slideShowEnableCheckbox.Visibility = Visibility.Hidden;
            slideShowEnableCheckbox.IsChecked = false;
            slideShowPauseLabel.Visibility = Visibility.Hidden;
            slideShowPauseText.Visibility = Visibility.Hidden;
            slideShowStartLabel.Visibility = Visibility.Hidden;
            slideShowStartText.Visibility = Visibility.Hidden;

            comPortLabel.Visibility = Visibility.Hidden;
            comPortText.Visibility = Visibility.Hidden;

            defaultMessageLabel.Visibility = Visibility.Hidden;
            defaultMessageText.Visibility = Visibility.Hidden;

            DebitMIDLabel.Visibility = Visibility.Hidden;
            DebitMIDText.Visibility = Visibility.Hidden;

            CreditMIDLabel.Visibility = Visibility.Hidden;
            CreditMIDText.Visibility = Visibility.Hidden;
        }

        public string BuildXMLInput(bool toSend)
        {
            StringBuilder sb = new StringBuilder();
            XmlWriterSettings ws = new XmlWriterSettings();
            if (toSend == true) // When Sending to XL2, Indent must be set to false
            {
                ws.Indent = false;
            }
            if (toSend == false) // For viewing the XML for Debug/Demo Purposes.
            {
                ws.Indent = true;
            }
            ws.OmitXmlDeclaration = true;

            using (XmlWriter xml = XmlWriter.Create(sb, ws))
            {


                xml.WriteStartElement("DeviceConfigurationData"); //Open Device Configuration Data

                if (((ComboBoxItem)deviceNameComboBox.SelectedItem).Content.ToString() != null)
                {
                    xml.WriteStartElement("DeviceName");
                    xml.WriteString(((ComboBoxItem)deviceNameComboBox.SelectedItem).Content.ToString());
                    xml.WriteEndElement();
                }
                if (suppressUICheckbox.IsChecked == true)
                {
                    xml.WriteStartElement("SuppressUi");
                    xml.WriteString("true");
                    xml.WriteEndElement();
                }
                if (suppressUICheckbox.IsChecked == false)
                {
                    xml.WriteStartElement("SuppressUi");
                    xml.WriteString("false");
                    xml.WriteEndElement();
                }

                xml.WriteStartElement("Settings"); // Open Settigns XML

                if (((ComboBoxItem)deviceNameComboBox.SelectedItem).Name != "Dynapro")
                {

                    xml.WriteStartElement("Default_Message");
                    xml.WriteString(defaultMessageText.Text);
                    xml.WriteEndElement();

                    xml.WriteStartElement("COM_Port");
                    xml.WriteString(comPortText.Text);
                    xml.WriteEndElement();
                }


                if (slideShowEnableCheckbox.IsChecked == true)
                {
                    xml.WriteStartElement("SlideShow_Enabled");
                    xml.WriteString("1");
                    xml.WriteEndElement();

                    xml.WriteStartElement("SlideShow_Pause");
                    xml.WriteString(slideShowPauseText.Text);
                    xml.WriteEndElement();

                    xml.WriteStartElement("SlideShow_Start");
                    xml.WriteString(slideShowStartText.Text);
                    xml.WriteEndElement();

                }
                if (((ComboBoxItem)deviceNameComboBox.SelectedItem).Name == "iPPCAEMV")
                {
                    xml.WriteStartElement("MID");
                    xml.WriteString(DebitMIDText.Text);
                    xml.WriteEndElement();

                    xml.WriteStartElement("CreditMid");
                    xml.WriteString(CreditMIDText.Text);
                    xml.WriteEndElement();

                }
                xml.WriteEndElement(); //Close Settings XML

                xml.WriteStartElement("SetupActionsToRun"); // open Setup actions to run

                xml.WriteStartElement("DeviceSetupFunctions");
                xml.WriteString(((ComboBoxItem)deviceSetupFunctionsCombobox.SelectedItem).Name);
                xml.WriteEndElement();

                xml.WriteEndElement(); //Close Setup Actions to Run

                xml.WriteEndElement(); //Close DeviceConfiguration Data
            }

            return sb.ToString();
            
        }

        private void deviceNameComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                switch (((ComboBoxItem)deviceNameComboBox.SelectedItem).Name)
                {
                    case "Dynapro":
                        slideShowEnableCheckbox.Visibility = Visibility.Hidden;
                        slideShowEnableCheckbox.IsChecked = false;
                        slideShowPauseLabel.Visibility = Visibility.Hidden;
                        slideShowPauseText.Visibility = Visibility.Hidden;
                        slideShowStartLabel.Visibility = Visibility.Hidden;
                        slideShowStartText.Visibility = Visibility.Hidden;

                        comPortLabel.Visibility = Visibility.Hidden;
                        comPortText.Visibility = Visibility.Hidden;

                        defaultMessageLabel.Visibility = Visibility.Hidden;
                        defaultMessageText.Visibility = Visibility.Hidden;

                        DebitMIDLabel.Visibility = Visibility.Hidden;
                        DebitMIDText.Visibility = Visibility.Hidden;

                        CreditMIDLabel.Visibility = Visibility.Hidden;
                        CreditMIDText.Visibility = Visibility.Hidden;

                        break;

                    case "iPP320":
                        slideShowEnableCheckbox.Visibility = Visibility.Hidden;
                        slideShowEnableCheckbox.IsChecked = false;
                        slideShowPauseLabel.Visibility = Visibility.Hidden;
                        slideShowPauseText.Visibility = Visibility.Hidden;
                        slideShowStartLabel.Visibility = Visibility.Hidden;
                        slideShowStartText.Visibility = Visibility.Hidden;

                        comPortLabel.Visibility = Visibility.Visible;
                        comPortText.Visibility = Visibility.Visible;

                        defaultMessageLabel.Visibility = Visibility.Visible;
                        defaultMessageText.Visibility = Visibility.Visible;

                        DebitMIDLabel.Visibility = Visibility.Hidden;
                        DebitMIDText.Visibility = Visibility.Hidden;

                        CreditMIDLabel.Visibility = Visibility.Hidden;
                        CreditMIDText.Visibility = Visibility.Hidden;

                        break;

                    case "iSC250":
                        slideShowEnableCheckbox.Visibility = Visibility.Visible;
                        slideShowEnableCheckbox.IsChecked = false;
                        slideShowPauseLabel.Visibility = Visibility.Visible;
                        slideShowPauseText.Visibility = Visibility.Visible;
                        slideShowStartLabel.Visibility = Visibility.Visible;
                        slideShowStartText.Visibility = Visibility.Visible;

                        comPortLabel.Visibility = Visibility.Visible;
                        comPortText.Visibility = Visibility.Visible;

                        defaultMessageLabel.Visibility = Visibility.Visible;
                        defaultMessageText.Visibility = Visibility.Visible;

                        DebitMIDLabel.Visibility = Visibility.Hidden;
                        DebitMIDText.Visibility = Visibility.Hidden;

                        CreditMIDLabel.Visibility = Visibility.Hidden;
                        CreditMIDText.Visibility = Visibility.Hidden;

                        break;

                    case "iSC480":

                        slideShowEnableCheckbox.Visibility = Visibility.Visible;
                        slideShowEnableCheckbox.IsChecked = false;
                        slideShowPauseLabel.Visibility = Visibility.Visible;
                        slideShowPauseText.Visibility = Visibility.Visible;
                        slideShowStartLabel.Visibility = Visibility.Visible;
                        slideShowStartText.Visibility = Visibility.Visible;

                        comPortLabel.Visibility = Visibility.Visible;
                        comPortText.Visibility = Visibility.Visible;

                        defaultMessageLabel.Visibility = Visibility.Visible;
                        defaultMessageText.Visibility = Visibility.Visible;

                        DebitMIDLabel.Visibility = Visibility.Hidden;
                        DebitMIDText.Visibility = Visibility.Hidden;

                        CreditMIDLabel.Visibility = Visibility.Hidden;
                        CreditMIDText.Visibility = Visibility.Hidden;

                        break;

                    case "iPP320EMV":

                        slideShowEnableCheckbox.Visibility = Visibility.Hidden;
                        slideShowEnableCheckbox.IsChecked = false;
                        slideShowPauseLabel.Visibility = Visibility.Hidden;
                        slideShowPauseText.Visibility = Visibility.Hidden;
                        slideShowStartLabel.Visibility = Visibility.Hidden;
                        slideShowStartText.Visibility = Visibility.Hidden;

                        comPortLabel.Visibility = Visibility.Visible;
                        comPortText.Visibility = Visibility.Visible;

                        defaultMessageLabel.Visibility = Visibility.Visible;
                        defaultMessageText.Visibility = Visibility.Visible;

                        DebitMIDLabel.Visibility = Visibility.Hidden;
                        DebitMIDText.Visibility = Visibility.Hidden;

                        CreditMIDLabel.Visibility = Visibility.Hidden;
                        CreditMIDText.Visibility = Visibility.Hidden;

                        break;

                    case "iSC250EMV":

                        slideShowEnableCheckbox.Visibility = Visibility.Visible;
                        slideShowEnableCheckbox.IsChecked = false;
                        slideShowPauseLabel.Visibility = Visibility.Visible;
                        slideShowPauseText.Visibility = Visibility.Visible;
                        slideShowStartLabel.Visibility = Visibility.Visible;
                        slideShowStartText.Visibility = Visibility.Visible;

                        comPortLabel.Visibility = Visibility.Visible;
                        comPortText.Visibility = Visibility.Visible;

                        defaultMessageLabel.Visibility = Visibility.Visible;
                        defaultMessageText.Visibility = Visibility.Visible;

                        DebitMIDLabel.Visibility = Visibility.Hidden;
                        DebitMIDText.Visibility = Visibility.Hidden;

                        CreditMIDLabel.Visibility = Visibility.Hidden;
                        CreditMIDText.Visibility = Visibility.Hidden;

                        break;

                    case "iSC480EMV":

                        slideShowEnableCheckbox.Visibility = Visibility.Visible;
                        slideShowEnableCheckbox.IsChecked = false;
                        slideShowPauseLabel.Visibility = Visibility.Visible;
                        slideShowPauseText.Visibility = Visibility.Visible;
                        slideShowStartLabel.Visibility = Visibility.Visible;
                        slideShowStartText.Visibility = Visibility.Visible;

                        comPortLabel.Visibility = Visibility.Visible;
                        comPortText.Visibility = Visibility.Visible;

                        defaultMessageLabel.Visibility = Visibility.Visible;
                        defaultMessageText.Visibility = Visibility.Visible;

                        DebitMIDLabel.Visibility = Visibility.Hidden;
                        DebitMIDText.Visibility = Visibility.Hidden;

                        CreditMIDLabel.Visibility = Visibility.Hidden;
                        CreditMIDText.Visibility = Visibility.Hidden;

                        break;

                    case "iPPCAEMV":

                        slideShowEnableCheckbox.Visibility = Visibility.Hidden;
                        slideShowEnableCheckbox.IsChecked = false;
                        slideShowPauseLabel.Visibility = Visibility.Hidden;
                        slideShowPauseText.Visibility = Visibility.Hidden;
                        slideShowStartLabel.Visibility = Visibility.Hidden;
                        slideShowStartText.Visibility = Visibility.Hidden;

                        comPortLabel.Visibility = Visibility.Visible;
                        comPortText.Visibility = Visibility.Visible;

                        defaultMessageLabel.Visibility = Visibility.Visible;
                        defaultMessageText.Visibility = Visibility.Visible;

                        DebitMIDLabel.Visibility = Visibility.Visible;
                        DebitMIDText.Visibility = Visibility.Visible;

                        CreditMIDLabel.Visibility = Visibility.Visible;
                        CreditMIDText.Visibility = Visibility.Visible;

                        break;

                    default:
                        break;

                }
            }
            catch (NullReferenceException)
            {

                //Do Nothing
            }
        }

        private void sendToXL2Button_Click(object sender, RoutedEventArgs e)
        {
            string parameters = "/TRANSACTIONTYPE:PPDAPPLYDEVICECONFIGURATION \"/PPDXMLINPUT:" + BuildXMLInput(true) + "\"";
            string response;

            XCharge.XpressLink2.XLEmv xl2 = new XCharge.XpressLink2.XLEmv();
            xl2.Execute(parameters, out response);

            MessageBox.Show(response, "Response from PinPad");
        }

        private void showPPDXML_Click(object sender, RoutedEventArgs e)
        {
            string xml = BuildXMLInput(false);

            if (xml != null)
            {
                MessageBox.Show(xml, "PPD XML");
            }
            else
            {
                MessageBox.Show("Invalid Configuration Settings.", "Error!");
            }
        }
    }
}

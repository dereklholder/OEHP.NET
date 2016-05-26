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

namespace DeveloperServicesPOS
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
            if (Properties.Settings.Default.OEEnabled == "true")
            {
                EnabledCheckBox.IsChecked = true;
            }
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Properties.Settings.Default.AccountToken = AccountTokenBox.Text;
                Properties.Settings.Default.RCMEnabled = RCMEnabledBox.SelectedValue.ToString();
                Properties.Settings.Default.DuplicateMode = DuplicateModeBox.SelectedValue.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("One or More Settings not Defined, Please select all Settings.");
            }
        }

        private void DuplicateModeBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void EnabledCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.OEEnabled = "true";
        }

        private void EnabledCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.OEEnabled = "false";
        }
    }
}

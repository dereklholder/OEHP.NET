using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Interaction logic for GetOEAccount.xaml
    /// </summary>
    public partial class GetOEAccount : Window
    {
        public GetOEAccount()
        {
            InitializeComponent();
        }

        private void image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Process openBrowser = new Process();
            try
            {
                openBrowser.StartInfo.FileName = "https://www.google.com";
                openBrowser.Start();
            }
            catch
            {
                MessageBox.Show("An error occured when opening the browser, please go to URL");
            }
        }
        private void Hyperlink_MouseLeftButtonDown(object sender, MouseEventArgs e)
        {
            var hyperlink = (Hyperlink)sender;
            Process.Start(hyperlink.NavigateUri.ToString());
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void richTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
    
}

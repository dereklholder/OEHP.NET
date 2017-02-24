using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for SignatureLookupDisplayWindow.xaml
    /// </summary>
    public partial class SignatureLookupDisplayWindow : Window
    {
        public SignatureLookupDisplayWindow(string sigString)
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            decodeBase64SigImage(sigString);
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void decodeBase64SigImage(string sigString)
        {
            byte[] bytes = Convert.FromBase64String(sigString);

            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.StreamSource = new MemoryStream(bytes);
            bi.EndInit();

            sigImage.Source = bi;
        }
    }
}

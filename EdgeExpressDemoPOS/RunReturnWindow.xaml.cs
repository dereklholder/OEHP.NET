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
    /// Interaction logic for RunReturnWindow.xaml
    /// </summary>
    public partial class RunReturnWindow : Window
    {
        public RunReturnWindow(string amount)
        {
            InitializeComponent();
            amountBox.Text = amount;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;

        }
        public string ContinueWithTransaction
        {
            get;
            private set;
        }

        private void continueButton_Click(object sender, RoutedEventArgs e)
        {
            ContinueWithTransaction = "Yes";
            this.Close();
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            ContinueWithTransaction = "No";
            this.Close();
        }
    }
}

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
    /// Interaction logic for TransactionIDLookupWindow.xaml
    /// </summary>
    public partial class TransactionIDLookupWindow : Window
    {
        public TransactionIDLookupWindow()
        {
            InitializeComponent();
        }

        private void Lookup_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }
    }
}

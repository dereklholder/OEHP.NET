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
using System.Web;

namespace OEHP_Tester
{
    /// <summary>
    /// Interaction logic for Receipt.xaml
    /// </summary>
    public partial class Receipt : Window
    {
        public Receipt(string receiptData)
        {
            InitializeComponent();
            receiptText.Text = HttpUtility.UrlDecode(receiptData);
        }
    }
}

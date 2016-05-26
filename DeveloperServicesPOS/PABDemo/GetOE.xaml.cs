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

namespace DeveloperServicesPOS.PABDemo
{
    /// <summary>
    /// Interaction logic for GetOE.xaml
    /// </summary>
    public partial class GetOE : Window
    {
        public GetOE()
        {
            InitializeComponent();

            DataContext = "https://staging.agreementexpress.net/api/TemplateServices/v2/publishTransaction?agexp_transrefid=LP_Test&agexp_transfolderid=1748&agexp_transcompany=645&allRecipients=Owner1:Owner1&aex_direct=true";
        }

        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Uri.ToString());
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}

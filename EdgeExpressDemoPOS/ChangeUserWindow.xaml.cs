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
    /// Interaction logic for ChangeUserWindow.xaml
    /// </summary>
    public partial class ChangeUserWindow : Window
    {
        public ChangeUserWindow()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }
        public static string CurrentUser = Globals.Default.ClerkID;

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Globals.Default.ClerkID = CurrentUser;
            Globals.Default.Save();
            this.Close();
        }
    }
}

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
    /// Interaction logic for PABDemoMain.xaml
    /// </summary>
    public partial class PABDemoMain : Window
    {
        public PABDemoMain()
        {
            InitializeComponent();
        }

        private void CreditRectangle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            PABDemo.GetOE window = new PABDemo.GetOE();
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.ShowDialog();
        }

        private void CheckRectangle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            PABDemo.GetOE window = new PABDemo.GetOE();
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.ShowDialog();
        }

        private void GiftRectangle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            PABDemo.GetOE window = new PABDemo.GetOE();
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.ShowDialog();
        }

        private void Window_Closed(object sender, EventArgs e)
        {

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MainWindow window = new MainWindow();
            window.Show();
        }

        private void CreditLabel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            PABDemo.GetOE window = new PABDemo.GetOE();
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.ShowDialog();
        }

        private void CreditLabel_Copy_MouseDown(object sender, MouseButtonEventArgs e)
        {
            PABDemo.GetOE window = new PABDemo.GetOE();
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.ShowDialog();
        }

        private void GiftLabel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            PABDemo.GetOE window = new PABDemo.GetOE();
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.ShowDialog();
        }
    }
}

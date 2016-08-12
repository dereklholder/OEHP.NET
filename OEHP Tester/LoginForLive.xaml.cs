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

namespace OEHP_Tester
{
    /// <summary>
    /// Interaction logic for LoginForLive.xaml
    /// </summary>
    public partial class LoginForLive : Window
    {
        public LoginForLive()
        {
            InitializeComponent();
        }
        public bool CorrectLogin { get; set; } 
        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            if (usernameBox.Text == "DevServices" && passwordBox.Password == "S3cr3tP@ssword")
            {
                CorrectLogin = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Invalid Username and Password");
                CorrectLogin = false;
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            CorrectLogin = false;
            this.Close();
        }
    }
}

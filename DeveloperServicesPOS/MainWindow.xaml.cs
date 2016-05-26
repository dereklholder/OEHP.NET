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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Collections;
using System.Reflection;
using System.ComponentModel;

namespace DeveloperServicesPOS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            
            InitializeComponent();
            DataContext = this;
        }

        private void rapidActivateOpen(object sender, RoutedEventArgs e) //Opens Web Browser to URL, Used to Navigate to RapidActivate URL
        {
            GetOEAccount window = new GetOEAccount();
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.ShowDialog();
        }

        private void SettingsClick(object sender, RoutedEventArgs e)
        {
            SettingsWindow w = new SettingsWindow();
            w.ShowDialog();
        }

        private void ExitButtonClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Item1Image_MouseDown(object sender, MouseButtonEventArgs e)
        {

            CurrentTicketList.Items.Add(new ItemController { Item = "Super Burger", Price = 3.99 });

            double currentPrice = double.Parse(TotalAmountBox.Text);
            TotalAmountBox.Text = (currentPrice + 1.99).ToString();

        }
        public class ItemController
        {
            public string Item { get; set; }

            public double Price { get; set; }
        }
        public class GlobalVariables
        {
            public static string currentTicketPrice { get; set; }
            public static string LastTransactionResult { get; set; }
        }

        private void Item2Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            CurrentTicketList.Items.Add(new ItemController { Item = "Soda", Price = .99 });

            double currentPrice = double.Parse(TotalAmountBox.Text);
            TotalAmountBox.Text = (currentPrice + 1.99).ToString();
        }

        private void Item3Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            CurrentTicketList.Items.Add(new ItemController { Item = "Fries", Price = 1.99 });

            double currentPrice = double.Parse(TotalAmountBox.Text);
            TotalAmountBox.Text = (currentPrice + 1.99).ToString();
        }

        private void CurrentTicketList_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ListView listView = sender as ListView;
            GridView gView = listView.View as GridView;

            var workingWidth = listView.ActualWidth - 35; // take into account vertical scrollbar
            var col1 = 0.80;
            var col2 = 0.20;

            gView.Columns[0].Width = workingWidth * col1;
            gView.Columns[1].Width = workingWidth * col2;
        }

        

        private void CancelTransaction_Click(object sender, RoutedEventArgs e)
        {
            CurrentTicketList.Items.Clear();
            TotalAmountBox.Text = "0";
        }

        private void TotalAmountBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            GlobalVariables.currentTicketPrice = TotalAmountBox.Text;
        }

        private void CreditButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (Properties.Settings.Default.OEEnabled == "true")
            {
                TransactionRequest tr = new TransactionRequest();
                string orderID = tr.OrderIDRandom(8);
                string transactionRequest = tr.CreditCardParamBuilder(Properties.Settings.Default.AccountToken, "CREDIT_CARD", "SALE", "EMV", orderID, TotalAmountBox.Text, "&prompt_signature=TRUE");

                OEHP.NET.GatewayRequest gr = new OEHP.NET.GatewayRequest();
                ProcessingWindow pw = new ProcessingWindow(gr.TestPayPagePost(transactionRequest), "CREDIT_CARD", orderID);
                pw.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                pw.ShowDialog();

                if (GlobalVariables.LastTransactionResult == "1")
                {
                    MessageBox.Show("Transaction Was Successful!");
                    CurrentTicketList.Items.Clear();
                    TotalAmountBox.Text = "0";
                }
                if (GlobalVariables.LastTransactionResult != "1")
                {
                    MessageBox.Show("Transaction Failed!");
                    CurrentTicketList.Items.Clear();
                    TotalAmountBox.Text = "0";
                } 
            }
            else
            {
                MessageBox.Show("Integrated payment processing not enabled! Please enabled Payment Processing by going to File -> Settings. If you do not have an OpenEdge Account, you can go to Help -> Integrated Payment Processing to learn more");

            }
            
        }

        private void DebitButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TransactionRequest tr = new TransactionRequest();
            string orderID = tr.OrderIDRandom(8);
            string transactionRequest = tr.DebitCardParamBuilder(Properties.Settings.Default.AccountToken, "DEBIT_CARD", "PURCHASE", "EMV", orderID, TotalAmountBox.Text, "DEFAULT", "");

            OEHP.NET.GatewayRequest gr = new OEHP.NET.GatewayRequest();
            ProcessingWindow pw = new ProcessingWindow(gr.TestPayPagePost(transactionRequest), "DEBIT_CARD", orderID);
            pw.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            pw.ShowDialog();

            if (GlobalVariables.LastTransactionResult == "1")
            {
                MessageBox.Show("Transaction Was Successful!");
                CurrentTicketList.Items.Clear();
                TotalAmountBox.Text = "0";
            }
            if (GlobalVariables.LastTransactionResult != "1")
            {
                MessageBox.Show("Transaction Failed!");
                CurrentTicketList.Items.Clear();
                TotalAmountBox.Text = "0";
            }

        }

        private void CurrentTicketList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        

        private void PabDemoClick_Click(object sender, RoutedEventArgs e)
        {
            PABDemoMain pdm = new PABDemoMain();
            pdm.Show();
            this.Close();
            
        }
    }
}

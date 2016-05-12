﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Web;
using System.Net;
using System.Windows.Controls;
using System.Data.SQLite;

namespace OEHP_Tester
{
    public class GeneralFunctions : OEHP.NET.VariableHandler
    {
        public void WriteToLog(string logString) //Code for logging functions.
        {
            try
            {
                var logPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, System.IO.Path.Combine("Log", "Log.txt")).ToString();
                string timeStamp = DateTime.Now.ToString();
                File.AppendAllText(logPath, timeStamp + Environment.NewLine + logString + Environment.NewLine + "--------------------------------------------------" + Environment.NewLine);
            }
            catch (Exception ex)
            {

                MessageBox.Show("An Error Occured when Writing to the Log File.");
            }
        }
        public BitmapImage DecodeBase64Image(string base64String)
        {
            try
            {
                
                byte[] bytes = Convert.FromBase64String(base64String);

                BitmapImage bi = new BitmapImage();
                bi.BeginInit();
                bi.StreamSource = new MemoryStream(bytes);
                bi.EndInit();

                return bi;
            }
            catch (Exception ex)
            {
                
                WriteToLog(ex.ToString());
                return null;
            }
        }
        public string GetPageContent(WebBrowser wb)
        {
            if (wb != null)
            {
                return ((mshtml.HTMLDocumentClass)wb.Document).body.innerHTML;
            }
            else
            {
                return null;
            }
        }

        public string PaymentFinishedSignal(string pageHTML)
        {
            if (pageHTML != null)
            {
                HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(pageHTML);
                try
                {
                    var value = doc.DocumentNode.SelectSingleNode("//input[@type='hidden' and @id='paymentFinishedSignal']").Attributes["value"].Value;
                    return value;
                }
                catch (Exception ex)
                {
                    GeneralFunctions gf = new GeneralFunctions();
                    gf.WriteToLog(ex.ToString());
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        public void CreateDBFile()
        {
            try
            {
                SQLiteConnection.CreateFile("tran.oehp");

                string sql = "create table transactiondb (response_code varchar(5), response_code_text varchar(50), secondary_response_code varchar(5), secondary_response_code_text varchar(50), time_stamp varchar(50), retry_recommended varchar(50), authorized_amount varchar(50), bin varchar(50), captured_amount varchar(50), original_authorized_amount varchar(50), requested_amount varchar(50), time_stamp_created varchar(50), original_response_code int, original_response_code_text varchar(50), original_secondary_response_code int, original_secondary_response_code_text varchar(50), time_stamp_updated varchar(50), state varchar(15), bank_approval_code varchar(10), expire_month varchar(5), expire_year varchar(5), order_id varchar(50), payer_identifier varchar(50), reference_id varchar(50), span varchar(50), card_brand varchar(50), batch_id varchar(10), receipt_application_cryptogram varchar(25), receipt_application_identifier varchar(25), receipt_application_preferred_name varchar(25), receipt_application_transaction_counter varchar(25), receipt_approval_code varchar(25), receipt_authorization_reseponse_code varchar(25), receipt_card_number varchar(20), receipt_card_type varchar(10), receipt_entry_legend varchar(10), receipt_entry_method varchar(10), receipt_line_items varchar(50), receipt_merchant_id varchar(20), receipt_order_id varchar(50), receipt_signature_text varchar(25), receipt_terminal_verificiation_results varchar(25), receipt_transaction_date_time varchar(25), receipt_transaction_id varchar(25), receipt_transaction_reference_number varchar(25), receipt_transaction_type varchar(10), receipt_validation_code varchar(10), receipt_verbiage varchar(50))";
                SQLiteConnection m_dbConnection = new SQLiteConnection("Data Source = tran.oehp;Version=3;");

                m_dbConnection.Open();
                SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
                command.ExecuteNonQuery();

                m_dbConnection.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show("An error occured when creating the database. Please check the log");
                WriteToLog(ex.ToString());
            }
            

        }
        public void InsertTransactionData(string sqlString)
        {
            try
            {
                SQLiteConnection m_dbConnection = new SQLiteConnection("Data Source = tran.oehp;Version=3;");

                m_dbConnection.Open();
                SQLiteCommand command = new SQLiteCommand(sqlString, m_dbConnection);
                command.ExecuteNonQuery();

                m_dbConnection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occured while writing the transaction to the database, please check the log.");
                WriteToLog(ex.ToString());
                
            }

        }
    }
}
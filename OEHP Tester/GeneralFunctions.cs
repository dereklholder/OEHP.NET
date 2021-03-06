﻿using System;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;

namespace OEHP_Tester
{

    #region Classes for Serializing XMl Results from EdgeExpress Calls
    [Serializable, XmlRoot("XLinkEMVResult")]
    public class RCMPortXML
    {
        public string RCMPORT { get; set; }
        public string RESULT { get; set; }
        public string RESULTMSG { get; set; }
    }
    [Serializable, XmlRoot("XLinkEMVResult")]
    public class PPDConfigurationXML
    {
        public string RESULT { get; set; }
        public string RESULTMSG { get; set; }
    }
#endregion

    public class GeneralFunctions : OEHP.NET.VariableHandler
    {
        #region Window Launchers
        public static void AboutWindowLauncher()
        {
            About window = new About();
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.Topmost = true;
            window.ShowDialog();
        }
        public static void PresetHelpWindowLauncher()
        {
            PresetHelp ph = new PresetHelp();
            ph.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            ph.Topmost = true;
            ph.ShowDialog();
        }
        #endregion
        #region General UI Calls
        public static void SetDupModeOff()// Sets duplicate checking mode to OFF
        {
            Globals.Default.DuplicateOn = "FALSE";
            Globals.Default.DuplicateOff = "TRUE";
            ;
            Globals.Default.Save();
        } 
        public static void SetDupModeOn()// Sets Duplicate Checking mode to ON 
        {
            Globals.Default.DuplicateOn = "TRUE";
            Globals.Default.DuplicateOff = "FALSE";

            Globals.Default.Save();
        }
        public static void NavToDevPortal()//Opens Web Browser to Developer Portal
        {
            System.Diagnostics.Process.Start(Globals.Default.DevPortalURL);
        }
        public static void EmailDevServices()//Opoens e-mail client to e-mail DeveloperServices
        {
            System.Diagnostics.Process.Start("mailto:" + Globals.Default.ContactDevServices);
        }
        #endregion
        #region Parsers and Decoders.
        public static BitmapImage DecodeBase64Image(string base64String) // Decodes Base64 Encoded Signature into a image to be displayed
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
        public static string GetPageContent(WebBrowser wb) //Parses Web Browser content from an HTMLDocument intoa  simple string
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
        public static string PaymentFinishedSignal(string pageHTML)//Scrapes HTML for Payment Finished Signal, Used with GetPageContent method
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
                    WriteToLog(ex.ToString());
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        public static string RCMStatusFromWebPage(string pageHTML) //Scrapes HTML for RCM status element, used with GetPageContent
        {
            if (pageHTML != null)
            {
                HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(pageHTML);
                try
                {
                    string rcmStartingSignal = doc.DocumentNode.SelectSingleNode("//input[@type='hidden' and @id='rcmStartingSignal']").Attributes["value"].Value;
                    string rcmFinishedSignal = doc.DocumentNode.SelectSingleNode("//input[@type='hidden' and @id='rcmFinishedSignal']").Attributes["value"].Value;
                    string rcmResponseCode = doc.DocumentNode.SelectSingleNode("//input[@type='hidden' and @id='rcmResponseCode']").Attributes["value"].Value;
                    string rcmResponseDescription = doc.DocumentNode.SelectSingleNode("//input[@type='hidden' and @id='rcmResponseDescription']").Attributes["value"].Value;
                    string result = "rcm_starting_signal=" + rcmStartingSignal + "&rcm_finished_signal=" + rcmFinishedSignal + "&rcm_response_code=" + rcmResponseCode + "&rcm_response_description=" + rcmResponseDescription;
                    return result;
                }
                catch (Exception ex)
                {
                    WriteToLog(ex.ToString());
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        #endregion
        #region Database Functions.
        public void CreateDBFile()
        {
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "tran.oehp") != true)
            {
                try
                {
                    SQLiteConnection.CreateFile(AppDomain.CurrentDomain.BaseDirectory + "tran.oehp");

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
            else
            {
                try
                {
                    File.Delete(AppDomain.CurrentDomain.BaseDirectory + "tran.oehp");

                    SQLiteConnection.CreateFile(AppDomain.CurrentDomain.BaseDirectory +  "tran.oehp");

                    string sql = "create table transactiondb (response_code varchar(5), response_code_text varchar(50), secondary_response_code varchar(5), secondary_response_code_text varchar(50), time_stamp varchar(50), retry_recommended varchar(50), authorized_amount varchar(50), bin varchar(50), captured_amount varchar(50), original_authorized_amount varchar(50), requested_amount varchar(50), time_stamp_created varchar(50), original_response_code int, original_response_code_text varchar(50), original_secondary_response_code int, original_secondary_response_code_text varchar(50), time_stamp_updated varchar(50), state varchar(15), bank_approval_code varchar(10), expire_month varchar(5), expire_year varchar(5), order_id varchar(50), payer_identifier varchar(50), reference_id varchar(50), span varchar(50), card_brand varchar(50), batch_id varchar(10), receipt_application_cryptogram varchar(25), receipt_application_identifier varchar(25), receipt_application_preferred_name varchar(25), receipt_application_transaction_counter varchar(25), receipt_approval_code varchar(25), receipt_authorization_reseponse_code varchar(25), receipt_card_number varchar(20), receipt_card_type varchar(10), receipt_entry_legend varchar(10), receipt_entry_method varchar(10), receipt_line_items varchar(50), receipt_merchant_id varchar(20), receipt_order_id varchar(50), receipt_signature_text varchar(25), receipt_terminal_verificiation_results varchar(25), receipt_transaction_date_time varchar(25), receipt_transaction_id varchar(25), receipt_transaction_reference_number varchar(25), receipt_transaction_type varchar(10), receipt_validation_code varchar(10), receipt_verbiage varchar(50))";
                    SQLiteConnection m_dbConnection = new SQLiteConnection("Data Source = tran.oehp;Version=3;");

                    m_dbConnection.Open();
                    SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
                    command.ExecuteNonQuery();

                    m_dbConnection.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An Error Occured when creating the database. Please check the log");
                    WriteToLog(ex.ToString());
                }
            }
            
            

        }
        public static void InsertTransactionData(string sqlString)
        {
            SQLiteConnection m_dbConnection = new SQLiteConnection("Data Source = tran.oehp;Version=3;");
            try
            {
                m_dbConnection.Open();
                SQLiteCommand command = new SQLiteCommand(sqlString, m_dbConnection);
                command.ExecuteNonQuery();            
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occured while writing the transaction to the database, please check the log.");
                WriteToLog(ex.ToString());         
            }
            finally
            {
                m_dbConnection.Close();
            }

        }
        public static DataTable GetPayerIDAndSpan()
        {
            DataTable dt = new DataTable();
            SQLiteConnection con = new SQLiteConnection("Data Source = tran.oehp;version=3");
            try
            {                
                con.Open();
                SQLiteCommand command = new SQLiteCommand(con);
                command.CommandText = "SELECT payer_identifier, span  FROM transactiondb";
                SQLiteDataReader sdr = command.ExecuteReader();
                dt.Load(sdr);
                sdr.Close();         
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                con.Close();
            }
            return dt;

        }
        #endregion
        public static void WriteToLog(string logString) //Code for logging.
        {
            try
            {
                var logPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, System.IO.Path.Combine("Log", "Log.txt")).ToString();
                string timeStamp = DateTime.Now.ToString();
                File.AppendAllText(logPath, timeStamp + Environment.NewLine + logString + Environment.NewLine + "--------------------------------------------------" + Environment.NewLine);
            }
            catch (Exception ex)
            {

                MessageBox.Show("An Error Occured when Writing to the Log File." + Environment.NewLine + ex.GetBaseException().ToString());
            }
        }

    }
#region Receipt Objects, may be redone at a later time.
    public class ReceiptFormatter : OEHP.NET.DataManipulation
    {
        //Go through receipt Data and Create a Tip Line
        public string TipReceipt(ReceiptDataOnly ReceiptDataObject)
        {
            StringBuilder sb = new StringBuilder(20000);
            if (string.IsNullOrEmpty(ReceiptDataObject.receipt_merchant_id_label) != true)
            {
                sb.Append(ReceiptDataObject.receipt_merchant_id_label).Append("%09").Append(ReceiptDataObject.receipt_merchant_id).Append(Environment.NewLine);
            }
            if (string.IsNullOrEmpty(ReceiptDataObject.receipt_transaction_id_label) != true)
            {
                sb.Append(ReceiptDataObject.receipt_transaction_id_label).Append("%09").Append("%09").Append(ReceiptDataObject.receipt_transaction_id).Append(Environment.NewLine);
            }
            if (string.IsNullOrEmpty(ReceiptDataObject.receipt_order_id_label) != true)
            {
                sb.Append(ReceiptDataObject.receipt_order_id_label).Append("%09").Append("%09").Append(ReceiptDataObject.receipt_order_id).Append(Environment.NewLine);
            }
            if (string.IsNullOrEmpty(ReceiptDataObject.receipt_transaction_type_label) != true)
            {
                sb.Append(ReceiptDataObject.receipt_transaction_type_label).Append("%09").Append(ReceiptDataObject.receipt_transaction_type).Append(Environment.NewLine);
            }
            if (string.IsNullOrEmpty(ReceiptDataObject.receipt_transaction_date_time_label) != true)
            {
                sb.Append(ReceiptDataObject.receipt_transaction_date_time_label).Append("%09").Append(ReceiptDataObject.receipt_transaction_date_time).Append(Environment.NewLine);
            }
            if (string.IsNullOrEmpty(ReceiptDataObject.receipt_card_type_label) != true)
            {
                sb.Append(ReceiptDataObject.receipt_card_type_label).Append("%09").Append(ReceiptDataObject.receipt_card_type).Append(Environment.NewLine);
            }
            if (string.IsNullOrEmpty(ReceiptDataObject.receipt_card_number_label) != true)
            {
                sb.Append(ReceiptDataObject.receipt_card_number_label).Append("%09").Append(ReceiptDataObject.receipt_card_number).Append(Environment.NewLine);
            }
            if (string.IsNullOrEmpty(ReceiptDataObject.receipt_entry_legend_label) != true)
            {
                sb.Append(ReceiptDataObject.receipt_entry_legend_label).Append("%09").Append(ReceiptDataObject.receipt_entry_legend).Append(Environment.NewLine);
            }
            if (string.IsNullOrEmpty(ReceiptDataObject.receipt_entry_method_label) != true)
            {
                sb.Append(ReceiptDataObject.receipt_entry_method_label).Append("%09").Append(ReceiptDataObject.receipt_entry_method).Append(Environment.NewLine);
            }
            if (string.IsNullOrEmpty(ReceiptDataObject.receipt_approval_code_label) != true)
            {
                sb.Append(ReceiptDataObject.receipt_approval_code_label).Append("%09").Append(ReceiptDataObject.receipt_approval_code).Append(Environment.NewLine);
            }
            if (string.IsNullOrEmpty(ReceiptDataObject.receipt_application_cryoptogram_label) != true)
            {
                sb.Append(ReceiptDataObject.receipt_application_cryoptogram_label).Append("%09").Append("%09").Append(ReceiptDataObject.receipt_application_cryptogram).Append(Environment.NewLine);
            }
            if (string.IsNullOrEmpty(ReceiptDataObject.receipt_application_transaction_counter_label) != true)
            {
                sb.Append(ReceiptDataObject.receipt_application_transaction_counter_label).Append("%09").Append("%09").Append(ReceiptDataObject.receipt_application_transaction_counter).Append(Environment.NewLine);
            }
            if (string.IsNullOrEmpty(ReceiptDataObject.receipt_application_identifier_label) != true)
            {
                sb.Append(ReceiptDataObject.receipt_application_identifier_label).Append("%09").Append("%09").Append(ReceiptDataObject.receipt_application_identifier).Append(Environment.NewLine);
            }
            if (string.IsNullOrEmpty(ReceiptDataObject.receipt_application_preferred_name_label) != true)
            {
                sb.Append(ReceiptDataObject.receipt_application_preferred_name_label).Append("%09").Append(ReceiptDataObject.receipt_application_preferred_name).Append(Environment.NewLine);
            }
            if (string.IsNullOrEmpty(ReceiptDataObject.receipt_terminal_verification_results_label) != true)
            {
                sb.Append(ReceiptDataObject.receipt_terminal_verification_results_label).Append("%09").Append("%09").Append(ReceiptDataObject.receipt_terminal_verification_results).Append(Environment.NewLine);
            }
            if (string.IsNullOrEmpty(ReceiptDataObject.receipt_transaction_status_information_label) != true)
            {
                sb.Append(ReceiptDataObject.receipt_transaction_status_information_label).Append("%09").Append("%09").Append(ReceiptDataObject.receipt_transaction_status_information).Append(Environment.NewLine);
            }
            if (string.IsNullOrEmpty(ReceiptDataObject.receipt_transaction_reference_number_label) != true)
            {
                sb.Append(ReceiptDataObject.receipt_transaction_reference_number_label).Append("%09").Append(ReceiptDataObject.receipt_transaction_reference_number).Append(Environment.NewLine);
            }
            if (string.IsNullOrEmpty(ReceiptDataObject.receipt_authorization_response_code_label) != true)
            {
                sb.Append(ReceiptDataObject.receipt_authorization_response_code_label).Append("%09").Append("%09").Append(ReceiptDataObject.receipt_authorization_response_code).Append(Environment.NewLine);
            }
            if (string.IsNullOrEmpty(ReceiptDataObject.receipt_validation_code_label) != true)
            {
                sb.Append(ReceiptDataObject.receipt_validation_code_label).Append("%09").Append(ReceiptDataObject.receipt_validation_code).Append(Environment.NewLine);
            }
            if (string.IsNullOrEmpty(ReceiptDataObject.receipt_total_amount_label) != true)
            {
                sb.Append(Environment.NewLine).Append("Subtotal:").Append("%09").Append("%09").Append(ReceiptDataObject.receipt_total_amount).Append(Environment.NewLine).Append(Environment.NewLine)
                    .Append("Tip:").Append("%09").Append("%09").Append("____________").Append(Environment.NewLine).Append(Environment.NewLine)
                    .Append("Total:").Append("%09").Append("%09").Append("____________").Append(Environment.NewLine);
            }
            
            if (string.IsNullOrEmpty(ReceiptDataObject.receipt_line_items) != true)
            {
                sb.Append(Environment.NewLine).Append(ReceiptDataObject.receipt_line_items).Append(Environment.NewLine);
            }
            if (string.IsNullOrEmpty(ReceiptDataObject.receipt_verbiage) != true)
            {
                sb.Append(Environment.NewLine).Append("%09").Append(ReceiptDataObject.receipt_verbiage).Append(Environment.NewLine);
            }
            if (string.IsNullOrEmpty(ReceiptDataObject.receipt_signature_line) != true)
            {
                sb.Append(ReceiptDataObject.receipt_signature_line);
            }
            if (string.IsNullOrEmpty(ReceiptDataObject.receipt_signature_text) != true)
            {
                sb.Append(Environment.NewLine).Append("%09").Append(ReceiptDataObject.receipt_signature_text).Append(Environment.NewLine);
            }
            if (string.IsNullOrEmpty(ReceiptDataObject.receipt_authorization_agreement) != true)
            {
                sb.Append(Environment.NewLine).Append(ReceiptDataObject.receipt_authorization_agreement).Append(Environment.NewLine).Append("%09").Append(ReceiptDataObject.receipt_merchant_copy_label);
            }
            return sb.ToString();
        }
        //Create Generic Receipt
        public string GenericReceipt(ReceiptDataOnly ReceiptDataObject)
        {
            StringBuilder sb = new StringBuilder(20000);
            if (string.IsNullOrEmpty(ReceiptDataObject.receipt_merchant_id_label) != true)
            {
                sb.Append(ReceiptDataObject.receipt_merchant_id_label).Append("%09").Append(ReceiptDataObject.receipt_merchant_id).Append(Environment.NewLine);
            }
            if (string.IsNullOrEmpty(ReceiptDataObject.receipt_transaction_id_label) != true)
            {
                sb.Append(ReceiptDataObject.receipt_transaction_id_label).Append("%09").Append("%09").Append(ReceiptDataObject.receipt_transaction_id).Append(Environment.NewLine);
            }
            if (string.IsNullOrEmpty(ReceiptDataObject.receipt_order_id_label) != true)
            {
                sb.Append(ReceiptDataObject.receipt_order_id_label).Append("%09").Append("%09").Append(ReceiptDataObject.receipt_order_id).Append(Environment.NewLine);
            }
            if (string.IsNullOrEmpty(ReceiptDataObject.receipt_transaction_type_label) != true)
            {
                sb.Append(ReceiptDataObject.receipt_transaction_type_label).Append("%09").Append(ReceiptDataObject.receipt_transaction_type).Append(Environment.NewLine);
            }
            if (string.IsNullOrEmpty(ReceiptDataObject.receipt_transaction_date_time_label) != true)
            {
                sb.Append(ReceiptDataObject.receipt_transaction_date_time_label).Append("%09").Append(ReceiptDataObject.receipt_transaction_date_time).Append(Environment.NewLine);
            }
            if (string.IsNullOrEmpty(ReceiptDataObject.receipt_card_type_label) != true)
            {
                sb.Append(ReceiptDataObject.receipt_card_type_label).Append("%09").Append(ReceiptDataObject.receipt_card_type).Append(Environment.NewLine);
            }
            if (string.IsNullOrEmpty(ReceiptDataObject.receipt_card_number_label) != true)
            {
                sb.Append(ReceiptDataObject.receipt_card_number_label).Append("%09").Append(ReceiptDataObject.receipt_card_number).Append(Environment.NewLine);
            }
            if (string.IsNullOrEmpty(ReceiptDataObject.receipt_entry_legend_label) != true)
            {
                sb.Append(ReceiptDataObject.receipt_entry_legend_label).Append("%09").Append(ReceiptDataObject.receipt_entry_legend).Append(Environment.NewLine);
            }
            if (string.IsNullOrEmpty(ReceiptDataObject.receipt_entry_method_label) != true)
            {
                sb.Append(ReceiptDataObject.receipt_entry_method_label).Append("%09").Append(ReceiptDataObject.receipt_entry_method).Append(Environment.NewLine);
            }
            if (string.IsNullOrEmpty(ReceiptDataObject.receipt_approval_code_label) != true)
            {
                sb.Append(ReceiptDataObject.receipt_approval_code_label).Append("%09").Append(ReceiptDataObject.receipt_approval_code).Append(Environment.NewLine);
            }
            if (string.IsNullOrEmpty(ReceiptDataObject.receipt_application_cryoptogram_label) != true)
            {
                sb.Append(ReceiptDataObject.receipt_application_cryoptogram_label).Append("%09").Append("%09").Append(ReceiptDataObject.receipt_application_cryptogram).Append(Environment.NewLine);
            }
            if (string.IsNullOrEmpty(ReceiptDataObject.receipt_application_transaction_counter_label) != true)
            {
                sb.Append(ReceiptDataObject.receipt_application_transaction_counter_label).Append("%09").Append("%09").Append(ReceiptDataObject.receipt_application_transaction_counter).Append(Environment.NewLine);
            }
            if (string.IsNullOrEmpty(ReceiptDataObject.receipt_application_identifier_label) != true)
            {
                sb.Append(ReceiptDataObject.receipt_application_identifier_label).Append("%09").Append("%09").Append(ReceiptDataObject.receipt_application_identifier).Append(Environment.NewLine);
            }
            if (string.IsNullOrEmpty(ReceiptDataObject.receipt_application_preferred_name_label) != true)
            {
                sb.Append(ReceiptDataObject.receipt_application_preferred_name_label).Append("%09").Append(ReceiptDataObject.receipt_application_preferred_name).Append(Environment.NewLine);
            }
            if (string.IsNullOrEmpty(ReceiptDataObject.receipt_terminal_verification_results_label) != true)
            {
                sb.Append(ReceiptDataObject.receipt_terminal_verification_results_label).Append("%09").Append("%09").Append(ReceiptDataObject.receipt_terminal_verification_results).Append(Environment.NewLine);
            }
            if (string.IsNullOrEmpty(ReceiptDataObject.receipt_transaction_status_information_label) != true)
            {
                sb.Append(ReceiptDataObject.receipt_transaction_status_information_label).Append("%09").Append("%09").Append(ReceiptDataObject.receipt_transaction_status_information).Append(Environment.NewLine);
            }
            if (string.IsNullOrEmpty(ReceiptDataObject.receipt_transaction_reference_number_label) != true)
            {
                sb.Append(ReceiptDataObject.receipt_transaction_reference_number_label).Append("%09").Append(ReceiptDataObject.receipt_transaction_reference_number).Append(Environment.NewLine);
            }
            if (string.IsNullOrEmpty(ReceiptDataObject.receipt_authorization_response_code_label) != true)
            {
                sb.Append(ReceiptDataObject.receipt_authorization_response_code_label).Append("%09").Append("%09").Append(ReceiptDataObject.receipt_authorization_response_code).Append(Environment.NewLine);
            }
            if (string.IsNullOrEmpty(ReceiptDataObject.receipt_validation_code_label) != true)
            {
                sb.Append(ReceiptDataObject.receipt_validation_code_label).Append("%09").Append(ReceiptDataObject.receipt_validation_code).Append(Environment.NewLine);
            }
            if (string.IsNullOrEmpty(ReceiptDataObject.receipt_total_amount_label) != true)
            {
                sb.Append(Environment.NewLine).Append(ReceiptDataObject.receipt_total_amount_label).Append("%09").Append("%09").Append(ReceiptDataObject.receipt_total_amount).Append(Environment.NewLine).Append(Environment.NewLine);               
            }

            if (string.IsNullOrEmpty(ReceiptDataObject.receipt_line_items) != true)
            {
                sb.Append(Environment.NewLine).Append(ReceiptDataObject.receipt_line_items).Append(Environment.NewLine);
            }
            if (string.IsNullOrEmpty(ReceiptDataObject.receipt_verbiage) != true)
            {
                sb.Append(Environment.NewLine).Append("%09").Append(ReceiptDataObject.receipt_verbiage).Append(Environment.NewLine);
            }
            if (string.IsNullOrEmpty(ReceiptDataObject.receipt_signature_line) != true)
            {
                sb.Append(ReceiptDataObject.receipt_signature_line);
            }
            if (string.IsNullOrEmpty(ReceiptDataObject.receipt_signature_text) != true)
            {
                sb.Append(Environment.NewLine).Append("%09").Append(ReceiptDataObject.receipt_signature_text).Append(Environment.NewLine);
            }
            if (string.IsNullOrEmpty(ReceiptDataObject.receipt_authorization_agreement) != true)
            {
                sb.Append(Environment.NewLine).Append(ReceiptDataObject.receipt_authorization_agreement).Append(Environment.NewLine).Append("%09").Append(ReceiptDataObject.receipt_merchant_copy_label);
            }
            return sb.ToString();
        }

    }
    #endregion

}

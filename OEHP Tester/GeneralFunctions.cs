using System;
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
        
    }
}

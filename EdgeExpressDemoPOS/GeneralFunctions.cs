using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;

namespace EdgeExpressDemoPOS
{
    /// <summary>
    /// Used for Various Functions such as logging.
    /// </summary>
    public class GeneralFunctions
    {
        public static void WriteToLog(string StringToWrite)
        {
            try
            {
                var logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Path.Combine("Log", "Log.Txt")).ToString();
                string timeStamp = DateTime.Now.ToString();
                File.AppendAllText(logPath, timeStamp + Environment.NewLine + StringToWrite + Environment.NewLine + "--------------------------------------------------" + Environment.NewLine);

            }
            catch (Exception ex)
            {
                MessageBox.Show("An Error Occured when Writing to the Log File." + Environment.NewLine + ex.GetBaseException().ToString());
            }
        }
    }
    public class ItemController
    {
        public string Item { get; set; }
        public double Price { get; set; }
    }
    public class VariableHandler
    {
        public static string RCMUrl = "https://localsystem.paygateway.com:21113/RcmService.svc";
        public static string RCMMethod = "/Initialize";
        public static string RCMQuerystring = "?xl2Parameters=";
        public static string RCMStatusMethod = "/Status";
    }
    public enum TranType
    {
        Sale,
        DebitReturn,
        CreditReturn,
        Void
    };
}

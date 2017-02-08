using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;

namespace EdgeExpressDemoPOS
{
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
}

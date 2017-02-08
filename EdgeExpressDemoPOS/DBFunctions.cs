using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data.SQLite;
using System.Windows;

namespace EdgeExpressDemoPOS
{
    public class DBFunctions : GeneralFunctions
    {
        #region DBAndTableCreation
        public 
            static void CreateTransactionDBTable()
        {
            try
            {
                
                string sql = "CREATE TABLE transactiondb (DUPLICATECARD varchar(10), DATE_TIME varchar(25), HOSTRESPONSECODE varchar(5), HOSTRESPONSEDESCRIPTION varchar(255), RESULT varchar(10), RESULTMSG varchar(255), APPROVEDAMOUNT varchar(10), BATCHNO varchar(10), BATCHAMOUNT varchar(10), APPROVALCODE varchar(10), ACCOUNT varchar(10), CARDHOLDERNAME varchar(25), CARDTYPE varchar(5), CARDBRAND varchar(10), CARDBRANDSHORT varchar(5), LANGUAGE varchar(10), ALIAS varchar(50), ENTRYTYPE varchar(10), RECEIPTTEXT varchar(500), EXPMONTH varchar(10), EXPYEAR varchar(10), TRANSACTIONID varchar(10), EMVTRANSACTION varchar(10))";
                SQLiteConnection m_dbConnection = new SQLiteConnection("Data Source = database.eedb;Version=3;");

                m_dbConnection.Open();
                SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
                command.ExecuteNonQuery();

                m_dbConnection.Close();
            }
            catch (Exception ex)
            {

                WriteToLog(ex.GetBaseException().ToString());
            }
        }
        public static void CreateSignatureDBTable()
        {
            try
            {
                string sql = "CREATE TABLE signaturedb (ID int, SIGNATUREFORMAT varchar(10), SIGNATUREIMAGE varchar(200), RESULT varchar(10), RESULTMSG varchar(25))";
                SQLiteConnection m_dbConnection = new SQLiteConnection("Data Source = database.eedb;Version=3;");

                m_dbConnection.Open();
                SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
                command.ExecuteNonQuery();

                m_dbConnection.Close();
            }
            catch (Exception ex)
            {
                WriteToLog(ex.GetBaseException().ToString());
            }
        }

        public static void CreateDBFile()
        {
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "database.eedb") != true)
            {
                try
                {
                    SQLiteConnection.CreateFile(AppDomain.CurrentDomain.BaseDirectory + "database.eedb");

                    CreateTransactionDBTable();
                    CreateSignatureDBTable();

                }
                catch (Exception ex)
                {
                    WriteToLog(ex.GetBaseException().ToString());
                }
            }
            else
            {
                try
                {
                    File.Delete(AppDomain.CurrentDomain.BaseDirectory + "database.eedb");

                    SQLiteConnection.CreateFile(AppDomain.CurrentDomain.BaseDirectory + "database.eedb");

                    CreateTransactionDBTable();
                    CreateSignatureDBTable();
                }
                catch (Exception ex)
                {
                    WriteToLog(ex.GetBaseException().ToString());
                }
            }
        }
        #endregion
        #region InsertQueries
        public static void InsertSaleTransaction(SaleResultXML sRX)
        {
            try
            {
                object[] args = new object[] { sRX.DUPLICATECARD, sRX.DATE_TIME, sRX.HOSTRESPONSECODE, sRX.HOSTRESPONSEDESCRIPTION, sRX.RESULT, sRX.RESULTMSG, sRX.APPROVEDAMOUNT, sRX.BATCHNO, sRX.BATCHAMOUNT, sRX.APPROVALCODE, sRX.ACCOUNT, sRX.CARDHOLDERNAME, sRX.CARDTYPE, sRX.CARDBRAND, sRX.CARDBRANDSHORT, sRX.LANGUAGE, sRX.ALIAS, sRX.ENTRYTYPE, sRX.RECEIPTTEXT, sRX.EXPMONTH, sRX.EXPYEAR, sRX.TRANSACTIONID, sRX.EMVTRANSACTION };
                string sql = string.Format("INSERT INTO transactiondb (DUPLICATECARD, DATE_TIME, HOSTRESPONSECODE, HOSTRESPONSEDESCRIPTION, RESULT, RESULTMSG, APPROVEDAMOUNT, BATCHNO, BATCHAMOUNT, APPROVALCODE, ACCOUNT, CARDHOLDERNAME, CARDTYPE, CARDBRAND, CARDBRANDSHORT, LANGUAGE, ALIAS, ENTRYTYPE, RECEIPTTEXT, EXPMONTH, EXPYEAR, TRANSACTIONID, EMVTRANSACTION) VALUES ('{0}', '{1}', {2}, '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}', '{13}', '{14}', '{15}', '{16}', '{17}', '{18}', '{19}', '{20}', '{21}', '{22}')", args);

                SQLiteConnection m_dbConnection = new SQLiteConnection("Data Source = database.eedb;Version=3;");
                m_dbConnection.Open();

                SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
                command.ExecuteNonQuery();

                m_dbConnection.Close();
            }
            catch (Exception ex)
            {
                WriteToLog(ex.GetBaseException().ToString());
            }
        }
        #endregion
        public static void InsertDebitReturnTransaction(DebitReturnResultXML dRX)
        {
            try
            {
                object[] args = new object[] { dRX.DUPLICATECARD,  dRX.DATE_TIME, dRX.HOSTRESPONSECODE, dRX.HOSTRESPONSEDESCRIPTION, dRX.RESULT, dRX.RESULTMSG, dRX.APPROVEDAMOUNT, dRX.BATCHNO, dRX.ACCOUNT, dRX.CARDHOLDERNAME, dRX.CARDTYPE, dRX.CARDBRAND, dRX.ENTRYTYPE, dRX.RECEIPTTEXT, dRX.EXPMONTH, dRX.EXPYEAR, dRX.TRANSACTIONID };
                string sql = string.Format("INSERT INTO transactiondb (DUPLICATECARD, DATE_TIME, HOSTRESPONSECODE, HOSTRESPONSEDESCRIPTION, RESULT, RESULTMSG, APPROVEDAMOUNT, BATCHNO, ACCOUNT, CARDHOLDERNAME, CARDTYPE, CARDBRAND, ENTRYTYPE, RECEIPTTEXT, EXPMONTH, EXPYEAR, TRANSACTION) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}', '{13}', '{14}', '{15}', '{16}')", args);

                SQLiteConnection m_dbConnection = new SQLiteConnection("Data Source = database.eedb;Version=3;");
                m_dbConnection.Open();

                SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
                command.ExecuteNonQuery();

                m_dbConnection.Close();

            }
            catch (Exception ex)
            {
                WriteToLog(ex.GetBaseException().ToString());
            }
        }
        public static void InsertSignatureTransaction(PromptSignatureResultXML pSRX)
        {
            try
            {
                object[] args = new object[] { Globals.Default.TransactionCounter, pSRX.SIGNATUREFORMAT, pSRX.SIGNATUREIMAGE, pSRX.RESULT, pSRX.RESULTMSG };
                string sql = string.Format("INSERT INTO signaturedb (ID, SIGNATUREFORMAT, SIGNATUREIMAGE, RESULT, RESULTMSG) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}')", args);

                SQLiteConnection m_dbConnection = new SQLiteConnection("Data Source = database.eedb;Version=3;");
                m_dbConnection.Open();

                SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
                command.ExecuteNonQuery();

                m_dbConnection.Close();

            }
            catch (Exception ex)
            {
                WriteToLog(ex.GetBaseException().ToString());
            }
        }
    }
}

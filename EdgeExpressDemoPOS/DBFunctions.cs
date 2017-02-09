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
        public static void CreateTransactionDBTable()
        {
            string sql = "CREATE TABLE transactiondb (DUPLICATECARD varchar(10), DATE_TIME varchar(25), HOSTRESPONSECODE varchar(5), HOSTRESPONSEDESCRIPTION varchar(255), RESULT varchar(10), RESULTMSG varchar(255), APPROVEDAMOUNT varchar(10), BATCHNO varchar(10), BATCHAMOUNT varchar(10), APPROVALCODE varchar(10), ACCOUNT varchar(10), CARDHOLDERNAME varchar(25), CARDTYPE varchar(5), CARDBRAND varchar(10), CARDBRANDSHORT varchar(5), LANGUAGE varchar(10), ALIAS varchar(50), ENTRYTYPE varchar(10), RECEIPTTEXT varchar(500), EXPMONTH varchar(10), EXPYEAR varchar(10), TRANSACTIONID varchar(10), EMVTRANSACTION varchar(10))";
            SQLiteConnection m_dbConnection = new SQLiteConnection("Data Source = database.eedb;Version=3;");
            try
            {
                m_dbConnection.Open();
                SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

                WriteToLog(ex.GetBaseException().ToString());
            }
            finally
            {
                m_dbConnection.Close();
            }
        }
        public static void CreateSignatureDBTable()
        {
            string sql = "CREATE TABLE signaturedb (ID varchar(20), SIGNATUREFORMAT varchar(10), SIGNATUREIMAGE varchar(200), RESULT varchar(10), RESULTMSG varchar(25))";
            SQLiteConnection m_dbConnection = new SQLiteConnection("Data Source = database.eedb;Version=3;");
            try
            {
                m_dbConnection.Open();
                SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                WriteToLog(ex.GetBaseException().ToString());
            }
            finally
            {
                m_dbConnection.Close();
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
            object[] args = new object[] { sRX.DUPLICATECARD, sRX.DATE_TIME, sRX.HOSTRESPONSECODE, sRX.HOSTRESPONSEDESCRIPTION, sRX.RESULT, sRX.RESULTMSG, sRX.APPROVEDAMOUNT, sRX.BATCHNO, sRX.BATCHAMOUNT, sRX.APPROVALCODE, sRX.ACCOUNT, sRX.CARDHOLDERNAME, sRX.CARDTYPE, sRX.CARDBRAND, sRX.CARDBRANDSHORT, sRX.LANGUAGE, sRX.ALIAS, sRX.ENTRYTYPE, sRX.RECEIPTTEXT, sRX.EXPMONTH, sRX.EXPYEAR, sRX.TRANSACTIONID, sRX.EMVTRANSACTION };
            string sql = string.Format("INSERT INTO transactiondb (DUPLICATECARD, DATE_TIME, HOSTRESPONSECODE, HOSTRESPONSEDESCRIPTION, RESULT, RESULTMSG, APPROVEDAMOUNT, BATCHNO, BATCHAMOUNT, APPROVALCODE, ACCOUNT, CARDHOLDERNAME, CARDTYPE, CARDBRAND, CARDBRANDSHORT, LANGUAGE, ALIAS, ENTRYTYPE, RECEIPTTEXT, EXPMONTH, EXPYEAR, TRANSACTIONID, EMVTRANSACTION) VALUES ('{0}', '{1}', {2}, '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}', '{13}', '{14}', '{15}', '{16}', '{17}', '', '{19}', '{20}', '{21}', '{22}')", args); //17 is ReceiptText
            //Issue with Receipt Text not Escaping properly, omitted from insert statement
            SQLiteConnection m_dbConnection = new SQLiteConnection("Data Source = database.eedb;Version=3;");
            try
            {

                m_dbConnection.Open();

                SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                WriteToLog(ex.GetBaseException().ToString());
            }
            finally
            {
                m_dbConnection.Close();
            }
        }
        public static void InsertDebitReturnTransaction(DebitReturnResultXML dRX)
        {
            object[] args = new object[] { dRX.DUPLICATECARD, dRX.DATE_TIME, dRX.HOSTRESPONSECODE, dRX.HOSTRESPONSEDESCRIPTION, dRX.RESULT, dRX.RESULTMSG, dRX.APPROVEDAMOUNT, dRX.BATCHNO, dRX.ACCOUNT, dRX.CARDHOLDERNAME, dRX.CARDTYPE, dRX.CARDBRAND, dRX.ENTRYTYPE, dRX.RECEIPTTEXT, dRX.EXPMONTH, dRX.EXPYEAR, dRX.TRANSACTIONID };
            string sql = string.Format("INSERT INTO transactiondb (DUPLICATECARD, DATE_TIME, HOSTRESPONSECODE, HOSTRESPONSEDESCRIPTION, RESULT, RESULTMSG, APPROVEDAMOUNT, BATCHNO, ACCOUNT, CARDHOLDERNAME, CARDTYPE, CARDBRAND, ENTRYTYPE, RECEIPTTEXT, EXPMONTH, EXPYEAR, TRANSACTION) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}', '{13}', '{14}', '{15}', '{16}')", args);

            SQLiteConnection m_dbConnection = new SQLiteConnection("Data Source = database.eedb;Version=3;");
            try
            {

                m_dbConnection.Open();

                SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
                command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                WriteToLog(ex.GetBaseException().ToString());
            }
            finally
            {
                m_dbConnection.Close();
            }
        }
        public static void InsertCreditReturnTransaction(CreditReturnResultXML cRX)
        {
            object[] args = new object[] { cRX.DUPLICATECARD, cRX.DATE_TIME, cRX.HOSTRESPONSECODE, cRX.HOSTRESPONSEDESCRIPTION, cRX.RESULT, cRX.RESULTMSG, cRX.APPROVEDAMOUNT, cRX.BATCHNO, cRX.ACCOUNT, cRX.CARDHOLDERNAME, cRX.CARDTYPE, cRX.CARDBRAND, cRX.ENTRYTYPE, cRX.RECEIPTTEXT, cRX.EXPMONTH, cRX.EXPYEAR, cRX.TRANSACTIONID };
            string sql = string.Format("INSERT INTO transactiondb (DUPLICATECARD, DATE_TIME, HOSTRESPONSECODE, HOSTRESPONSEDESCRIPTION, RESULT, RESULTMSG, APPROVEDAMOUNT, BATCHNO, ACCOUNT, CARDHOLDERNAME, CARDTYPE, CARDBRAND, ENTRYTYPE, RECEIPTTEXT, EXPMONTH, EXPYEAR, TRANSACTION) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}', ' ', '{14}', '{15}', '{16}')", args); //13 is ReceiptText

            SQLiteConnection m_dbConnection = new SQLiteConnection("Data Source = database.eedb;Version=3;");
            try
            {
                m_dbConnection.Open();

                SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                WriteToLog(ex.GetBaseException().ToString());
            }
            finally
            {
                m_dbConnection.Close();
            }
        }
        public static void InsertSignatureTransaction(PromptSignatureResultXML pSRX, string transactionID)
        {
            object[] args = new object[] { transactionID, pSRX.SIGNATUREFORMAT, pSRX.SIGNATUREIMAGE, pSRX.RESULT, pSRX.RESULTMSG };
            string sql = string.Format("INSERT INTO signaturedb (ID, SIGNATUREFORMAT, SIGNATUREIMAGE, RESULT, RESULTMSG) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}')", args);

            SQLiteConnection m_dbConnection = new SQLiteConnection("Data Source = database.eedb;Version=3;");
            try
            {

                m_dbConnection.Open();

                SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
                command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                WriteToLog(ex.GetBaseException().ToString());
            }
            finally
            {
                m_dbConnection.Close();
            }
        }
        public static void InsertVoidTransaction(CreditVoidResultXML cVRX)
        {
            object[] args = new object[] { cVRX.DATE_TIME, cVRX.HOSTRESPONSECODE, cVRX.HOSTRESPONSEDESCRIPTION, cVRX.RESULT, cVRX.RESULTMSG, cVRX.APPROVEDAMOUNT, cVRX.BATCHNO, cVRX.BATCHAMOUNT, cVRX.APPROVALCODE, cVRX.ACCOUNT, cVRX.CARDHOLDERNAME, cVRX.CARDTYPE, cVRX.CARDBRAND, cVRX.CARDBRANDSHORT, cVRX.LANGUAGE, cVRX.ALIAS, cVRX.ENTRYTYPE, cVRX.RECEIPTTEXT, cVRX.EXPMONTH, cVRX.EXPYEAR, cVRX.TRANSACTIONID };
            string sql = string.Format("INSERT INTO transactiondb (DUPLICATECARD, DATE_TIME, HOSTRESPONSECODE, HOSTRESPONSEDESCRIPTION, RESULT, RESULTMSG, APPROVEDAMOUNT, BATCHNO, BATCHAMOUNT, APPROVALCODE, ACCOUNT, CARDHOLDERNAME, CARDTYPE, CARDBRAND, CARDBRANDSHORT, LANGUAGE, ALIAS, ENTRYTYPE, RECEIPTTEXT, EXPMONTH, EXPYEAR, TRANSACTIONID, EMVTRANSACTION) VALUES ('{0}', '{1}', {2}, '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', '{12}', '{13}', '{14}', '{15}', '{16}', ' ', '{18}', '{19}', '{20}')", args); //17 is ReceiptText

            SQLiteConnection m_dbConnection = new SQLiteConnection("Data Source = database.eedb;Version=3;");
            try
            {
                m_dbConnection.Open();

                SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
                command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                WriteToLog(ex.GetBaseException().ToString());
            }
            finally
            {
                m_dbConnection.Close();
            }
        }
        #endregion
        #region SelectTransactionQueries
        public static CardTypeAndAmount GetTransactionType(string transactionID) //CardType Being returned is not... Reliable, Using Alias to determine Credit vs Debit on Test (CardType should be valid on Prod Gateway)
        {
            CardTypeAndAmount cTA = new CardTypeAndAmount();
            string sql = string.Format("SELECT ALIAS, APPROVEDAMOUNT FROM transactiondb WHERE TRANSACTIONID IN ('{0}')", transactionID);
            SQLiteConnection m_dbConnection = new SQLiteConnection("Data Source = database.eedb;Version=3;");
            try
            {
                m_dbConnection.Open();
                SQLiteCommand command = new SQLiteCommand(m_dbConnection);
                command.CommandText = sql;
                SQLiteDataReader sdr = command.ExecuteReader();
                while (sdr.Read())
                {
                    if (String.IsNullOrEmpty(sdr["ALIAS"].ToString()) == true)
                    {
                        cTA.cardType = "Debit";
                    }
                    else
                    {
                        cTA.cardType = "Credit";
                    }
                    cTA.amount = sdr["APPROVEDAMOUNT"].ToString();
                }
                sdr.Close();
            }
            catch (Exception ex)
            {
                WriteToLog(ex.GetBaseException().ToString());
            }
            finally
            {
                m_dbConnection.Close();

            }
            return cTA;
        }
        #endregion
    }
    public struct CardTypeAndAmount
    {
        public string cardType { get; set; }
        public string amount { get; set; }
    }
}

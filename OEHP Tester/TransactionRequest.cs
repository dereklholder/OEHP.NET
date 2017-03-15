using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OEHP_Tester
{
    public class TransactionRequest
    {
        public static string ACHParamBuilder(string accountToken, string transactionType, string chargeType,
            string entryMode, string orderID, string chargeAmount, string tcc, string customParameters) //Builds Parameters for standard ACH Post
        {
            //Verbose and Unneeded, Commented out
            //string entryModeBuilder = "entry_mode=" + entryMode;
            //string transactionTypeBuilder = "transaction_type=" + transactionType;
            //string chargeTypeBuilder = "charge_type=" + chargeType;
            //string chargeAmountBuilder = "charge_total=" + chargeAmount;
            //string orderIDBuilder = "order_id=" + orderID;
            //string accountTokenBuilder = "account_token=" + accountToken;
            //string tccBuilder = "transaction_condition_code=" + tcc;
            
            StringBuilder parameters = new StringBuilder();
            parameters.Append("account_token=" + accountToken
                        + "&transaction_type=" + transactionType
                        + "&entry_mode=" + entryMode
                        + "&charge_type=" + chargeType
                        + "&charge_total=" + chargeAmount
                        + "&order_id=" + orderID
                        + "&transaction_condition_code=" + tcc
                        + customParameters)
                        ;
            if (Globals.Default.DuplicateOn == "TRUE")
            {
                parameters.Append("&duplicate_check=CHECK");
            }
            if (Globals.Default.DuplicateOff =="TRUE")
            {
                parameters.Append("&duplicate_check=NO_CHECK");
            }
            if (chargeType == "DEBIT")
            {
                parameters.Append("&manage_payer_data=true");
            }
            GeneralFunctions.WriteToLog(parameters.ToString());
            return parameters.ToString();

        }
        public static string CreditCardParamBuilder(string accountToken, string transactionType, string chargeType,
            string entryMode, string orderID, string chargeAmount, string customParameters) //Builds Parameters for standard CreditCard Post
        {

            StringBuilder parameters = new StringBuilder();
            parameters.Append("account_token=" + accountToken
                        + "&transaction_type=" + transactionType
                        + "&entry_mode=" + entryMode
                        + "&charge_type=" + chargeType                        
                        + "&charge_total=" + chargeAmount
                        + "&order_id=" + orderID
                        + customParameters);
            if (Globals.Default.DuplicateOn == "TRUE")
            {
                parameters.Append("&duplicate_check=CHECK");
            }
            if (Globals.Default.DuplicateOff == "TRUE")
            {
                parameters.Append("&duplicate_check=NO_CHECK");
            }
            if (chargeType == "SALE" || chargeType == "AUTH")
            {
                parameters.Append("&manage_payer_data=true");
            }
            GeneralFunctions.WriteToLog(parameters.ToString());
            return parameters.ToString();

        }
        public static string CreditForceParamBuilder(string accountToken, string transactionType, string chargeType,
            string entryMode, string orderID, string chargeAmount, string approvalCode, string customParameters) //builds parameters for CreditForce Transaciton (Since it has Dependencies different than standard CreditCard Requests
        {
            StringBuilder parameters = new StringBuilder();
            parameters.Append("account_token=" + accountToken
                                + "&transaction_type=" + transactionType
                                + "&entry_mode=" + entryMode
                                + "&charge_type=" + chargeType
                                + "&charge_total=" + chargeAmount
                                + "&order_id=" + orderID
                                + "&bank_approval_code=" + approvalCode
                                + customParameters)
                                ;
            ;
            if (Globals.Default.DuplicateOn == "TRUE")
            {
                parameters.Append("&duplicate_check=CHECK");
            }
            if (Globals.Default.DuplicateOff == "TRUE")
            {
                parameters.Append("&duplicate_check=NO_CHECK");
            }
            GeneralFunctions.WriteToLog(parameters.ToString());
            return parameters.ToString();

        }
        public static string DebitCardParamBuilder(string accountToken, string transactionType, string chargeType,
            string entryMode, string orderID, string chargeAmount, string accountType, string customParameters) //Builds Parameters for Debit/Interac transactions
        {
            string accountTypeBuilder = null;
            StringBuilder parameters = new StringBuilder();
            bool usesAccountType = false;

            switch (accountType)
            {
                case "DEFAULT":
                    accountTypeBuilder = "&account_type=2";
                    usesAccountType = false;
                    break;
                case "CASH_BENEFIT":
                    accountTypeBuilder = "&account_type=3";
                    usesAccountType = true;
                    break;
                case "FOOD_STAMP":
                    accountTypeBuilder = "&account_type=4";
                    usesAccountType = true;
                    break;
                default:
                    break;
            }
            parameters.Append("account_token=" + accountToken
                                + "&transaction_type=" + transactionType
                                + "&entry_mode=" + entryMode
                                + "&charge_type=" + chargeType
                                + "&charge_total=" + chargeAmount
                                + "&order_id=" + orderID
                                + customParameters)
                                ;
            if (Globals.Default.DuplicateOn == "TRUE")
            {
                parameters.Append("&duplicate_check=CHECK");
            }
            if (Globals.Default.DuplicateOff == "TRUE")
            {
                parameters.Append("&duplicate_check=NO_CHECK");
            }
            
            if (usesAccountType == true)
            {
                parameters.Append(accountTypeBuilder);
            }
            GeneralFunctions.WriteToLog(parameters.ToString());
            return parameters.ToString();
        }
        public static string DirectPostBuilder(string accountToken, string orderID, string transactionType, string chargeType) //Build Direct Post web requests, also used for QUERYY transactions
        {
            StringBuilder parameters = new StringBuilder();
            parameters.Append("account_token=" + accountToken
                                + "&transaction_type=" + transactionType
                                + "&charge_type=" + chargeType
                                + "&order_id=" + orderID
                                + "&" + "full_detail_flag=true"); //Only Needed for Query_Payment, but causes no issues if included

            GeneralFunctions.WriteToLog(parameters.ToString());
            return parameters.ToString();
        }
        //MPD Transaction ParamBuilder
        public static string MpdBuilder(string accountToken, string orderID, string transactionType, string chargeType,
            string chargeAmount, string payer_id, string span, string transactionConditionCode, string customParameters) //Builds request for MPD (card-on-file/tokenized) transactions
        {
            StringBuilder parameters = new StringBuilder();
            parameters.Append("account_token=" + accountToken
                                + "&transaction_type=" + transactionType
                                + "&charge_type=" + chargeType
                                + "&order_id=" + orderID
                                + "&payer_identifier=" + payer_id
                                + "&span=" + span
                                + "&charge_total=" + chargeAmount
                                + "&managed_payer_data=true"
                                + "&transaction_condition_code=" + transactionConditionCode
                                + customParameters);
            if (Globals.Default.DuplicateOn == "TRUE")
            {
                parameters.Append("&duplicate_check=CHECK");
            }
            if (Globals.Default.DuplicateOff == "TRUE")
            {
                parameters.Append("&duplicate_check=NO_CHECK");
            }
            GeneralFunctions.WriteToLog(parameters.ToString());
            return parameters.ToString();
        }
        public static string MPDCheckBuilder(string accountToken, string orderID, string transactionType, string chargeType,
            string chargeAmount, string payer_id, string span, string transactionConditionCode, string customParameters) //builds parameters for MPD Check transactions
        {
            StringBuilder parameters = new StringBuilder();
            parameters.Append("account_token=" + accountToken
                                + "&transaction_type=" + transactionType
                                + "&charge_type=" + chargeType
                                + "&order_id=" + orderID
                                + "&payer_identifier=" + payer_id
                                + "&span=" + span
                                + "&charge_total=" + chargeAmount
                                + "&managed_payer_data=true"                             
                                + "&transaction_condition_code=" + transactionConditionCode
                                + customParameters);
            if (Globals.Default.DuplicateOn == "TRUE")
            {
                parameters.Append("&duplicate_check=CHECK");
            }
            if (Globals.Default.DuplicateOff == "TRUE")
            {
                parameters.Append("&duplicate_check=NO_CHECK");
            }

            GeneralFunctions.WriteToLog(parameters.ToString());
            return parameters.ToString();
        }
        public static string OrderIDRandom(int size) //Code for creating Randomized OrderIDs
        {
            Random random = new Random((int)DateTime.Now.Ticks); // Use Timestamp to Seed Random Number
            StringBuilder builder = new StringBuilder();
            Int32 ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65));
                builder.Append(ch.ToString());
            }
            return builder.ToString();
        }

    }
}

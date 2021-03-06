﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeveloperServicesPOS
{
    public class TransactionRequest
    {
        public string CreditCardParamBuilder(string accountToken, string transactionType, string chargeType,
            string entryMode, string orderID, string chargeAmount, string customParameters) // Builds Parameters for WebPost
        {

            string entryModeBuilder = "entry_mode=" + entryMode;
            string transactionTypeBuilder = "transaction_type=" + transactionType;
            string chargeTypeBuilder = "charge_type=" + chargeType;
            string chargeAmountBuilder = "charge_total=" + chargeAmount;
            string orderIDBuilder = "order_id=" + orderID;
            string accountTokenBuilder = "account_token=" + accountToken;

            StringBuilder parameters = new StringBuilder();
            parameters.Append(accountTokenBuilder
                        + "&" + transactionTypeBuilder
                        + "&" + entryModeBuilder
                        + "&" + chargeTypeBuilder
                        + "&" + chargeAmountBuilder
                        + "&" + orderIDBuilder
                        + "&" + "duplicate_check=NO_CHECK"
                        + customParameters);

            return parameters.ToString();

        }
        //Debit Card ParamBuilder
        public string DebitCardParamBuilder(string accountToken, string transactionType, string chargeType,
            string entryMode, string orderID, string chargeAmount, string accountType, string customParameters) // Builds Parameters for WebPost
        {

            string entryModeBuilder = "entry_mode=" + entryMode;
            string transactionTypeBuilder = "transaction_type=" + transactionType;
            string chargeTypeBuilder = "charge_type=" + chargeType;
            string chargeAmountBuilder = "charge_total=" + chargeAmount;
            string orderIDBuilder = "order_id=" + orderID;
            string accountTokenBuilder = "account_token=" + accountToken;
            string accountTypeStatus = accountType;
            string customParamBuilder = customParameters;
            string accountTypeBuilder = null;
            StringBuilder parameters = new StringBuilder();
            bool usesAccountType = false;

            switch (accountTypeStatus)
            {
                case "DEFAULT":
                    accountTypeBuilder = "account_type=2";
                    usesAccountType = false;
                    break;
                case "CASH_BENEFIT":
                    accountTypeBuilder = "account_type=3";
                    usesAccountType = true;
                    break;
                case "FOOD_STAMP":
                    accountTypeBuilder = "account_type=4";
                    usesAccountType = true;
                    break;
                default:
                    break;
            }
            if (usesAccountType == false)
            {
                parameters.Append(accountTokenBuilder
                                    + "&" + transactionTypeBuilder
                                    + "&" + entryModeBuilder
                                    + "&" + chargeTypeBuilder
                                    + "&" + chargeAmountBuilder
                                    + "&" + orderIDBuilder
                                    + "&" + "duplicate_check=NO_CHECK"
                                    + customParamBuilder)
                ;
            }
            if (usesAccountType == true)
            {
                parameters.Append(accountTokenBuilder
                                    + "&" + transactionTypeBuilder
                                    + "&" + entryModeBuilder
                                    + "&" + chargeTypeBuilder
                                    + "&" + chargeAmountBuilder
                                    + "&" + orderIDBuilder
                                    + "&" + "duplicate_check=NO_CHECK"
                                    + "&" + accountTypeBuilder
                                    + customParamBuilder)
                ;
            }

            return parameters.ToString();

        }
        //Direct Post Builder
        public string DirectPostBuilder(string accountToken, string orderID, string transactionType, string chargeType) //Build Direct Post
        {
            string accountTokenBuilder = "account_token=" + accountToken;
            string transactionTypeBuilder = "transaction_type=" + transactionType;
            string chargeTypeBuilder = "charge_type=" + chargeType;
            string orderIDBuilder = "order_id=" + orderID;

            StringBuilder parameters = new StringBuilder();
            parameters.Append(accountTokenBuilder
                                + "&" + transactionTypeBuilder
                                + "&" + chargeTypeBuilder
                                + "&" + orderIDBuilder
                                + "&" + "full_detail_flag=true"); //Only Needed for Query_Payment, but causes no issues if included


            return parameters.ToString();

        }
        public string OrderIDRandom(int size) //Code for creating Randomized OrderIDs
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

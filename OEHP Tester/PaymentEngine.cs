using OEHP.NET;

namespace OEHP_Tester
{
    public struct ResponseForWebBrowser //Object for Responses from PaymentEngine Classes for UI to render Browser
    {
        public string actionURL { get; set; }
        public string sealedSetupParameters { get; set; }
        public string errorMessage { get; set; }
        public string htmlDoc { get; set; }
        public string directPostResponse { get; set; }
        public string submitMethodUsed { get; set; }
    }
    public class PaymentEngine : TransactionRequest // Methods Hat are called from MainWindow to perform transactions against gateway and handle logic of Processing mode, SubmitMethod, and such.
    {
        /// <summary>
        /// Various Methods are virtually identical, however some have different arguments, and to make code clear and not have massive logic chains each is separated into separate method. Should provide better enhancement if future changes are needed.
        /// Also Separated from TransactionRequest in case those methods are altered themselves, might be a little redundant, but time will tell.
        /// Also makes workflow/logic easier to work with for future enhancement.
        /// 
        /// </summary>
        public static ResponseForWebBrowser SendToGateway(string parameters, string submitMethod) //Sends request to the gateway, Can be used without using a 'PerformXTransaction' Call if desired.
        {
            ResponseForWebBrowser response = new ResponseForWebBrowser();
            Globals.Default.PostParameters = parameters;
            switch (submitMethod)
            {
                case "PayPage Post":
                    if (Globals.Default.ProcessingMode == "Test")
                    {
                        VariableHandler.PayPageJson json = GatewayRequest.TestPayPagePost(parameters);
                        Globals.Default.SessionToken = json.sealedSetupParameters; // Sets Global Session Token variable for use in RCMStatus HTTPS Get
                        if (json.errorMessage != null)
                        {
                            response.errorMessage = json.errorMessage;
                            response.submitMethodUsed = "error";
                        }
                        else
                        {
                            response.actionURL = json.actionUrl;
                            response.sealedSetupParameters = json.sealedSetupParameters;
                            response.submitMethodUsed = "payPagePost";
                        }
                    }
                    if (Globals.Default.ProcessingMode == "Live")
                    {
                        VariableHandler.PayPageJson json = GatewayRequest.LivePayPagePost(parameters);
                        Globals.Default.SessionToken = json.sealedSetupParameters; // Sets Global Session Token variable for use in RCMStatus HTTPS Get
                        if (json.errorMessage != null)
                        {
                            response.errorMessage = json.errorMessage;
                            response.submitMethodUsed = "error";
                        }
                        else
                        {
                            response.actionURL = json.actionUrl;
                            response.sealedSetupParameters = json.sealedSetupParameters;
                            response.submitMethodUsed = "payPagePost";
                        }
                    }
                    break;
                case "HTML Doc Post":
                    //NYI
                    break;
                case "directPost":
                    if (Globals.Default.ProcessingMode == "Test")
                    {
                        response.directPostResponse = GatewayRequest.TestDirectPost(parameters);
                        response.submitMethodUsed = "directPost";
                    }
                    if (Globals.Default.ProcessingMode == "Live")
                    {
                        response.directPostResponse = GatewayRequest.LiveDirectPost(parameters);
                        response.submitMethodUsed = "directPost";
                    }
                    break;
            }
            return response;

        }
        #region Credit_Card Transactions
        public static ResponseForWebBrowser PerformCreditSaleTransaction(string accountToken, string transactionType, string chargeType, string entryMode, string orderID, string amount, string customParameters, string submitMethod) //Returns String of PayPage URL or String of HTML Doc
        {
            string parameters = TransactionRequest.CreditCardParamBuilder(accountToken, transactionType, chargeType, entryMode, orderID, amount, customParameters);
            ResponseForWebBrowser response = SendToGateway(parameters, submitMethod);
            return response;
            
        }
        public static ResponseForWebBrowser PerformAuthTransaction(string accountToken, string transactionType, string chargeType, string entryMode, string orderID, string amount, string customParameters, string submitMethod)
        {
            string parameters = TransactionRequest.CreditCardParamBuilder(accountToken, transactionType, chargeType, entryMode, orderID, amount, customParameters);
            ResponseForWebBrowser response = SendToGateway(parameters, submitMethod);
            return response;
        }
        public static ResponseForWebBrowser PerformSignatureTransaction(string accountToken, string transactionType, string chargeType, string entryMode, string orderID, string amount, string customParameters, string submitMethod)
        {
            string parameters = TransactionRequest.CreditCardParamBuilder(accountToken, transactionType, chargeType, entryMode, orderID, amount, customParameters);
            ResponseForWebBrowser response = SendToGateway(parameters, submitMethod);
            return response;
        }
        public static ResponseForWebBrowser PerformCreditVoidTransaction(string accountToken, string transactionType, string chargeType, string orderID)
        {
            string parameters = TransactionRequest.DirectPostBuilder(accountToken, orderID, transactionType, chargeType);
            ResponseForWebBrowser response = SendToGateway(parameters, "directPost");
            return response;
        }
        public static ResponseForWebBrowser PerformCreditForceTransaction(string accountToken, string transactionType, string chargeType, string entryMode, string orderID, string amount, string approvalCode, string customParameters, string submitMethod)
        {
            string parameters = TransactionRequest.CreditForceParamBuilder(accountToken, transactionType, chargeType, entryMode, orderID, amount, approvalCode, customParameters);
            ResponseForWebBrowser response = SendToGateway(parameters, submitMethod);
            return response;

        }
        public static ResponseForWebBrowser PerformCreditCaptureTransaction(string accountToken, string transactionType, string chargeType, string entryMode, string orderID, string amount, string customParameters)
        {
            string parameters = TransactionRequest.CreditCardParamBuilder(accountToken, transactionType, chargeType, entryMode, orderID, amount, customParameters);
            ResponseForWebBrowser response = SendToGateway(parameters, "directPost");
            return response;
        }
        public static ResponseForWebBrowser PerformCreditQueryPaymentTransaction(string accountToken, string transactionType, string chargeType, string entryMode, string orderID, string customParameters)
        {
            string parameters = TransactionRequest.CreditCardParamBuilder(accountToken, transactionType, chargeType, entryMode, orderID, "", customParameters);
            ResponseForWebBrowser response = SendToGateway(parameters, "directPost");
            return response;
        }
        public static ResponseForWebBrowser PerformCreditAdjustmentTransaction(string accountToken, string transactionType, string chargeType, string entryMode, string orderID, string amount, string customParameters)
        {
            string parameters = TransactionRequest.CreditCardParamBuilder(accountToken, transactionType, chargeType, entryMode, orderID, amount, customParameters);
            ResponseForWebBrowser response = SendToGateway(parameters, "directPost");
            return response;
        }
        public static ResponseForWebBrowser PerformCreditIndependentCreditTransaction(string accountToken, string transactionType, string chargeType, string entryMode, string orderID, string amount, string customParameters, string submitMethod)
        {
            string parameters = CreditCardParamBuilder(accountToken, transactionType, chargeType, entryMode, orderID, amount, customParameters);
            ResponseForWebBrowser response = SendToGateway(parameters, submitMethod);
            return response;
        }
        public static ResponseForWebBrowser PerformCreditDependentCreditTransaction(string accountToken, string transactionType, string chargeType, string entryMode, string orderID, string amount, string customParameters)
        {
            string parameters = CreditCardParamBuilder(accountToken, transactionType, chargeType, entryMode, orderID, amount, customParameters);
            ResponseForWebBrowser response = SendToGateway(parameters, "directPost");
            return response;
        }
        #endregion
        #region Debit_Card Transactions
        public static ResponseForWebBrowser PerformDebitPurchaseTransaction(string accountToken, string transactionType, string chargeType, string entryMode, string orderID, string amount, string accountType, string customParameters, string submitMethod)
        {
            string parameters = TransactionRequest.DebitCardParamBuilder(accountToken, transactionType, chargeType, entryMode, orderID, amount, accountType, customParameters);
            ResponseForWebBrowser response = SendToGateway(parameters, submitMethod);
            return response;
        }
        public static ResponseForWebBrowser PerformDebitRefundTransaction(string accountToken, string transactionType, string chargeType, string entryMode, string orderID, string amount, string accountType, string customParameters, string submitMethod)
        {
            string parameters = TransactionRequest.DebitCardParamBuilder(accountToken, transactionType, chargeType, entryMode, orderID, amount, accountType, customParameters);
            ResponseForWebBrowser response = SendToGateway(parameters, submitMethod);
            return response;
        }
        public static ResponseForWebBrowser PerformDebitQueryTransaction(string accountToken, string transactionType, string chargeType, string entryMode, string orderID, string amount, string customParameters)
        {
            string parameters = TransactionRequest.CreditCardParamBuilder(accountToken, transactionType, chargeType, entryMode, orderID, "", customParameters);
            ResponseForWebBrowser response = SendToGateway(parameters, "directPost");
            return response;
        }
        #endregion
        #region ACH Transactions
        public static ResponseForWebBrowser PerformACHDebitTransaction(string accountToken, string transactionType, string chargeType, string entryMode, string orderID, string amount, string TCC, string customParameters, string submitMethod)
        {
            string parameters = TransactionRequest.ACHParamBuilder(accountToken, transactionType, chargeType, entryMode, orderID, amount, TCC, customParameters);
            ResponseForWebBrowser response = SendToGateway(parameters, submitMethod);
            return response;
        }
        public static ResponseForWebBrowser PerformACHCreditTransaction(string accountToken, string transactionType, string chargeType, string entryMode, string orderID, string amount, string TCC, string customParameters, string submitMethod)
        {
            string parameters = ACHParamBuilder(accountToken, transactionType, chargeType, entryMode, orderID, amount, TCC, customParameters);
            ResponseForWebBrowser response = SendToGateway(parameters, submitMethod);
            return response;
        }
        public static ResponseForWebBrowser PerformACHDependentCreditTransaction(string accountToken, string transactionType, string chargeType, string entryMode, string orderID, string amount, string customParameters, string submitMethod)
        {
            string parameters = ACHParamBuilder(accountToken, transactionType, chargeType, entryMode, orderID, amount, "51", customParameters);
            ResponseForWebBrowser response = SendToGateway(parameters, "directPost");
            return response;
        }
        public static ResponseForWebBrowser PerformACHQueryTransaction(string accountToken, string transactionType, string chargeType, string entryMode, string orderID, string amount, string customParameters)
        {
            string parameters = TransactionRequest.CreditCardParamBuilder(accountToken, transactionType, chargeType, entryMode, orderID, "", customParameters);
            ResponseForWebBrowser response = SendToGateway(parameters, "directPost");
            return response;
        }

        #endregion
    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OEHP.NET.Test
{
    [TestClass]
    public class DataManipulationTest
    {
        //QueryStringToJsonUnitTests
        [TestMethod]
        public void QueryStringToJsonConvertsToJson()
        {
            //Arrage
            DataManipulation dm = new DataManipulation();
            string queryString = "foo=bar&bar=foo";
            string expected = "{\"foo\": \"bar\"";
            //Act
            string result = dm.QueryStringToJson(queryString);
            //Assert
            StringAssert.Contains(result, expected);
        }
        [TestMethod]
        public void QueryStringToJsonReturnsException()
        {
            //Arrange
            DataManipulation dm = new DataManipulation();
            string queryString = "blarg";
            string expected = "Invalid QueryString";
            //Act
            string result = dm.QueryStringToJson(queryString);
            //Assert
            Assert.AreEqual(expected, result);
        }
        [TestMethod]
        public void QueryResultObjectHasData()
        {
            //Arrange
            DataManipulation dm = new DataManipulation();
            string resultQueryString = "response_code=1&response_code_text=Successful transaction: The transaction completed successfully.&secondary_response_code=0&time_stamp=1461692608166&retry_recommended=false&authorized_amount=1.00&bin=541333&captured_amount=1.00&original_authorized_amount=1.00&requested_amount=1.00&state=payment_deposited&time_stamp_created=1461699803000&initial_action=SALE&original_response_code=1&original_response_code_text=Successful transaction: The transaction completed successfully.&original_secondary_response_code=0&time_stamp_updated=1461699803000&merchant_receipt=%0ABatch+%23+++++++++++++000130%0ATrans+ID++++++++++++000000001807%0AOrder+ID++++++++++++7672657888859079%0ATrans+Type++++++++++Purchase%0ADate%2FTime+++++++++++2016-04-26+12%3A43%3A23%0ACard+Type+++++++++++MasterCard%0ACard+Number+++++++++XXXXXXXXXXXX4111%0AEntry+Method++++++++Manual%0AApproval+Code+++++++005485%0A%0ATotal+Amount++++++++USD%241.00%0A%0A++++++++++Approved+-+Thank+You%0A%0A+X____________________________________%0A++++++++++Cardholder+Signature%0A%0ABuyer+agrees+to+pay+total+amount+above%0Aaccording+to+cardholder%26%2339%3Bs+agreement+with%0Aissuer.%0A%0A+++++++++****Merchant+Copy****&customer_receipt=%0ABatch+%23+++++++++++++000130%0ATrans+ID++++++++++++000000001807%0AOrder+ID++++++++++++7672657888859079%0ATrans+Type++++++++++Purchase%0ADate%2FTime+++++++++++2016-04-26+12%3A43%3A23%0ACard+Type+++++++++++MasterCard%0ACard+Number+++++++++XXXXXXXXXXXX4111%0AEntry+Method++++++++Manual%0AApproval+Code+++++++005485%0A%0ATotal+Amount++++++++USD%241.00%0A%0A++++++++++Approved+-+Thank+You%0A%0A+X____________________________________%0A++++++++++Cardholder+Signature%0A%0ABuyer+agrees+to+pay+total+amount+above%0Aaccording+to+cardholder%26%2339%3Bs+agreement+with%0Aissuer.%0A%0A+++++++++****Customer+Copy****&receipt_approval_code_label=Approval+Code&receipt_approval_code=005485&receipt_authorization_agreement=Buyer+agrees+to+pay+total+amount+above+according+to+cardholder%26%2339%3Bs+agreement+with+issuer.&receipt_batch_number_label=Batch+%23&receipt_batch_number=000130&receipt_card_number_label=Card+Number&receipt_card_number=XXXXXXXXXXXX4111&receipt_card_type_label=Card+Type&receipt_card_type=MasterCard&receipt_entry_method_label=Entry+Method&receipt_entry_method=Manual&receipt_order_id_label=Order+ID&receipt_order_id=7672657888859079&receipt_signature_line=X____________________________________&receipt_signature_text=Cardholder+Signature&receipt_total_amount_label=Total+Amount&receipt_total_amount=USD%241.00&receipt_transaction_date_time_label=Date%2FTime&receipt_transaction_date_time=2016-04-26+12%3A43%3A23&receipt_transaction_id_label=Trans+ID&receipt_transaction_id=000000001807&receipt_transaction_type_label=Trans+Type&receipt_transaction_type=Purchase&receipt_verbiage=Approved+-+Thank+You&receipt_merchant_copy_label=****Merchant+Copy****&receipt_customer_copy_label=****Customer+Copy****&bank_approval_code=005485&expire_month=4&expire_year=16&order_id=7672657888859079&payer_identifier=3oYRf8xS0T&reference_id=0&span=4111&card_brand=MASTERCARD&batch_id=000130&";
            string resultJson = dm.QueryStringToJson(resultQueryString);
            string expected = "MASTERCARD";
            //Act
            VariableHandler.QueryResultJson QRO = new VariableHandler.QueryResultJson();
            QRO = dm.QueryResultObject(resultJson);

            //Assert
            Assert.AreEqual(expected, QRO.card_brand);
            Assert.AreEqual("1", QRO.response_code);
        }
        [TestMethod]
        public void QueryResultObjectNullData()
        {
            //Arrange
            DataManipulation dm = new DataManipulation();
            string resultJson = null;
            //Act
            VariableHandler.QueryResultJson QRO = new VariableHandler.QueryResultJson();
            QRO = dm.QueryResultObject(resultJson);

            //Assert
            StringAssert.Contains(QRO.exceptionData, "Exception");
        }

    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OEHP.NET.Test
{
	[TestClass]
    public class GatewayRequestTest
    {
		//TestPayPage Unit Tests
        [TestMethod]
		public void TestPayPageReturnsPayPageURL()
        {
            //Arrange
            GatewayRequest gr = new GatewayRequest();
            string parameters = "account_token=04173F8DCE65520D3580E5FF8555A961CECF249E46B5C2FAEFA04E248CD95FEA9D55BB581758D0591B&transaction_type=CREDIT_CARD&entry_mode=EMV&charge_type=SALE&charge_total=1&order_id=6787667675907888&duplicate_check=NO_CHECK&prompt_signature=true&manage_payer_data=true&show_emv_receipt=false";
            //Act
            string result = gr.TestPayPagePost(parameters);
            //Assert
            StringAssert.Contains(result, "https://ws.test.paygateway.com/HostPayService/v1/hostpay/paypage/");
        }
		[TestMethod]
		public void TestPayPageReturnsError()
        {
            //Arrage
            GatewayRequest gr = new GatewayRequest();
            string parameters = "account_token=InvalidAccountToken&transaction_type=CREDIT_CARD&entry_mode=EMV&charge_type=SALE&charge_total=1&order_id=6787667675907888&duplicate_check=NO_CHECK&prompt_signature=true&manage_payer_data=true&show_emv_receipt=false";
            //Act
            string result = gr.TestPayPagePost(parameters);
            //Assert
            StringAssert.Contains(result, "Account Status is not set");
        }
		[TestMethod]
		public void TestPayPageExceptionHandling()
        {
			//Arrange
			GatewayRequest gr = new GatewayRequest();
            string parameters = null;
            //Act
            string result = gr.TestPayPagePost(parameters);
            //Assert
            StringAssert.Contains(result, "Exception");
        }
		[TestMethod]
		public void TestPayPageInvalidField()
        {
            //Arrage
            GatewayRequest gr = new GatewayRequest();
            string parameters = "account_token=InvalidAccountToken&transaction_type=CREDIT_&entry_mode=EMV&charge_type=SALE&charge_total=1&order_id=6787667675907888&duplicate_check=NO_CHECK&prompt_signature=true&manage_payer_data=true&show_emv_receipt=false";
            //Act
            string result = gr.TestPayPagePost(parameters);
            //Assert
            StringAssert.Contains(result, "Invalid Field:");
        }
        //TestDirectPost Unit tests
        [TestMethod]
        public void TestDirectPostReturnsSuccess()
        {
            //Arrange
            GatewayRequest gr = new GatewayRequest();
            string parameters = "account_token=04173F8DCE65520D3580E5FF8555A961CECF249E46B5C2FAEFA04E248CD95FEA9D55BB581758D0591B&transaction_type=CREDIT_CARD&charge_type=QUERY_PAYMENT&order_id=6787667675907888&full_detail_flag=true";
            //Act
            string result = gr.TestDirectPost(parameters);
            //Assert
            StringAssert.Contains(result, "Successful transaction: The transaction completed successfully.");
        }

		
    }
}

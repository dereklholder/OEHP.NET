using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EdgeExpressCloudWeb
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            //string parameters = EdgeEngine.CreditCardSaleXML("1.00");
            //string result = EdgeEngine.SendToEdgeExpressCloud(parameters);

            string parameters = EdgeEngine.PromptCreditDebitXML();
            string result = EdgeEngine.SendToEdgeExpressCloud(parameters);
            RcmResult rr = EdgeEngine.GetDebitOrCreditResult(result);
            ResultTextPH.Text = rr.RCMResponse.XLinkEMVResult.DEBITORCREDIT;
        }
    }
}
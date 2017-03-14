using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Specialized;

namespace OEHP.NET
{
    public class GatewayRequest : VariableHandler
    {
        #region TestPostMethods
        //Posts to TEST
        public static PayPageJson TestPayPagePost(string parameters) //returns Object that contains SealedSetupParameters, ActionURL, and ErrorMessage as properties.
        {
            try
            {
                WebRequest oehpRequest = WebRequest.Create(TestPayPagePostURL);

                string postData = parameters;
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);

                oehpRequest.ContentType = "application/x-www-form-urlencoded";
                oehpRequest.Method = "POST";
                oehpRequest.ContentLength = byteArray.Length;

                Stream dataStream = oehpRequest.GetRequestStream();

                dataStream.Write(byteArray, 0, byteArray.Length);

                dataStream.Close();

                WebResponse oehpResponse = oehpRequest.GetResponse();

                dataStream = oehpResponse.GetResponseStream();

                StreamReader reader = new StreamReader(dataStream);

                string responseFromOEHP = reader.ReadToEnd();

                reader.Close();
                oehpResponse.Close();

                PayPageJson jsonResponse = new PayPageJson();
                jsonResponse = JsonConvert.DeserializeObject<PayPageJson>(responseFromOEHP);

                return jsonResponse;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static string TestDirectPost(string parameters) //Returns raw result from direct OEHP
        {
            try
            {

                WebRequest oehpRequest = WebRequest.Create(TestDirectPostURL);

                string postData = parameters;
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);

                oehpRequest.ContentType = "application/x-www-form-urlencoded";
                oehpRequest.Method = "POST";
                oehpRequest.ContentLength = byteArray.Length;

                Stream dataStream = oehpRequest.GetRequestStream();

                dataStream.Write(byteArray, 0, byteArray.Length);

                dataStream.Close();

                WebResponse oehpResponse = oehpRequest.GetResponse();

                dataStream = oehpResponse.GetResponseStream();

                StreamReader reader = new StreamReader(dataStream);

                string responseFromOEHP = reader.ReadToEnd();

                reader.Close();
                oehpResponse.Close();

                return responseFromOEHP;


            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
        public static string TestHtmlDocPost(string parameters)  //Performs HTMLDOc Post, returns HTML doc as String
        {
            try
            {
                WebRequest oehpRequest = WebRequest.Create(TestHtmlDocPostURL);

                string postData = parameters;
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);

                oehpRequest.ContentType = "application/x-www-form-urlencoded";
                oehpRequest.Method = "POST";
                oehpRequest.ContentLength = byteArray.Length;

                Stream dataStream = oehpRequest.GetRequestStream();

                dataStream.Write(byteArray, 0, byteArray.Length);

                dataStream.Close();

                WebResponse oehpResponse = oehpRequest.GetResponse();

                dataStream = oehpResponse.GetResponseStream();

                StreamReader reader = new StreamReader(dataStream);

                string responseFromOEHP = reader.ReadToEnd();

                reader.Close();
                oehpResponse.Close();

                return responseFromOEHP;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }

        }
        #endregion
        #region LivePostMethods
        //Posts to PROD
        public static PayPageJson LivePayPagePost(string parameters)
        {
            try
            {
                WebRequest oehpRequest = WebRequest.Create(LivePayPagePostURL);

                string postData = parameters;
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);

                oehpRequest.ContentType = "application/x-www-form-urlencoded";
                oehpRequest.Method = "POST";
                oehpRequest.ContentLength = byteArray.Length;

                Stream dataStream = oehpRequest.GetRequestStream();

                dataStream.Write(byteArray, 0, byteArray.Length);

                dataStream.Close();

                WebResponse oehpResponse = oehpRequest.GetResponse();

                dataStream = oehpResponse.GetResponseStream();

                StreamReader reader = new StreamReader(dataStream);

                string responseFromOEHP = reader.ReadToEnd();

                reader.Close();
                oehpResponse.Close();

                PayPageJson jsonResponse = new PayPageJson();
                jsonResponse = JsonConvert.DeserializeObject<PayPageJson>(responseFromOEHP);

                return jsonResponse;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static string LiveDirectPost(string parameters) //Returns raw result from direct OEHP, will need to construct wrapper to handle result data.
        {
            try
            {
                
                WebRequest oehpRequest = WebRequest.Create(LiveDirectPostURL);

                string postData = parameters;
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);

                oehpRequest.ContentType = "application/x-www-form-urlencoded";
                oehpRequest.Method = "POST";
                oehpRequest.ContentLength = byteArray.Length;

                Stream dataStream = oehpRequest.GetRequestStream();

                dataStream.Write(byteArray, 0, byteArray.Length);

                dataStream.Close();

                WebResponse oehpResponse = oehpRequest.GetResponse();

                dataStream = oehpResponse.GetResponseStream();

                StreamReader reader = new StreamReader(dataStream);

                string responseFromOEHP = @reader.ReadToEnd();

                reader.Close();
                oehpResponse.Close();

                return responseFromOEHP;


            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
        public static string LiveHtmlDocumentPost(string parameters)  //Not Fully IMplemented
        {
            try
            {
                WebRequest oehpRequest = WebRequest.Create(LiveHtmlDocPostURL);

                string postData = parameters;
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);

                oehpRequest.ContentType = "application/x-www-form-urlencoded";
                oehpRequest.Method = "POST";
                oehpRequest.ContentLength = byteArray.Length;

                Stream dataStream = oehpRequest.GetRequestStream();

                dataStream.Write(byteArray, 0, byteArray.Length);

                dataStream.Close();

                WebResponse oehpResponse = oehpRequest.GetResponse();

                dataStream = oehpResponse.GetResponseStream();

                StreamReader reader = new StreamReader(dataStream);

                string responseFromOEHP = reader.ReadToEnd();

                reader.Close();
                oehpResponse.Close();

                return responseFromOEHP;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }

        }
#endregion
    }

}

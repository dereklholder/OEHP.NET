﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using System.IO;

namespace OEHP.NET
{
    /// <summary>
    /// Methods for manipulation of data for ease of use, mostly conversion/Deserialization
    /// </summary>
    public class DataManipulation : VariableHandler
    {
        //Used for Converting QueryString to Json
        public string QueryStringToJson(string queryString)
        {
            try
            {
                queryString = queryString.Remove(queryString.Length - 1); // Query String from query Payment contains & character at the end, removes this to prevent null exceptions
                NameValueCollection keyPairs = HttpUtility.ParseQueryString(queryString);
                keyPairs.AllKeys.Where(k => !String.IsNullOrEmpty(k)).ToDictionary(k => k, k => keyPairs[k]);
                Dictionary<string, string> dictData = new Dictionary<string, string>(keyPairs.Count);
                foreach (string key in keyPairs.AllKeys)
                {
                    dictData.Add(key, keyPairs.Get(key));
                }
                //Convert Dictionary to Json
                var entries = dictData.Select(d => string.Format("\"{0}\": \"{1}\"", d.Key, string.Join(",", d.Value)));
                return "{" + string.Join(", \n", entries) + "}";
            }
            catch (Exception)
            {
                return "Invalid QueryString";

            }
        }
        //Query Result into object. Result must be changed into JSON before serializing.
        public QueryResultJson QueryResultObject(string json)
        {

            try
            {
                QueryResultJson QRO = new QueryResultJson();
                QRO = JsonConvert.DeserializeObject<QueryResultJson>(json);
                return QRO;
            }
            catch (Exception ex)
            {
                QueryResultJson QRO = new QueryResultJson();
                QRO.exceptionData = ex.ToString();
                return QRO;
            }
            
        }
        public PayPageCSS PayPageCSSObject(string json)
        {
            try
            {
                PayPageCSS PPC = new PayPageCSS();
                PPC = JsonConvert.DeserializeObject<PayPageCSS>(json);
                return PPC;
            }
            catch (Exception ex)
            {
                PayPageCSS PPC = new PayPageCSS();
                PPC.exceptionData = ex.ToString();
                return PPC;
            }
        }
        public ReceiptDataOnly ReceiptDataObject(string json)
        {
            try
            {
                ReceiptDataOnly RDO = new ReceiptDataOnly();
                RDO = JsonConvert.DeserializeObject<ReceiptDataOnly>(json);
                return RDO;
            }
            catch (Exception ex)
            {
                ReceiptDataOnly RDO = new ReceiptDataOnly();
                return RDO;
            }
        }
    }
}

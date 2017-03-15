﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OEHP.NET
{
    public class VariableHandler
    {

        #region URLs of Requests to OEHP API
        //Test OEHP URLs
        public static string TestPayPagePostURL = "https://ws.test.paygateway.com/HostPayService/v1/hostpay/transactions"; 
        public static string TestDirectPostURL = "https://ws.test.paygateway.com/api/v1/transactions";
        public static string TestHtmlDocPostURL = "https://ws.test.paygateway.com/HostPayService/v1/hostpay/paypage/";
        public static string TestRcmStatusURL = "https://ws.test.paygateway.com/HostPayService/v1/hostpay/transactions/status/";

        //Live OEHP Urls
        public static string LivePayPagePostURL = "https://ws.paygateway.com/HostPayService/v1/hostpay/transactions";
        public static string LiveDirectPostURL = "https://ws.paygateway.com/api/v1/transactions";
        public static string LiveHtmlDocPostURL = "https://ws.paygateway.com/HostPayService/v1/hostpay/paypage/";
        public static string LiveRcmStatusURL = "https://ws.paygateway.com/HostPayService/v1/hostpay/transactions/status/";
        #endregion
        #region Object Classes for deserialization      
        public struct PayPageCSS //Object for PayPage CSS Fields
        {
            public string font_family { get; set; }
            public string font_size { get; set; }
            public string color { get; set; }
            public string background_color { get; set; }
            public string btn_background_color { get; set; }
            public string btn_color { get; set; }
            public string btn_width { get; set; }
            public string btn_height { get; set; }
            public string btn_border_top_left_radius { get; set; }
            public string btn_border_top_right_radius { get; set; }
            public string btn_border_bottom_left_radius { get; set; }
            public string btn_border_bottom_right_radius { get; set; }
            public string btn_border_color { get; set; }
            public string btn_border_style { get; set; }
            public string section_header_font_size { get; set; }
            public string line_spacing_size { get; set; }
            public string input_field_height { get; set; }

            public string exceptionData { get; set; }

        }       
        public struct PayPageJson //Object for deserializing the JSOn returned from PayPage POST
        {
            public string sealedSetupParameters { get; set; }
            public string actionUrl { get; set; }
            public string errorMessage { get; set; }
        }
        public static string SessionToken { get; set; }   
        public struct QueryResultJson//Object for deserializing result of Query transactions.
        {
            //General Transaction data
            public string response_code { get; set; }
            public string response_code_text { get; set; }
            public string secondary_response_code { get; set; }
            public string secondary_response_code_text { get; set; }
            public string time_stamp { get; set; }
            public string retry_recommended { get; set; }
            public string authorized_amount { get; set; }
            public string bin { get; set; }
            public string captured_amount { get; set; }
            public string original_authorized_amount { get; set; }
            public string requested_amount { get; set; }
            public string time_stamp_created { get; set; }
            public string original_response_code { get; set; }
            public string original_response_code_text { get; set; }
            public string original_secondary_response_code { get; set; }
            public string original_secondary_response_code_text { get; set; }
            public string time_stamp_updated { get; set; }
            public string merchant_receipt { get; set; }
            public string customer_receipt { get; set; }
            public string state { get; set; }
            public string bank_approval_code { get; set; }
            public string expire_month { get; set; }
            public string expire_year { get; set; }
            public string order_id { get; set; }
            public string payer_identifier { get; set; }
            public string reference_id { get; set; }
            public string span { get; set; }
            public string card_brand { get; set; }
            public string batch_id { get; set; }

            //Receipt Tags
            public string receipt_application_cryoptogram_label { get; set; }
            public string receipt_application_cryptogram { get; set; }
            public string receipt_application_identifier_label { get; set; }
            public string receipt_application_identifier { get; set; }
            public string receipt_application_preferred_name { get; set; }
            public string receipt_application_preferred_name_label { get; set; }
            public string receipt_application_transaction_counter_label { get; set; }
            public string receipt_application_transaction_counter { get; set; }
            public string receipt_approval_code_label { get; set; }
            public string receipt_approval_code { get; set; }
            public string receipt_authorization_response_code_label { get; set; }
            public string receipt_authorization_response_code { get; set; }
            public string receipt_card_number_label { get; set; }
            public string receipt_card_number { get; set; }
            public string receipt_card_type_label { get; set; }
            public string receipt_card_type { get; set; }
            public string receipt_entry_legend_label { get; set; }
            public string receipt_entry_legend { get; set; }
            public string receipt_entry_method_label { get; set; }
            public string receipt_entry_method { get; set; }
            public string receipt_line_items { get; set; }
            public string receipt_merchant_id_label { get; set; }
            public string receipt_merchant_id { get; set; }
            public string receipt_order_id_label { get; set; }
            public string receipt_order_id { get; set; }
            public string receipt_signature_text { get; set; }
            public string receipt_terminal_verification_results_label { get; set; }
            public string receipt_terminal_verification_results { get; set; }
            public string receipt_total_amount_label { get; set; }
            public string receipt_total_amount { get; set; }
            public string receipt_transaction_date_time_label { get; set; }
            public string receipt_transaction_date_time { get; set; }
            public string receipt_transaction_id_label { get; set; }
            public string receipt_transaction_id { get; set; }
            public string receipt_transaction_reference_number_label { get; set; }
            public string receipt_transaction_reference_number { get; set; }
            public string receipt_transaction_status_information_label { get; set; }
            public string receipt_transaction_type_label { get; set; }
            public string receipt_transaction_type { get; set; }
            public string receipt_validation_code_label { get; set; }
            public string receipt_validation_code { get; set; }
            public string receipt_verbiage { get; set; }
            public string receipt_merchant_copy_label { get; set; }
            public string receipt_customer_copy_label { get; set; }
            
            public string exceptionData { get; set; } //If an Exception Occurs when Deserializing, puts exception string here.

        }
        public struct ReceiptDataOnly
        {
            public string receipt_application_cryoptogram_label { get; set; }
            public string receipt_application_cryptogram { get; set; }
            public string receipt_application_identifier_label { get; set; }
            public string receipt_application_identifier { get; set; }
            public string receipt_application_preferred_name { get; set; }
            public string receipt_application_preferred_name_label { get; set; }
            public string receipt_application_transaction_counter_label { get; set; }
            public string receipt_application_transaction_counter { get; set; }
            public string receipt_approval_code_label { get; set; }
            public string receipt_approval_code { get; set; }
            public string receipt_authorization_response_code_label { get; set; }
            public string receipt_authorization_response_code { get; set; }
            public string receipt_authorization_agreement { get; set; }
            public string receipt_card_number_label { get; set; }
            public string receipt_card_number { get; set; }
            public string receipt_card_type_label { get; set; }
            public string receipt_card_type { get; set; }
            public string receipt_entry_legend_label { get; set; }
            public string receipt_entry_legend { get; set; }
            public string receipt_entry_method_label { get; set; }
            public string receipt_entry_method { get; set; }
            public string receipt_line_items { get; set; }
            public string receipt_merchant_id_label { get; set; }
            public string receipt_merchant_id { get; set; }
            public string receipt_order_id_label { get; set; }
            public string receipt_order_id { get; set; }
            public string receipt_signature_text { get; set; }
            public string receipt_signature_line { get; set; }
            public string receipt_terminal_verification_results_label { get; set; }
            public string receipt_terminal_verification_results { get; set; }
            public string receipt_total_amount_label { get; set; }
            public string receipt_total_amount { get; set; }
            public string receipt_transaction_date_time_label { get; set; }
            public string receipt_transaction_date_time { get; set; }
            public string receipt_transaction_id_label { get; set; }
            public string receipt_transaction_id { get; set; }
            public string receipt_transaction_reference_number_label { get; set; }
            public string receipt_transaction_reference_number { get; set; }
            public string receipt_transaction_status_information_label { get; set; }
            public string receipt_transaction_status_information { get; set; }
            public string receipt_transaction_type_label { get; set; }
            public string receipt_transaction_type { get; set; }
            public string receipt_validation_code_label { get; set; }
            public string receipt_validation_code { get; set; }
            public string receipt_verbiage { get; set; }
            public string receipt_merchant_copy_label { get; set; }
            public string receipt_customer_copy_label { get; set; }
        }
    }
#endregion
    //Legacy Variable, to remove when no longer needed.
    public static class SSP
    {
        public static string SessionToken { get; set; }
    }
}

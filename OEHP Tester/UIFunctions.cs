using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OEHP_Tester
{
    //Various Functions to get Data/Values for UI Elements.

    public class UICollections
    {
        public static ObservableCollection<string> SubmitMethodBoxValues()
        {
            ObservableCollection<string> values = new ObservableCollection<string>();
            values.Add("PayPage Post");
            values.Add("HTML Doc Post");
            return values;
        }
        public static ObservableCollection<string> ModeBoxValues()
        {
            ObservableCollection<string> values = new ObservableCollection<string>();
            values.Add("Test");
            values.Add("Live");
            return values;
        }
        public static ObservableCollection<string> TransactionTypeValues()
        {
            ObservableCollection<string> values = new ObservableCollection<string>();
            values.Add("CREDIT_CARD");
            values.Add("DEBIT_CARD");
            values.Add("CREDIT_DEBIT_CARD");
            values.Add("ACH");
            values.Add("INTERAC");
            return values;
        }
        public static ObservableCollection<string> TCCValues()
        {
            ObservableCollection<string> values = new ObservableCollection<string>();
            values.Add("50");
            values.Add("51");
            values.Add("52");
            values.Add("53");
            return values;
        }
        public static ObservableCollection<string> CreditChargeTypeValues()
        {
            ObservableCollection<string> values = new ObservableCollection<string>();
            values.Add("SALE");
            values.Add("CREDIT");
            values.Add("VOID");
            values.Add("FORCE_SALE");
            values.Add("AUTH");
            values.Add("CAPTURE");
            values.Add("ADJUSTMENT");
            values.Add("SIGNATURE");
            values.Add("QUERY_PAYMENT");
            return values;
        }
        public static ObservableCollection<string> DebitChargeTypeValues()
        {
            ObservableCollection<string> values = new ObservableCollection<string>();
            values.Add("PURCHASE");
            values.Add("REFUND");
            values.Add("QUERY_PURCHASE");
            return values;
        }
        public static ObservableCollection<string> CreditDebitChargeTypeValues()
        {
            ObservableCollection<string> values = new ObservableCollection<string>();
            values.Add("SALE");
            return values;
        }
        public static ObservableCollection<string> ACHChargeTypeValues()
        {
            ObservableCollection<string> values = new ObservableCollection<string>();
            values.Add("DEBIT");
            values.Add("CREDIT");
            values.Add("VOID");
            values.Add("QUERY");
            return values;
        }
        public static ObservableCollection<string> AccountTypeValues()
        {
            ObservableCollection<string> values = new ObservableCollection<string>();
            values.Add("DEFAULT");
            values.Add("CASH_BENEFIT");
            values.Add("FOOD_STAMP");
            return values;
        }
        public static ObservableCollection<string> CreditTypeValues() // Not actual parameters, merely determine workflow and logic.
        {
            ObservableCollection<string> values = new ObservableCollection<string>();
            values.Add("INDEPENDENT");
            values.Add("DEPENDENT");
            return values;
        }
        public static ObservableCollection<string> CreditEntryModeValues()
        {
            ObservableCollection<string> values = new ObservableCollection<string>();
            values.Add("KEYED");
            values.Add("EMV");
            values.Add("AUTO");
            values.Add("HID");
            return values;
        }
        public static ObservableCollection<string> DebitEntryModeValues()
        {
            ObservableCollection<string> values = new ObservableCollection<string>();
            values.Add("EMV");
            values.Add("HID");
            return values;
        }
        public static ObservableCollection<string> ACHEntryModeValues()
        {
            ObservableCollection<string> values = new ObservableCollection<string>();
            values.Add("KEYED");
            return values;
        }
    }

}

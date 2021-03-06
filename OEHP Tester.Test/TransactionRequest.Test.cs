﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OEHP_Tester.Test
{
    [TestClass]
    public class TransactionRequest : OEHP_Tester.TransactionRequest
    {
        [TestMethod]
        public void ACHParamBuilderFormatsCorrectly()
        {
            //Arrange
            OEHP_Tester.TransactionRequest tr = new OEHP_Tester.TransactionRequest();
            string expected = "account_token=foo&transaction_type=bar&entry_mode=foo&charge_type=bar&charge_total=foo&order_id=bar&transaction_condition_code=foo&foo=bar&bar=foo";
            string foo = "foo";
            string bar = "bar";
            //Act
            string result = tr.ACHParamBuilder(foo, bar, bar, foo, bar, foo, foo, "&foo=bar&bar=foo");
            //Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void CreditCardParamBuilderFormatsCorrect()
        {
            //Arrange
            OEHP_Tester.TransactionRequest tr = new OEHP_Tester.TransactionRequest();
            string expected = "account_token=foo&transaction_type=bar&entry_mode=foo&charge_type=bar&charge_total=foo&order_id=bar&foo=bar&bar=foo";
            string foo = "foo";
            string bar = "bar";
            //ACT
            string result = tr.CreditCardParamBuilder(foo, bar, bar, foo, bar, foo, "&foo=bar&bar=foo");
            //Assert
            Assert.AreEqual(expected, result);
        }
        [TestMethod]
        public void CreditForceParamBuilderFormatsCorrect()
        {
            //Arrage
            OEHP_Tester.TransactionRequest tr = new OEHP_Tester.TransactionRequest();
            string expected = "account_token=foo&transaction_type=bar&entry_mode=foo&charge_type=bar&charge_total=foo&order_id=bar&bank_approval_code=foo&foo=bar";
            string foo = "foo";
            string bar = "bar";
            //Act
            string result = tr.CreditForceParamBuilder(foo, bar, bar, foo, bar, foo, foo, "&foo=bar");
            //Assert
            Assert.AreEqual(expected, result);
        }
    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using ConciliaPraGente.Domain;

namespace ConciliaPraGente.Testes.Domain
{
    [TestClass]
    public class BankTransactionFactoryTest
    {
        [TestMethod]
        public void ShouldNotCreateWithInvalidTransactionText()
        {
            const string invalidText = @"
                < TRNTYPE > CREDIT
                < DTPOSTED > 20140204100000[-03:EST]
                < TRNAMT > 435.00
                < MEMO > DOC 399.1934NIBO SOF CUR";
            var factory = new BankTransactionFactory();

            Assert.ThrowsException<Exception>(() => factory.CreateBankTransactionWith(invalidText),
                "Invalid transaction text");
        }

        [TestMethod]
        public void ShouldCreateWithViewModelValues()
        {
            var factory = new BankTransactionFactory();
            const string dateInText = "04/02/2014 10:00:00";
            const string valueInText = "R$ 14.000,00";
            const string typeInText = "CRÉDITO";
            const string expectedDescription = "DOC 399.1934NIBO SOF CUR";
            const int expectedValue = 14000;
            var expectedDate = new DateTime(2014, 2, 4, 10, 0, 0);
            const TransactionType expectedType = TransactionType.Credit;

            var bankTransaction = factory.CreateBankTransactionWith(expectedDescription, dateInText, valueInText, typeInText);

            Assert.AreEqual(expectedValue, bankTransaction.Value);
            Assert.AreEqual(expectedDescription, bankTransaction.Description);
            Assert.AreEqual(expectedDate, bankTransaction.Date);
            Assert.AreEqual(expectedType, bankTransaction.Type);
        }
    }
}

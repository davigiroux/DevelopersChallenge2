using System;
using ConciliaPraGente.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConciliaPraGente.Testes.Domain
{
    [TestClass]
    public class BankTransactionTest
    {
        [TestMethod]
        public void ShouldCreateATransaction()
        {
           const TransactionType transactionType = TransactionType.Credit;
           const decimal value = 3000m;
           var date = DateTime.Today;
           const string description = "Transaction test description";

           var transaction = new BankTransaction(description, date, value, transactionType);

           Assert.AreEqual(transaction.Description, description);
           Assert.AreEqual(transaction.Date, date);
           Assert.AreEqual(transaction.Value, value);
           Assert.AreEqual(transaction.Type, transactionType);
        }

        [TestMethod]
        public void ShouldNotCreateWithInvalidValue()
        {
            const TransactionType transactionType = TransactionType.Credit;
            const decimal value = -100;
            var date = DateTime.Today;
            const string description = "Transaction test description";

            Assert.ThrowsException<Exception>(() => new BankTransaction(description, date, value, transactionType), "Invalid value for transaction");
        }

        [TestMethod]
        public void ShouldNotCreateWithInvalidDescription()
        {
            const TransactionType transactionType = TransactionType.Credit;
            const decimal value = 100;
            var date = DateTime.Today;
            const string description = "";

            Assert.ThrowsException<Exception>(() => new BankTransaction(description, date, value, transactionType), "Invalid description for transaction");
        }

        [TestMethod]
        public void ShouldNotCreateWithAFutureDate()
        {
            const TransactionType transactionType = TransactionType.Credit;
            const decimal value = 100;
            var date = DateTime.Today.AddMonths(5);
            const string description = "Transaction test description";

            Assert.ThrowsException<Exception>(() => new BankTransaction(description, date, value, transactionType), "Transaction should not be in the future");
        }
    }
}

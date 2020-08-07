using System;
using System.Drawing;

namespace ConciliaPraGente.Domain
{
    public class Transaction
    {
        public string Description { get; }
        public DateTime Date { get; }
        public decimal Value { get; }
        public TransactionType Type { get; }

        public Transaction(string description, DateTime date, decimal value, TransactionType transactionType)
        {
            Validate(description, date, value, transactionType);

            Description = description;
            Date = date;
            Value = value;
            Type = transactionType;
        }

        private static void Validate(string description, DateTime date, decimal value, TransactionType transactionType)
        {
            if (value <= 0)
                throw new Exception("Invalid value for transaction");
            if (description.Length <= 0)
                throw new Exception("Invalid description for transaction");
            if (date > DateTime.Today)
                throw new Exception("Transaction should not be in the future");
        }
    }
}
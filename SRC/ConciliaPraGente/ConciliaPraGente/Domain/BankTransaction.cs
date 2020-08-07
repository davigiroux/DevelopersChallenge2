using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConciliaPraGente.Domain
{
    [Table("BankTransaction")]
    public partial class BankTransaction
    {
        public int Id { get; set; }

        [StringLength(100)]
        public string Description { get; set; }

        public decimal Value { get; set; }

        public DateTime Date { get; set; }

        public TransactionType Type { get; set; }

        public BankTransaction(){}
        public BankTransaction(string description, DateTime date, decimal value, TransactionType transactionType)
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

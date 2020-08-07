using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ConciliaPraGente.Domain
{
    public class BankTransactionFactory : IBankTransactionFactory
    {
        public BankTransaction CreateBankTransactionWith(string transactionInText)
        {
            Validate(transactionInText);

            var transactionPropertiesInStrings = CleanTransactionText(transactionInText);

            var transactionTypeText = GetPropertyValue(transactionPropertiesInStrings, "<TRNTYPE>");
            var dateInText = GetPropertyValue(transactionPropertiesInStrings, "<DTPOSTED>");
            var valueInText = GetPropertyValue(transactionPropertiesInStrings, "<TRNAMT>");
            var description = GetPropertyValue(transactionPropertiesInStrings, "<MEMO>");

            var transactionType = GetTransactionTypeOfText(transactionTypeText);
            var date = GetDateOfText(dateInText);
            var value = Decimal.Parse(valueInText);
            value = value < 0 ? value * -1 : value;

            var transaction = new BankTransaction(description, date, value, transactionType);

            return transaction;
        }


        private static TransactionType GetTransactionTypeOfText(string transactionTypeText)
        {
            return transactionTypeText == "DEBIT" ? TransactionType.Debit : transactionTypeText == "CREDIT" ? TransactionType.Credit : TransactionType.Unknown;
        }

        private static void Validate(string transactionInText)
        {
            var isAValidTransactionText = transactionInText.Contains("<TRNTYPE>") &&
                                          transactionInText.Contains("<DTPOSTED>") &&
                                          transactionInText.Contains("<TRNAMT>") &&
                                          transactionInText.Contains("<MEMO>");
            if (!isAValidTransactionText)
                throw new Exception("Invalid transaction text");
        }

        private static IEnumerable<string> CleanTransactionText(string transactionInText)
        {
            string[] lineBreakerSeparator = {"\n"};
            var tempTransaction = transactionInText
                .Split(lineBreakerSeparator, transactionInText.Length, StringSplitOptions.None)
                .Where(t => t.Length > 0 && t != "\n");
            return tempTransaction;
        }

        private DateTime GetDateOfText(string dateInText)
        {
            var year = int.Parse(dateInText.Substring(0, 4));
            var month = int.Parse(dateInText.Substring(4, 2));
            var day = int.Parse(dateInText.Substring(6, 2));
            var hour = int.Parse(dateInText.Substring(8, 2));
            var minute = int.Parse(dateInText.Substring(10, 2));
            var second = int.Parse(dateInText.Substring(12, 2));

            return new DateTime(year, month, day, hour, minute, second);
        }

        private static string GetPropertyValue(IEnumerable<string> tempTransaction, string propertyTag)
        {
            string[] propertySeparator = {propertyTag};
            var textLineOfTheProp = tempTransaction.FirstOrDefault(t => t.Contains(propertyTag));

            var valueOfTheProp = textLineOfTheProp.Split(propertySeparator, textLineOfTheProp.Length, StringSplitOptions.None)[1];

            return valueOfTheProp;
        }

        public BankTransaction CreateBankTransactionWith(string description, string formattedDateTime, string formattedValue, string transactionTypeInText)
        {
            var date = DateTime.ParseExact(formattedDateTime, "dd/MM/yyyy HH:mm:ss", CultureInfo.CurrentCulture);

            var valueInText = "";
            var charsToRemove = new string[] { "R", "$", ".", " " };

            valueInText = charsToRemove.Aggregate(formattedValue, (current, c) => current.Replace(c, string.Empty));
            var value = decimal.Parse(valueInText);

            var type = transactionTypeInText == "CRÉDITO" ? TransactionType.Credit :
                transactionTypeInText == "DÉBITO" ? TransactionType.Debit : TransactionType.Unknown;

            return new BankTransaction(description.TrimEnd(), date, value, type);
        }
    }
}
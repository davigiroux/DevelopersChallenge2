namespace ConciliaPraGente.Domain
{
    public interface IBankTransactionFactory
    {
        BankTransaction CreateBankTransactionWith(string transactionInText);

        BankTransaction CreateBankTransactionWith(string description, string formattedDateTime, string formattedValue, string transactionTypeInText);
    }
}
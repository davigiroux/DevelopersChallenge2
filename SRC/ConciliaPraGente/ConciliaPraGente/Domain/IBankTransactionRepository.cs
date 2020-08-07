using System.Collections.Generic;

namespace ConciliaPraGente.Domain
{
    public interface IBankTransactionRepository
    {
        void SaveBankTransactions(IEnumerable<BankTransaction> bankTransactions);
        IEnumerable<BankTransaction> GetAll();
    }
}
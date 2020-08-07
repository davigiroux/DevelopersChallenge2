using ConciliaPraGente.Domain;
using System.Collections.Generic;
using System.Linq;

namespace ConciliaPraGente.Infra
{
    public class BankTransactionRepository : IBankTransactionRepository
    {
        private readonly TransactionContext _transactionContext;

        public BankTransactionRepository()
        {
            _transactionContext = new TransactionContext();
        }

        public void SaveBankTransactions(IEnumerable<BankTransaction> bankTransactions)
        {
            _transactionContext.BankTransaction.AddRange(bankTransactions);

            _transactionContext.SaveChanges();
        }

        public IEnumerable<BankTransaction> GetAll()
        {
            return _transactionContext.BankTransaction.ToList();
        }
    }
}
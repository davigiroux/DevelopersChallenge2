using ConciliaPraGente.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ConciliaPraGente.Controllers;
using ConciliaPraGente.Infra;

namespace ConciliaPraGente.Application
{
    public class BankTransactionManager
    {
        private readonly IBankTransactionFactory _bankTransactionFactory;
        private readonly IBankTransactionsFileToTextConverter _bankTransactionsFileToTextConverter;
        private readonly IBankTransactionRepository _bankTransactionRepository;

        public BankTransactionManager(IBankTransactionFactory bankTransactionFactory, IBankTransactionsFileToTextConverter bankTransactionsFileToTextConverter, IBankTransactionRepository bankTransactionRepository)
        {
            _bankTransactionFactory = bankTransactionFactory;
            _bankTransactionsFileToTextConverter = bankTransactionsFileToTextConverter;
            _bankTransactionRepository = bankTransactionRepository;
        }

        public List<BankTransaction> ExtractBankTransactionsFromFiles(HttpFileCollectionBase files)
        {
            var transactionList = new List<BankTransaction>();

            for (var index = 0; index < files.Count; index++)
            {
                var file = files[index];
                var bankTransactionList = GetBankTransactionFromFile(file);

                transactionList.AddRange(bankTransactionList);
            }

            return transactionList;
        }

        private IEnumerable<BankTransaction> GetBankTransactionFromFile(HttpPostedFileBase file)
        {
            var inputStream = file.InputStream;
            var rawTransactionsInText = _bankTransactionsFileToTextConverter.ConvertFrom(inputStream);
            var validTransactionTexts = rawTransactionsInText.Where(t =>
            {
                var isAValidTransactionText = t.Contains("<TRNTYPE>") &&
                                              t.Contains("<DTPOSTED>") &&
                                              t.Contains("<TRNAMT>") &&
                                              t.Contains("<MEMO>");

                return isAValidTransactionText;
            });

            var bankTransactionList = validTransactionTexts.Select(bankTransactionInText => _bankTransactionFactory.CreateBankTransactionWith(bankTransactionInText));

            return bankTransactionList;
        }

        public void AddBankTransactions(IEnumerable<BankTransactionViewModel> listOfViewModels)
        {
            var bankTransactions = new List<BankTransaction>();
            var validBankTransactionsToBeAdded = RemoveDuplicates(listOfViewModels, bankTransactions);

            _bankTransactionRepository.SaveBankTransactions(validBankTransactionsToBeAdded);
        }

        private IEnumerable<BankTransaction> RemoveDuplicates(IEnumerable<BankTransactionViewModel> listOfViewModels, List<BankTransaction> bankTransactions)
        {
            var listWithoutDuplicates = listOfViewModels.Distinct(new BankTransactionViewModelEqualityComparer());
            bankTransactions.AddRange(listWithoutDuplicates.Select(vm => 
                _bankTransactionFactory.CreateBankTransactionWith(
                vm.Description, 
                vm.Date, 
                vm.Value,
                vm.Type))
            );

            var addedBankTransactions = _bankTransactionRepository.GetAll();

            return bankTransactions.Where(bt => !addedBankTransactions.Any(addedBt => addedBt.Description.Trim() == bt.Description.Trim()
            && addedBt.Date == bt.Date
            && addedBt.Type == bt.Type
            && addedBt.Value == bt.Value));
        }
    }
}
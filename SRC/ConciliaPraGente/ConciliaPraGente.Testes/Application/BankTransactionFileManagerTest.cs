using ConciliaPraGente.Application;
using ConciliaPraGente.Controllers;
using ConciliaPraGente.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;

namespace ConciliaPraGente.Testes.Application
{
    [TestClass]
    public class BankTransactionManagerTest
    {
        private Mock<IBankTransactionFactory> _factoryMock;
        private Mock<IBankTransactionsFileToTextConverter> _converterMock;
        private Mock<IBankTransactionRepository> _repositoryMock;
        private BankTransactionManager _bankTransactionManager;

        [TestInitialize]
        public void Setup()
        {
            _factoryMock = new Mock<IBankTransactionFactory>();
            _converterMock = new Mock<IBankTransactionsFileToTextConverter>();
            _repositoryMock = new Mock<IBankTransactionRepository>();
            _bankTransactionManager =
                new BankTransactionManager(_factoryMock.Object, _converterMock.Object, _repositoryMock.Object);

        }

        [TestMethod]
        public void ShouldCreateABankTransactionWithViewModelData()
        {
            var bankTransactionViewModel = new BankTransactionViewModel
            {
                Description = "DOC 399.1934NIBO SOF CUR",
                Date = "04/02/2014 10:00:00",
                Type = "CRÉDITO",
                Value = "R$ 14.000,00"
            };
            var listOfViewModels = new List<BankTransactionViewModel> {bankTransactionViewModel};

            _bankTransactionManager.AddBankTransactions(listOfViewModels);

            _factoryMock.Verify(
                factory => factory.CreateBankTransactionWith(bankTransactionViewModel.Description,
                    bankTransactionViewModel.Date, bankTransactionViewModel.Value, bankTransactionViewModel.Type),
                Times.Exactly(listOfViewModels.Count));
        }

        [TestMethod]
        public void ShouldSaveBankTransaction()
        {
            var bankTransactionViewModel = new BankTransactionViewModel
            {
                Description = "DOC 399.1934NIBO SOF CUR",
                Date = "04/02/2014 10:00:00",
                Type = "CRÉDITO",
                Value = "R$ 14.000,00"
            };
            var listOfViewModels = new List<BankTransactionViewModel> { bankTransactionViewModel };
            var bankTransaction = SetupForValidTransactions(listOfViewModels);
            var bankTransactionList = new List<BankTransaction>{bankTransaction};

            _bankTransactionManager.AddBankTransactions(listOfViewModels);

            _repositoryMock.Verify(repository => repository.SaveBankTransactions(bankTransactionList), Times.Once);
        }

        [TestMethod]
        public void ShouldNotCreateDuplicateBankTransaction()
        {
            var bankTransactionViewModel = SetupForDuplicateTransactions(out var listOfDuplicateViewModels);

            _bankTransactionManager.AddBankTransactions(listOfDuplicateViewModels);

            _factoryMock.Verify(
                factory => factory.CreateBankTransactionWith(bankTransactionViewModel.Description,
                    bankTransactionViewModel.Date, bankTransactionViewModel.Value, bankTransactionViewModel.Type),
                Times.Once);
        }

        private BankTransactionViewModel SetupForDuplicateTransactions(out List<BankTransactionViewModel> listOfDuplicateViewModels)
        {
            var bankTransactionViewModel = new BankTransactionViewModel
            {
                Description = "DOC 399.1934NIBO SOF CUR",
                Date = "04/02/2014 10:00:00",
                Type = "CRÉDITO",
                Value = "R$ 14.000,00"
            };
            listOfDuplicateViewModels = new List<BankTransactionViewModel> {bankTransactionViewModel, bankTransactionViewModel};
            const string expectedDescription = "DOC 399.1934NIBO SOF CUR";
            const int expectedValue = 14000;
            var expectedDate = new DateTime(2014, 2, 4, 10, 0, 0);
            const TransactionType expectedType = TransactionType.Credit;
            var bankTransaction = new BankTransaction(expectedDescription, expectedDate, expectedValue, expectedType);
            _factoryMock.Setup(factory => factory.CreateBankTransactionWith(bankTransactionViewModel.Description,
                    bankTransactionViewModel.Date, bankTransactionViewModel.Value, bankTransactionViewModel.Type))
                .Returns(bankTransaction);
            return bankTransactionViewModel;
        }

        private BankTransaction SetupForValidTransactions(IReadOnlyList<BankTransactionViewModel> listOfDuplicateViewModels)
        {
            var bankTransactionViewModel = listOfDuplicateViewModels[0];
            const string description = "DOC 399.1934NIBO SOF CUR";
            const int value = 14000;
            var date = new DateTime(2014, 2, 4, 10, 0, 0);
            const TransactionType type = TransactionType.Credit;
            var bankTransaction = new BankTransaction(description, date, value, type);
            _factoryMock.Setup(factory => factory.CreateBankTransactionWith(bankTransactionViewModel.Description,
                    bankTransactionViewModel.Date, bankTransactionViewModel.Value, bankTransactionViewModel.Type))
                .Returns(bankTransaction);
            return bankTransaction;
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Mvc;
using ConciliaPraGente.Application;
using ConciliaPraGente.Domain;
using ConciliaPraGente.Infra;
using Microsoft.EntityFrameworkCore.Internal;

namespace ConciliaPraGente.Controllers
{
    public class BankTransactionController : Controller
    {
        private readonly BankTransactionManager _manager;
        private readonly BankTransactionRepository _bankTransactionRepository;

        public BankTransactionController()
        {
            _manager = new BankTransactionManager(new BankTransactionFactory(), new BankTransactionsFileToTextConverter(), new BankTransactionRepository());
            _bankTransactionRepository = new BankTransactionRepository();
        }

        public ActionResult Index()
        {
            var bankTransactions = _bankTransactionRepository.GetAll();
            CreateBankTransactionViewModelList(bankTransactions);

            return View();
        }

        private void CreateBankTransactionViewModelList(IEnumerable<BankTransaction> bankTransactions)
        {
            ViewBag.BankTransactions = bankTransactions.Select(t => new BankTransactionViewModel
            {
                Description = t.Description,
                Date = t.Date.ToString("dd/MM/yyyy HH:mm:ss"),
                Value = t.Value.FormatMonetaryOrZeroInReal(),
                Type = t.Type.ToDescription()
            });
            ViewBag.TotalTransactions = bankTransactions.Count();
        }

        public ActionResult UploadBankTransactionFile()
        {
            ViewBag.TotalTransactions = 0;

            return View("UploadViewTransactionFile");
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult Upload()
        {
            var files = Request.Files;

            var areFilesValid = true;
            for (var index = 0; index < files.Count; index++)
            {
                var file = files[index];
                var extension = Path.GetExtension(file.FileName);
                if (extension != ".ofx")
                    areFilesValid = false;
            }

            if (!areFilesValid)
            {
                ViewBag.ErrorMessage = "Arquivo inválido para upload. Deve ser do tipo .OFX";
                return View("UploadViewTransactionFile");
            }

            var transactionList = _manager.ExtractBankTransactionsFromFiles(files);
            CreateBankTransactionViewModelList(transactionList);

            return View("UploadViewTransactionFile");
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult SaveBankTransactions(IEnumerable<BankTransactionViewModel> bankTransactions)
        {
            _manager.AddBankTransactions(bankTransactions);

            return Json(new
            {
                message = "success"
            });
        }
    }
}
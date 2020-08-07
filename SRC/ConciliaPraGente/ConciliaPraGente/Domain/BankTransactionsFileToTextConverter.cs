using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace ConciliaPraGente.Domain
{
    public class BankTransactionsFileToTextConverter : IBankTransactionsFileToTextConverter
    {
        public string[] ConvertFrom(Stream inputStream)
        {
            var fileLength = (int) inputStream.Length;
            var fileInBytes = new byte[fileLength];
            var count = inputStream.Read(fileInBytes, 0, fileLength);
            var textFromFile = Encoding.UTF8.GetString(fileInBytes, 0, count);
            string[] separatorTagForTransaction = { "<STMTTRN>", "</STMTTRN>" };

            var rawTransactionsInText = textFromFile.Split(separatorTagForTransaction, fileLength, StringSplitOptions.None);

            return rawTransactionsInText;
        }
    }

    public interface IBankTransactionsFileToTextConverter
    {
        string[] ConvertFrom(Stream inputStream);
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ConciliaPraGente.Controllers;

namespace ConciliaPraGente.Domain
{
    public class BankTransactionViewModelEqualityComparer : IEqualityComparer<BankTransactionViewModel>
    {
        public bool Equals(BankTransactionViewModel x, BankTransactionViewModel y)
        {
            return x != null && y != null && (x.Description == y.Description
                                 && x.Date == y.Date
                                 && x.Value == y.Value
                                 && x.Type == y.Type);
        }

        public int GetHashCode(BankTransactionViewModel obj)
        {
            return obj.Description.GetHashCode();
        }
    }
}
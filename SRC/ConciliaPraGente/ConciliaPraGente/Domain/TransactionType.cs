using System.ComponentModel;

namespace ConciliaPraGente.Domain
{
    public enum TransactionType
    {
        [Description("Débito")]
        Debit,
        [Description("Crédito")]
        Credit,
        [Description("Desconhecido")]
        Unknown

    }
}
using System.Data.Entity;
using ConciliaPraGente.Domain;

namespace ConciliaPraGente.Infra
{
    public partial class TransactionContext : DbContext
    {
        public TransactionContext() : base("name=TransactionContext")
        {
        }

        public virtual DbSet<BankTransaction> BankTransaction { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BankTransaction>()
                .Property(e => e.Description)
                .IsFixedLength();

            modelBuilder.Entity<BankTransaction>()
                .Property(e => e.Value)
                .HasPrecision(18, 0);
        }
    }
}

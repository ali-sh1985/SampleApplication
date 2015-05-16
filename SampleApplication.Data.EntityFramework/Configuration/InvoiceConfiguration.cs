using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using SampleApplication.Domain.Entities;

namespace SampleApplication.Data.EntityFramework.Configuration
{
    class InvoiceConfiguration : EntityTypeConfiguration<Invoice>
    {
        public InvoiceConfiguration()
        {
            ToTable("Invoice");

            HasKey(x => x.InvoiceId)
                .Property(x => x.InvoiceId)
                .HasColumnName("InvoiceId")
                .HasColumnType("int")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity)
                .IsRequired();

            Property(x => x.Date)
                .HasColumnName("Date")
                .HasColumnType("datetime")
                .IsRequired();

            HasRequired(x => x.Client)
                .WithMany(x => x.InvoiceList)
                .HasForeignKey(x => x.ClientId);

            HasMany(x => x.ItemList)
                .WithRequired(x => x.Invoice)
                .HasForeignKey(x => x.InvoiceId);

            HasMany(x => x.PaymentList)
                .WithRequired(x => x.Invoice)
                .HasForeignKey(x => x.InvoiceId);
        }
    }
}

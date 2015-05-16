using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using SampleApplication.Domain.Entities;

namespace SampleApplication.Data.EntityFramework.Configuration
{
    class PaymentConfiguration : EntityTypeConfiguration<Payment> 
    {
        public PaymentConfiguration()
        {
            ToTable("Payment");

            HasKey(x => x.PaymentId)
                .Property(x => x.PaymentId)
                .HasColumnName("PaymentId")
                .HasColumnType("int")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity)
                .IsRequired();

            Property(x => x.PaymentDate)
                .HasColumnName("PaymentDate")
                .IsRequired();

            Property(x => x.Method)
                .HasColumnName("Method")
                .HasColumnType("tinyint")
                .IsRequired();

            Property(x => x.Total)
                .HasColumnName("Total")
                .HasColumnType("money")
                .IsRequired();

            Property(x => x.Description)
                .HasColumnName("Description")
                .HasColumnType("nvarchar")
                .IsMaxLength()
                .IsOptional();

            HasRequired(x => x.Invoice)
                .WithMany(x => x.PaymentList)
                .HasForeignKey(x => x.InvoiceId);
        }
    }
}

using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using SampleApplication.Domain.Entities;

namespace SampleApplication.Data.EntityFramework.Configuration
{
    class ClientConfiguration : EntityTypeConfiguration<Client> 
    {
        public ClientConfiguration()
        {
            ToTable("Client");

            HasKey(x => x.ClientId)
                .Property(x => x.ClientId)
                .HasColumnName("ClientId")
                .HasColumnType("int")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity)
                .IsRequired();

            Property(x => x.Name)
                .HasColumnName("Name")
                .HasColumnType("nvarchar")
                .HasMaxLength(250)
                .IsRequired();

            Property(x => x.Town)
                .HasColumnName("Town")
                .HasColumnType("nvarchar")
                .HasMaxLength(250)
                .IsRequired();

            Property(x => x.Country)
                .HasColumnName("Country")
                .HasColumnType("nvarchar")
                .HasMaxLength(250)
                .IsRequired();

            Property(x => x.AddressLine1)
                .HasColumnName("AddressLine1")
                .HasColumnType("nvarchar")
                .HasMaxLength(500)
                .IsRequired();

            Property(x => x.AddressLine2)
                .HasColumnName("AddressLine2")
                .HasColumnType("nvarchar")
                .HasMaxLength(500)
                .IsOptional();

            Property(x => x.Postcode)
                .HasColumnName("Postcode")
                .HasColumnType("nvarchar")
                .HasMaxLength(15)
                .IsRequired();

            HasMany(x => x.InvoiceList)
                .WithRequired(x => x.Client)
                .HasForeignKey(x => x.ClientId);
        }
        
    }
}

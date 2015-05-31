using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using SampleApplication.Domain.Entities;

namespace SampleApplication.Data.EntityFramework.Configuration
{
    class ItemConfiguration : EntityTypeConfiguration<Item> 
    {
        public ItemConfiguration()
        {
            ToTable("Item");

            HasKey(x => x.ItemId)
                .Property(x => x.ItemId)
                .HasColumnName("ItemId")
                .HasColumnType("int")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity)
                .IsRequired();

            Property(x => x.Description)
                .HasColumnName("Description")
                .HasColumnType("nvarchar")
                .IsMaxLength()
                .IsOptional();

            Property(x => x.Net)
                .HasColumnName("Net")
                .HasColumnType("money")
                .IsRequired();

            Property(x => x.Tax)
                .HasColumnName("Tax")
                .HasColumnType("int")
                .IsRequired();

            Ignore(x => x.IsDeleted);

            HasRequired(x => x.Invoice)
                .WithMany(x => x.ItemList)
                .HasForeignKey(x => x.InvoiceId);
        }
    }
}

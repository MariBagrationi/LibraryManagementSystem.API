using LibraryManagementSystem.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryManagementSystem.Persistance.Configurations
{
    public class BorrowRecordConfiguration : IEntityTypeConfiguration<BorrowRecord>
    {
        public void Configure(EntityTypeBuilder<BorrowRecord> builder)
        {
            //builder filters - 
            //builder.HasQueryFilter(x => x.IsDelete == false);

            //ignore filters - repo

            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Patron)
                .WithMany(x => x.BorrowRecords)
                .HasForeignKey(x => x.PatronId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Book)
                .WithMany(x => x.BorrowRecords)
                .HasForeignKey(x => x.BookId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

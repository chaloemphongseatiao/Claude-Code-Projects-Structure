using Microsoft.EntityFrameworkCore;

using TemplateWebService.Models.Entities;

namespace TemplateWebService.Data
{
    public class LomaLottoContext : DbContext
    {
        public LomaLottoContext(DbContextOptions<LomaLottoContext> options) : base(options) { }

        public DbSet<Attachment> Attachments => Set<Attachment>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Attachment>(e =>
            {
                e.ToTable("Attachments");
                e.HasKey(x => x.FileId);

                e.HasIndex(x => x.FilePath).IsUnique().HasDatabaseName("UQ_Attachments_FilePath");

                e.Property(x => x.FileId).ValueGeneratedOnAdd();
                e.Property(x => x.OriginalName).HasMaxLength(255).IsRequired();
                e.Property(x => x.FileName).HasMaxLength(255).IsRequired();
                e.Property(x => x.FilePath).HasMaxLength(512).IsRequired();
                e.Property(x => x.FileExtension).HasMaxLength(10).IsUnicode(false).IsRequired();
                e.Property(x => x.ContentType).HasMaxLength(100).IsUnicode(false).IsRequired();
                e.Property(x => x.FileSize).IsRequired();
                e.Property(x => x.IsActive).HasDefaultValue(false).IsRequired();
                e.Property(x => x.CreatedBy).HasMaxLength(100).IsRequired();
                e.Property(x => x.CreatedDate).HasDefaultValueSql("SYSDATETIME()").ValueGeneratedOnAdd().IsRequired();
            });
        }
    }
}
using Microsoft.EntityFrameworkCore;
using transactions.portal.services.Models;

namespace transactions.portal.services.Data.Sql;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Transaction> Transactions => Set<Transaction>();
    public DbSet<Dispute> Disputes => Set<Dispute>();
    public DbSet<DisputeStatusHistory> DisputeStatusHistories => Set<DisputeStatusHistory>();
    public DbSet<Refund> Refunds => Set<Refund>();
    public DbSet<TransactionEvent> TransactionEvents => Set<TransactionEvent>();
    public DbSet<MerchantEntry> MerchantEntries => Set<MerchantEntry>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasMaxLength(64);
            entity.Property(e => e.Reference).HasMaxLength(64).IsRequired();
            entity.Property(e => e.Amount).HasPrecision(18, 2);
            entity.Property(e => e.Merchant).HasMaxLength(200).IsRequired();
            entity.Property(e => e.Status).HasMaxLength(50).IsRequired();
            entity.Property(e => e.PaymentBrand).HasMaxLength(50).IsRequired();
            entity.Property(e => e.CardLast4).HasMaxLength(4).IsRequired();
            entity.Property(e => e.CustomerId).HasMaxLength(64).IsRequired();

            entity.HasIndex(e => e.CustomerId);
            entity.HasIndex(e => e.Reference).IsUnique();
            entity.HasIndex(e => e.Status);
            entity.HasIndex(e => e.Date);

            entity.HasMany(e => e.Events)
                  .WithOne(e => e.Transaction)
                  .HasForeignKey(e => e.TransactionId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(e => e.Disputes)
                  .WithOne(e => e.Transaction)
                  .HasForeignKey(e => e.TransactionId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Refund)
                  .WithOne(e => e.Transaction)
                  .HasForeignKey<Refund>(e => e.TransactionId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.MerchantEntry)
                  .WithOne(e => e.Transaction)
                  .HasForeignKey<MerchantEntry>(e => e.TransactionId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Dispute>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasMaxLength(64);
            entity.Property(e => e.TransactionId).HasMaxLength(64).IsRequired();
            entity.Property(e => e.Reason).HasMaxLength(200).IsRequired();
            entity.Property(e => e.Description).HasMaxLength(1000).IsRequired();
            entity.Property(e => e.Status).HasMaxLength(50).IsRequired();
            entity.Property(e => e.CustomerId).HasMaxLength(64).IsRequired();

            entity.HasIndex(e => e.TransactionId);
            entity.HasIndex(e => e.CustomerId);
            entity.HasIndex(e => e.Status);
            entity.HasIndex(e => e.CreatedAt);

            entity.HasMany(e => e.StatusHistory)
                  .WithOne(e => e.Dispute)
                  .HasForeignKey(e => e.DisputeId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<DisputeStatusHistory>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasMaxLength(64);
            entity.Property(e => e.DisputeId).HasMaxLength(64).IsRequired();
            entity.Property(e => e.Status).HasMaxLength(50).IsRequired();
            entity.Property(e => e.Note).HasMaxLength(500);

            entity.HasIndex(e => e.DisputeId);
            entity.HasIndex(e => e.ChangedAt);
        });

        modelBuilder.Entity<Refund>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasMaxLength(64);
            entity.Property(e => e.Reference).HasMaxLength(64).IsRequired();
            entity.Property(e => e.TransactionId).HasMaxLength(64).IsRequired();
            entity.Property(e => e.Amount).HasPrecision(18, 2);
            entity.Property(e => e.Percentage).HasPrecision(5, 2);

            entity.HasIndex(e => e.TransactionId).IsUnique();
        });

        modelBuilder.Entity<TransactionEvent>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasMaxLength(64);
            entity.Property(e => e.TransactionId).HasMaxLength(64).IsRequired();
            entity.Property(e => e.Message).HasMaxLength(500).IsRequired();

            entity.HasIndex(e => e.TransactionId);
            entity.HasIndex(e => e.Timestamp);
        });

        modelBuilder.Entity<MerchantEntry>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasMaxLength(64);
            entity.Property(e => e.TransactionId).HasMaxLength(64).IsRequired();
            entity.Property(e => e.Reference).HasMaxLength(64).IsRequired();
            entity.Property(e => e.Type).HasMaxLength(20).IsRequired();
            entity.Property(e => e.Amount).HasPrecision(18, 2);

            entity.HasIndex(e => e.TransactionId).IsUnique();
        });
    }
}


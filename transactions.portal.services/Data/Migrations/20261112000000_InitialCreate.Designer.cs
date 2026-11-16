using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using transactions.portal.services.Data.Sql;

namespace transactions.portal.services.Data.Migrations;

[DbContext(typeof(AppDbContext))]
[Migration("20261112000000_InitialCreate")]
partial class InitialCreate
{
    protected override void BuildTargetModel(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasAnnotation("ProductVersion", "8.0.11")
            .HasAnnotation("Relational:MaxIdentifierLength", 128);

        SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

        modelBuilder.Entity("transactions.portal.services.Models.Dispute", b =>
        {
            b.Property<string>("Id")
                .HasMaxLength(64)
                .HasColumnType("nvarchar(64)");

            b.Property<DateTime>("CreatedAt")
                .HasColumnType("datetime2");

            b.Property<string>("CustomerId")
                .IsRequired()
                .HasMaxLength(64)
                .HasColumnType("nvarchar(64)");

            b.Property<string>("Description")
                .IsRequired()
                .HasMaxLength(1000)
                .HasColumnType("nvarchar(1000)");

            b.Property<string>("Reason")
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnType("nvarchar(200)");

            b.Property<DateTime?>("ResolvedAt")
                .HasColumnType("datetime2");

            b.Property<string>("Status")
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnType("nvarchar(50)");

            b.Property<string>("TransactionId")
                .IsRequired()
                .HasMaxLength(64)
                .HasColumnType("nvarchar(64)");

            b.HasKey("Id");

            b.HasIndex("CreatedAt");
            b.HasIndex("CustomerId");
            b.HasIndex("Status");
            b.HasIndex("TransactionId");

            b.ToTable("Disputes");
        });

        modelBuilder.Entity("transactions.portal.services.Models.DisputeStatusHistory", b =>
        {
            b.Property<string>("Id")
                .HasMaxLength(64)
                .HasColumnType("nvarchar(64)");

            b.Property<DateTime>("ChangedAt")
                .HasColumnType("datetime2");

            b.Property<string>("DisputeId")
                .IsRequired()
                .HasMaxLength(64)
                .HasColumnType("nvarchar(64)");

            b.Property<string>("Note")
                .HasMaxLength(500)
                .HasColumnType("nvarchar(500)");

            b.Property<string>("Status")
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnType("nvarchar(50)");

            b.HasKey("Id");

            b.HasIndex("ChangedAt");
            b.HasIndex("DisputeId");

            b.ToTable("DisputeStatusHistories");
        });

        modelBuilder.Entity("transactions.portal.services.Models.MerchantEntry", b =>
        {
            b.Property<string>("Id")
                .HasMaxLength(64)
                .HasColumnType("nvarchar(64)");

            b.Property<decimal>("Amount")
                .HasPrecision(18, 2)
                .HasColumnType("decimal(18,2)");

            b.Property<DateTime>("PostingDate")
                .HasColumnType("datetime2");

            b.Property<string>("Reference")
                .IsRequired()
                .HasMaxLength(64)
                .HasColumnType("nvarchar(64)");

            b.Property<string>("TransactionId")
                .IsRequired()
                .HasMaxLength(64)
                .HasColumnType("nvarchar(64)");

            b.Property<string>("Type")
                .IsRequired()
                .HasMaxLength(20)
                .HasColumnType("nvarchar(20)");

            b.HasKey("Id");

            b.HasIndex("TransactionId")
                .IsUnique();

            b.ToTable("MerchantEntries");
        });

        modelBuilder.Entity("transactions.portal.services.Models.Refund", b =>
        {
            b.Property<string>("Id")
                .HasMaxLength(64)
                .HasColumnType("nvarchar(64)");

            b.Property<decimal>("Amount")
                .HasPrecision(18, 2)
                .HasColumnType("decimal(18,2)");

            b.Property<decimal>("Percentage")
                .HasPrecision(5, 2)
                .HasColumnType("decimal(5,2)");

            b.Property<DateTime>("ProcessedAt")
                .HasColumnType("datetime2");

            b.Property<string>("Reference")
                .IsRequired()
                .HasMaxLength(64)
                .HasColumnType("nvarchar(64)");

            b.Property<string>("TransactionId")
                .IsRequired()
                .HasMaxLength(64)
                .HasColumnType("nvarchar(64)");

            b.HasKey("Id");

            b.HasIndex("TransactionId")
                .IsUnique();

            b.ToTable("Refunds");
        });

        modelBuilder.Entity("transactions.portal.services.Models.Transaction", b =>
        {
            b.Property<string>("Id")
                .HasMaxLength(64)
                .HasColumnType("nvarchar(64)");

            b.Property<decimal>("Amount")
                .HasPrecision(18, 2)
                .HasColumnType("decimal(18,2)");

            b.Property<string>("CardLast4")
                .IsRequired()
                .HasMaxLength(4)
                .HasColumnType("nvarchar(4)");

            b.Property<string>("CustomerId")
                .IsRequired()
                .HasMaxLength(64)
                .HasColumnType("nvarchar(64)");

            b.Property<DateTime>("Date")
                .HasColumnType("datetime2");

            b.Property<string>("Merchant")
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnType("nvarchar(200)");

            b.Property<string>("PaymentBrand")
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnType("nvarchar(50)");

            b.Property<string>("Reference")
                .IsRequired()
                .HasMaxLength(64)
                .HasColumnType("nvarchar(64)");

            b.Property<string>("Status")
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnType("nvarchar(50)");

            b.HasKey("Id");

            b.HasIndex("CustomerId");
            b.HasIndex("Date");
            b.HasIndex("Reference")
                .IsUnique();

            b.HasIndex("Status");

            b.ToTable("Transactions");
        });

        modelBuilder.Entity("transactions.portal.services.Models.TransactionEvent", b =>
        {
            b.Property<string>("Id")
                .HasMaxLength(64)
                .HasColumnType("nvarchar(64)");

            b.Property<string>("Message")
                .IsRequired()
                .HasMaxLength(500)
                .HasColumnType("nvarchar(500)");

            b.Property<DateTime>("Timestamp")
                .HasColumnType("datetime2");

            b.Property<string>("TransactionId")
                .IsRequired()
                .HasMaxLength(64)
                .HasColumnType("nvarchar(64)");

            b.HasKey("Id");

            b.HasIndex("Timestamp");
            b.HasIndex("TransactionId");

            b.ToTable("TransactionEvents");
        });

        modelBuilder.Entity("transactions.portal.services.Models.Dispute", b =>
        {
            b.HasOne("transactions.portal.services.Models.Transaction", "Transaction")
                .WithMany("Disputes")
                .HasForeignKey("TransactionId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            b.Navigation("Transaction");
        });

        modelBuilder.Entity("transactions.portal.services.Models.DisputeStatusHistory", b =>
        {
            b.HasOne("transactions.portal.services.Models.Dispute", "Dispute")
                .WithMany("StatusHistory")
                .HasForeignKey("DisputeId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            b.Navigation("Dispute");
        });

        modelBuilder.Entity("transactions.portal.services.Models.MerchantEntry", b =>
        {
            b.HasOne("transactions.portal.services.Models.Transaction", "Transaction")
                .WithOne("MerchantEntry")
                .HasForeignKey("transactions.portal.services.Models.MerchantEntry", "TransactionId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            b.Navigation("Transaction");
        });

        modelBuilder.Entity("transactions.portal.services.Models.Refund", b =>
        {
            b.HasOne("transactions.portal.services.Models.Transaction", "Transaction")
                .WithOne("Refund")
                .HasForeignKey("transactions.portal.services.Models.Refund", "TransactionId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            b.Navigation("Transaction");
        });

        modelBuilder.Entity("transactions.portal.services.Models.TransactionEvent", b =>
        {
            b.HasOne("transactions.portal.services.Models.Transaction", "Transaction")
                .WithMany("Events")
                .HasForeignKey("TransactionId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            b.Navigation("Transaction");
        });

        modelBuilder.Entity("transactions.portal.services.Models.Dispute", b =>
        {
            b.Navigation("StatusHistory");
        });

        modelBuilder.Entity("transactions.portal.services.Models.Transaction", b =>
        {
            b.Navigation("Disputes");
            b.Navigation("Events");
            b.Navigation("MerchantEntry");
            b.Navigation("Refund");
        });
    }
}


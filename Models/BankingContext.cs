using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BankingSystem1.Models;

public partial class BankingContext : DbContext
{
    public BankingContext()
    {
    }

    public BankingContext(DbContextOptions<BankingContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<AccountCreationRequest> AccountCreationRequests { get; set; }

    public virtual DbSet<AccountStatus> AccountStatuses { get; set; }

    public virtual DbSet<AccountType> AccountTypes { get; set; }

    public virtual DbSet<Branch> Branches { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public DbSet<Currency> Currencies { get; set; }

    public DbSet<TransactionExtractLog> TransactionExtractLog { get; set; }
    public DbSet<ParsedTransactionData> ParsedTransactionData { get; set; }



    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-IJPKG02\\SQLEXPRESS01;Database=BankingSystem;Trusted_Connection=True;Encrypt=False;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ParsedTransactionData>().HasNoKey();
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PK__Accounts__349DA586136C7AFB");

            entity.Property(e => e.Balance).HasDefaultValue(0m);
            entity.Property(e => e.DateOpened).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.AccountStatus).WithMany(p => p.Accounts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Accounts__Accoun__693CA210");

            entity.HasOne(d => d.AccountType).WithMany(p => p.Accounts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Accounts__Accoun__68487DD7");

            entity.HasOne(d => d.Branch).WithMany(p => p.Accounts).HasConstraintName("FK__Accounts__Branch__6A30C649");

            entity.HasOne(d => d.User).WithMany(p => p.Accounts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Accounts__UserID__6754599E");
        });

        modelBuilder.Entity<AccountCreationRequest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__AccountC__3214EC07FB753085");

            entity.Property(e => e.RequestDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Status).HasDefaultValue("Pending");
        });

        modelBuilder.Entity<AccountStatus>(entity =>
        {
            entity.HasKey(e => e.StatusId).HasName("PK__AccountS__C8EE2043902A7D31");
        });

        modelBuilder.Entity<AccountType>(entity =>
        {
            entity.HasKey(e => e.AccountTypeId).HasName("PK__AccountT__8F95854FC46F35C2");
        });

        modelBuilder.Entity<Branch>(entity =>
        {
            entity.HasKey(e => e.BranchId).HasName("PK__Branches__A1682FA591528A3C");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.TransactionId).HasName("PK__Transact__55433A4BBB3232D5");

            entity.Property(e => e.Timestamp).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.ReceiverAccount).WithMany(p => p.TransactionReceiverAccounts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Transacti__Recei__6EF57B66");

            entity.HasOne(d => d.SenderAccount).WithMany(p => p.TransactionSenderAccounts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Transacti__Sende__6E01572D");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CCACB2382CCB");

            entity.Property(e => e.IsAdmin).HasDefaultValue(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

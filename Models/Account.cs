using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BankingSystem1.Models;

[Index("UserId", "AccountTypeId", Name = "UQ_OneAccountPerType", IsUnique = true)]
public partial class Account
{
    
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("AccountID")]
    public int AccountId { get; set; }

    [Column("UserID")]
    public int UserId { get; set; }

    [Column("AccountTypeID")]
    public int AccountTypeId { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Balance { get; set; }

    [Column("AccountStatusID")]
    public int AccountStatusId { get; set; }

    [Column("BranchID")]
    public int? BranchId { get; set; }

    public DateOnly? DateOpened { get; set; }

    public DateOnly? DateClosed { get; set; }

    [ForeignKey("AccountStatusId")]
    [InverseProperty("Accounts")]
    public virtual AccountStatus AccountStatus { get; set; } = null!;

    [ForeignKey("AccountTypeId")]
    [InverseProperty("Accounts")]
    public virtual AccountType AccountType { get; set; } = null!;

    [ForeignKey("BranchId")]
    [InverseProperty("Accounts")]
    public virtual Branch? Branch { get; set; }

    public int CurrencyId { get; set; }
    public Currency Currency { get; set; }





    [InverseProperty("ReceiverAccount")]
    public virtual ICollection<Transaction> TransactionReceiverAccounts { get; set; } = new List<Transaction>();

    [InverseProperty("SenderAccount")]
    public virtual ICollection<Transaction> TransactionSenderAccounts { get; set; } = new List<Transaction>();

    [ForeignKey("UserId")]
    [InverseProperty("Accounts")]
    public virtual User User { get; set; } = null!;
}

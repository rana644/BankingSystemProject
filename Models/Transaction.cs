using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BankingSystem1.Models;

public partial class Transaction
{
    [Key]
    [Column("TransactionID")]
    public int TransactionId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? Timestamp { get; set; }

    [Column("SenderAccountID")]
    public int SenderAccountId { get; set; }

    [Column("ReceiverAccountID")]
    public int ReceiverAccountId { get; set; }

    [ForeignKey("ReceiverAccountId")]
    [InverseProperty("TransactionReceiverAccounts")]
    public virtual Account ReceiverAccount { get; set; } = null!;

    [ForeignKey("SenderAccountId")]
    [InverseProperty("TransactionSenderAccounts")]
    public virtual Account SenderAccount { get; set; } = null!;

    public decimal SenderAmount { get; set; }
    public decimal ReceiverAmount { get; set; }
    public decimal ExchangeRate { get; set; }

}

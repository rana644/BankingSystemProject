using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BankingSystem1.Models;

[Table("AccountStatus")]
public partial class AccountStatus
{
    [Key]
    [Column("StatusID")]
    public int StatusId { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string StatusName { get; set; } = null!;

    [InverseProperty("AccountStatus")]
    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();
}

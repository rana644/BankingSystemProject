using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BankingSystem1.Models;

[Table("AccountType")]
public partial class AccountType
{
    [Key]
    [Column("AccountTypeID")]
    public int AccountTypeId { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string TypeName { get; set; } = null!;

    [InverseProperty("AccountType")]
    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();
}

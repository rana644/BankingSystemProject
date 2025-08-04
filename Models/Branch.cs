using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BankingSystem1.Models;

public partial class Branch
{
    [Key]
    [Column("BranchID")]
    public int BranchId { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string BranchName { get; set; } = null!;

    [StringLength(100)]
    [Unicode(false)]
    public string? Location { get; set; }

    [InverseProperty("Branch")]
    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();
}

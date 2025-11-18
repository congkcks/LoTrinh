using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Service2.Models;

[Table("user_vocabulary")]
public partial class UserVocabulary
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("user_id")]
    public int UserId { get; set; }

    [Column("vocab_id")]
    public int VocabId { get; set; }

    [Column("status")]
    [StringLength(50)]
    public string? Status { get; set; }

    [Column("added_at", TypeName = "timestamp without time zone")]
    public DateTime? AddedAt { get; set; }
}

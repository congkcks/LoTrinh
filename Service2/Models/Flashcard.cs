using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Service2.Models;

[Table("flashcards")]
public partial class Flashcard
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("vocab_id")]
    public int VocabId { get; set; }

    [Column("front")]
    public string? Front { get; set; }

    [Column("back")]
    public string? Back { get; set; }
}

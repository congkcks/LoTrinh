using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Service2.Models;

[Table("vocabulary")]
public partial class Vocabulary
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("lesson_id")]
    public int LessonId { get; set; }

    [Column("word")]
    [StringLength(200)]
    public string Word { get; set; } = null!;

    [Column("meaning")]
    public string? Meaning { get; set; }

    [Column("example")]
    public string? Example { get; set; }

    [Column("phonetic")]
    [StringLength(100)]
    public string? Phonetic { get; set; }

    [Column("level")]
    [StringLength(50)]
    public string? Level { get; set; }
}

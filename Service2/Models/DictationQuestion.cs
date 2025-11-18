using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Service2.Models;

[Table("dictation_questions")]
public partial class DictationQuestion
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("lesson_id")]
    public int LessonId { get; set; }

    [Column("audio_text")]
    public string? AudioText { get; set; }

    [Column("correct_text")]
    public string? CorrectText { get; set; }
}

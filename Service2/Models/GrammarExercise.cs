using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Service2.Models;

[Table("grammar_exercises")]
public partial class GrammarExercise
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("lesson_id")]
    public int LessonId { get; set; }

    [Column("question")]
    public string? Question { get; set; }

    [Column("correct_answer")]
    public string? CorrectAnswer { get; set; }

    [Column("hint")]
    public string? Hint { get; set; }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Service2.Models;

[Table("reading_questions")]
public partial class ReadingQuestion
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("passage_id")]
    public int PassageId { get; set; }

    [Column("question")]
    public string? Question { get; set; }

    [Column("option_a")]
    public string? OptionA { get; set; }

    [Column("option_b")]
    public string? OptionB { get; set; }

    [Column("option_c")]
    public string? OptionC { get; set; }

    [Column("option_d")]
    public string? OptionD { get; set; }

    [Column("correct_option")]
    [MaxLength(1)]
    public char? CorrectOption { get; set; }
}

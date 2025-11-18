using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Service2.Models;

[Table("listening_practice")]
public partial class ListeningPractice
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("lesson_id")]
    public int LessonId { get; set; }

    [Column("audio_text")]
    public string? AudioText { get; set; }

    [Column("transcript")]
    public string? Transcript { get; set; }
}

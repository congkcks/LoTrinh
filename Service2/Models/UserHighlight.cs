using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Service2.Models;

[Table("user_highlights")]
public partial class UserHighlight
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("user_id")]
    public int UserId { get; set; }

    [Column("lesson_id")]
    public int LessonId { get; set; }

    [Column("start_index")]
    public int? StartIndex { get; set; }

    [Column("end_index")]
    public int? EndIndex { get; set; }

    [Column("color")]
    [StringLength(20)]
    public string? Color { get; set; }

    [Column("created_at", TypeName = "timestamp without time zone")]
    public DateTime? CreatedAt { get; set; }
}

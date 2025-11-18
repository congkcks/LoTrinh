
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Service2.Models;

[Table("user_flashcard_progress")]
public partial class UserFlashcardProgress
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("user_id")]
    public int UserId { get; set; }

    [Column("flashcard_id")]
    public int FlashcardId { get; set; }

    [Column("score")]
    public int? Score { get; set; }

    [Column("last_review", TypeName = "timestamp without time zone")]
    public DateTime? LastReview { get; set; }
}

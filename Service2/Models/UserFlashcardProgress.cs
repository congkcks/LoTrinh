using System;
using System.Collections.Generic;

namespace Service2.Models;

public partial class UserFlashcardProgress
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int FlashcardId { get; set; }

    public int? Score { get; set; }

    public DateTime? LastReview { get; set; }

    public bool? IsMastered { get; set; }

    public int? ReviewCount { get; set; }
}

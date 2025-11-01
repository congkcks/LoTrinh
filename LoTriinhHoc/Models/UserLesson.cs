using System;
using System.Collections.Generic;

namespace LoTriinhHoc.Models;

public partial class UserLesson
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public int? LessonId { get; set; }

    public int? ProgressPercent { get; set; }

    public bool? IsCompleted { get; set; }

    public DateTime? LastAccess { get; set; }

    public int? LastWatchedSecond { get; set; }

    public decimal? Score { get; set; }

    public string? Notes { get; set; }

    public virtual Lesson? Lesson { get; set; }

    public virtual User? User { get; set; }
}

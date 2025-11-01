using System;
using System.Collections.Generic;

namespace LoTriinhHoc.Models;

public partial class User
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public virtual ICollection<LearningPlan> LearningPlans { get; set; } = new List<LearningPlan>();

    public virtual ICollection<UserLesson> UserLessons { get; set; } = new List<UserLesson>();
}

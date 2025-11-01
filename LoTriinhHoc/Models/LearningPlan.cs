using System;
using System.Collections.Generic;

namespace LoTriinhHoc.Models;

public partial class LearningPlan
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public bool? IsActive { get; set; }

    public string? Goal { get; set; }

    public string? PlanData { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual User? User { get; set; }
}

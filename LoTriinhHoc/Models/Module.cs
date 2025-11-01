using System;
using System.Collections.Generic;

namespace LoTriinhHoc.Models;

public partial class Module
{
    public int Id { get; set; }

    public int? CourseId { get; set; }

    public string Name { get; set; } = null!;

    public virtual Course? Course { get; set; }

    public virtual ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
}

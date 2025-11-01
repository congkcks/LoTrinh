using System;
using System.Collections.Generic;

namespace LoTriinhHoc.Models;

public partial class Video
{
    public int Id { get; set; }

    public int? LessonId { get; set; }

    public string? Title { get; set; }

    public string? FilePath { get; set; }

    public virtual Lesson? Lesson { get; set; }
}

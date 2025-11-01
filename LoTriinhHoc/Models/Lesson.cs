using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoTriinhHoc.Models;

public partial class Lesson
{
    public int Id { get; set; }

    public int? ModuleId { get; set; }

    public string Title { get; set; } = null!;

    public virtual Module? Module { get; set; }

    public virtual ICollection<UserLesson> UserLessons { get; set; } = new List<UserLesson>();

    public virtual ICollection<Video> Videos { get; set; } = new List<Video>();
    

}

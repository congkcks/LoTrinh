

namespace LoTriinhHoc.Models;

public partial class ExerciseType
{
    public int Id { get; set; }

    public int LessonId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Exercise> Exercises { get; set; } = new List<Exercise>();
}

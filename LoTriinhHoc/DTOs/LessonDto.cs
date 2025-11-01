using LoTriinhHoc.DTOs;

namespace Lotrinhhoc.DTOs;

public class LessonDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;

    public List<VideoDto> Videos { get; set; } = new();
    public List<ExerciseDto> Exercises { get; set; } = new();
}

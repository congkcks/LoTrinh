
using Lotrinhhoc.DTOs;

namespace LoTriinhHoc.DTOs;

public class CourseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public List<ModuleDto> Modules { get; set; } = new();
}

public class ModuleDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public List<LessonDto> Lessons { get; set; } = new();
}
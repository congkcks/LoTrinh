namespace LoTriinhHoc.DTOs;

public class ExerciseTypeDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<ExerciseTypeDto> ExerciseTypes { get; set; } = new();
}

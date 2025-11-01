namespace LoTriinhHoc.DTOs;

public class ExerciseDto
{
    public int Id { get; set; }
    public string Question { get; set; } = string.Empty;
    public string? OptionA { get; set; }
    public string? OptionB { get; set; }
    public string? OptionC { get; set; }
    public string? OptionD { get; set; }
    public char? CorrectOption { get; set; }
    public string? Explanation { get; set; }
}

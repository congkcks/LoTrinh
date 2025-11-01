
namespace LoTriinhHoc.Models;

public partial class Exercise
{
    public int Id { get; set; }

    public int? TypeId { get; set; }

    public string Question { get; set; } = null!;

    public string? OptionA { get; set; }

    public string? OptionB { get; set; }

    public string? OptionC { get; set; }

    public string? OptionD { get; set; }

    public char? CorrectOption { get; set; }

    public string? Explanation { get; set; }

    public virtual ExerciseType? Type { get; set; }
}

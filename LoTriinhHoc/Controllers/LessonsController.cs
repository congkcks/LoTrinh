using LoTriinhHoc.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LoTriinhHoc.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LessonsController : ControllerBase
{
    private readonly LotrinhhocDbContext _context;

    public LessonsController(LotrinhhocDbContext context)
    {
        _context = context;
    }

    // lay chi tiet co

    [HttpGet("{id}")]
    public async Task<IActionResult> GetLessonDetail(int id)
    {
        var lesson = await _context.Lessons.FirstOrDefaultAsync(l => l.Id == id);
        if (lesson == null) return NotFound();

        // ✅ Lấy video theo LessonId
        var videos = await _context.Videos
            .Where(v => v.LessonId == id)
            .Select(v => new
            {
                v.Id,
                v.Title,
                v.FilePath
                
            })
            .ToListAsync();

        // ✅ Lấy ExerciseTypes + Exercises như trước
        var exerciseTypes = await _context.ExerciseTypes
            .Include(t => t.Exercises)
            .Where(t => t.LessonId == id)
            .Select(t => new
            {
                t.Id,
                t.Name,
                Exercises = t.Exercises.Select(e => new
                {
                    e.Id,
                    e.Question,
                    e.OptionA,
                    e.OptionB,
                    e.OptionC,
                    e.OptionD,
                    e.CorrectOption,
                    e.Explanation
                })
            })
            .ToListAsync();

        return Ok(new
        {
            lesson.Id,
            lesson.Title,
            Videos = videos,
            ExerciseTypes = exerciseTypes
        });
    }


}

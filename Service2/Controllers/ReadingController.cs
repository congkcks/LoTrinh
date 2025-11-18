using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service2.Data;

[ApiController]
[Route("api/reading")]
public class ReadingController : ControllerBase
{
    private readonly EngAceDbContext _db;

    public ReadingController(EngAceDbContext db)
    {
        _db = db;
    }

    [HttpGet("{lessonId}")]
    public async Task<IActionResult> GetReadingByLesson(int lessonId)
    {
        var passage = await _db.ReadingPassages
            .FirstOrDefaultAsync(r => r.LessonId == lessonId);

        if (passage == null)
            return NotFound("No passage found for this lesson.");

        var questions = await _db.ReadingQuestions
            .Where(q => q.PassageId == passage.Id)
            .ToListAsync();

        return Ok(new
        {
            Passage = passage,
            Questions = questions
        });
    }
}

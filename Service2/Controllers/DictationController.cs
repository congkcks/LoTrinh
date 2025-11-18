using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service2.Data;

[ApiController]
[Route("api/dictation")]
public class DictationController : ControllerBase
{
    private readonly EngAceDbContext _db;

    public DictationController(EngAceDbContext db)
    {
        _db = db;
    }

    [HttpGet("{lessonId}")]
    public async Task<IActionResult> GetDictationByLesson(int lessonId)
    {
        var items = await _db.DictationQuestions
            .Where(d => d.LessonId == lessonId)
            .ToListAsync();

        return Ok(items);
    }
}

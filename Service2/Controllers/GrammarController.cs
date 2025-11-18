using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service2.Data;

[ApiController]
[Route("api/grammar")]
public class GrammarController : ControllerBase
{
    private readonly EngAceDbContext _db;

    public GrammarController(EngAceDbContext db)
    {
        _db = db;
    }

    [HttpGet("{lessonId}")]
    public async Task<IActionResult> GetGrammarByLesson(int lessonId)
    {
        var items = await _db.GrammarExercises
            .Where(g => g.LessonId == lessonId)
            .ToListAsync();

        return Ok(items);
    }
}

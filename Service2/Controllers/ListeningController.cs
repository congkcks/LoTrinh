using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service2.Data;

[ApiController]
[Route("api/listening")]
public class ListeningController : ControllerBase
{
    private readonly EngAceDbContext _db;

    public ListeningController(EngAceDbContext db)
    {
        _db = db;
    }

    [HttpGet("{lessonId}")]
    public async Task<IActionResult> GetListeningByLesson(int lessonId)
    {
        var items = await _db.ListeningPractices
            .Where(l => l.LessonId == lessonId)
            .ToListAsync();

        return Ok(items);
    }
}

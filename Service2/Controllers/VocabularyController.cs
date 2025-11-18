using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service2.Data;

[ApiController]
[Route("api/vocabulary")]
public class VocabularyController : ControllerBase
{
    private readonly EngAceDbContext _db;

    public VocabularyController(EngAceDbContext db)
    {
        _db = db;
    }

    [HttpGet("{lessonId}")]
    public async Task<IActionResult> GetVocabByLesson(int lessonId)
    {
        var items = await _db.Vocabularies
            .Where(v => v.LessonId == lessonId)
            .ToListAsync();

        return Ok(items);
    }
}

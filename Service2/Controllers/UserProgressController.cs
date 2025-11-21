using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service2.Data;
using Service2.Models;

namespace Service2.Controllers;

[ApiController]
[Route("api/user-progress")]
public class UserProgressController : ControllerBase
{
    private readonly Service2DbContext _service2Db;
    private readonly EngAceDbContext _db;

    public UserProgressController(Service2DbContext service2Db, EngAceDbContext db)
    {
        _service2Db = service2Db;
        _db = db;
    }
    [HttpPost("grammar")]
    public async Task<IActionResult> SaveGrammarProgress([FromBody] UserGrammarProgress request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
 
        var existing = await _service2Db.UserGrammarProgresses
            .FirstOrDefaultAsync(x => x.UserId == request.UserId &&
                                      x.GrammarExerciseId == request.GrammarExerciseId);
        if (existing == null)
        {
            existing = new UserGrammarProgress
            {
                UserId = request.UserId,
                GrammarExerciseId = request.GrammarExerciseId
            };
            _service2Db.UserGrammarProgresses.Add(existing);
        }

        existing.Score = request.Score;
        existing.IsCompleted = request.IsCompleted;

        if (request.IsCompleted == true)
        {
            existing.CompletedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified);
        }

        await _service2Db.SaveChangesAsync();

        return Ok(new
        {
            message = "Grammar progress saved",
            data = existing
        });
    }

    [HttpPost("reading")]
    public async Task<IActionResult> SaveReadingProgress([FromBody] UserReadingProgress request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var existing = await _service2Db.UserReadingProgresses
            .FirstOrDefaultAsync(x => x.UserId == request.UserId &&
                                      x.ReadingPassageId == request.ReadingPassageId);

        if (existing == null)
        {
            existing = new UserReadingProgress
            {
                UserId = request.UserId,
                ReadingPassageId = request.ReadingPassageId
            };
            _service2Db.UserReadingProgresses.Add(existing);
        }

        existing.Score = request.Score;
        existing.IsCompleted = request.IsCompleted;

        if (request.IsCompleted == true)
        {
            existing.CompletedAt = DateTime.Now;
        }

        await _service2Db.SaveChangesAsync();

        return Ok(new
        {
            message = "Reading progress saved",
            data = existing
        });
    }

    [HttpPost("listening")]
    public async Task<IActionResult> SaveListeningProgress([FromBody] UserListeningProgress request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var existing = await _service2Db.UserListeningProgresses
            .FirstOrDefaultAsync(x => x.UserId == request.UserId &&
                                      x.ListeningId == request.ListeningId);

        if (existing == null)
        {
            existing = new UserListeningProgress
            {
                UserId = request.UserId,
                ListeningId = request.ListeningId
            };
            _service2Db.UserListeningProgresses.Add(existing);
        }

        existing.Score = request.Score;
        existing.IsCompleted = request.IsCompleted;

        if (request.IsCompleted == true)
        {
            existing.CompletedAt = DateTime.Now;
        }

        await _service2Db.SaveChangesAsync();

        return Ok(new
        {
            message = "Listening progress saved",
            data = existing
        });
    }
    [HttpPost("flashcard")]
    public async Task<IActionResult> SaveFlashcardProgress([FromBody] UserFlashcardProgress request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var existing = await _service2Db.UserFlashcardProgresses
            .FirstOrDefaultAsync(x => x.UserId == request.UserId &&
                                      x.FlashcardId == request.FlashcardId);

        if (existing == null)
        {
            existing = new UserFlashcardProgress
            {
                UserId = request.UserId,
                FlashcardId = request.FlashcardId,
                ReviewCount = 0
            };
            _db.UserFlashcardProgresses.Add(existing);
        }

        existing.Score = request.Score;
        existing.IsMastered = request.IsMastered;
        existing.LastReview = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified);
        existing.ReviewCount = (existing.ReviewCount ?? 0) + 1;

        await _db.SaveChangesAsync();

        return Ok(new
        {
            message = "Flashcard progress saved",
            data = existing
        });
    }
}

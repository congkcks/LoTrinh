using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service2.Models;

[ApiController]
[Route("api/user-progress")]
public class ProgressController : ControllerBase
{
    private readonly Service2DbContext _db;
    public ProgressController(Service2DbContext db) => _db = db;
    [HttpPost("grammar/submit")]
    public async Task<IActionResult> SubmitGrammar([FromBody] UserGrammarProgress req)
    {
        req.IsCompleted = true;
        req.CompletedAt = DateTime.UtcNow;

        _db.UserGrammarProgresses.Add(req);
        await _db.SaveChangesAsync();

        return Ok(new { message = "Grammar progress saved", data = req });
    }

    [HttpGet("grammar/completed/{userId}")]
    public async Task<int> GetGrammarCompleted(int userId)
    {
        return await _db.UserGrammarProgresses
            .CountAsync(x => x.UserId == userId && x.IsCompleted == true);
    }
    [HttpPost("reading/submit")]
    public async Task<IActionResult> SubmitReading([FromBody] UserReadingProgress req)
    {
        req.IsCompleted = true;
        req.CompletedAt = DateTime.UtcNow;

        _db.UserReadingProgresses.Add(req);
        await _db.SaveChangesAsync();

        return Ok(new { message = "Reading progress saved", data = req });
    }

    [HttpGet("reading/completed/{userId}")]
    public async Task<int> GetReadingCompleted(int userId)
    {
        return await _db.UserReadingProgresses
            .CountAsync(x => x.UserId == userId && x.IsCompleted == true);
    }

    [HttpPost("listening/submit")]
    public async Task<IActionResult> SubmitListening([FromBody] UserListeningProgress req)
    {
        req.IsCompleted = true;
        req.CompletedAt = DateTime.UtcNow;

        _db.UserListeningProgresses.Add(req);
        await _db.SaveChangesAsync();

        return Ok(new { message = "Listening progress saved", data = req });
    }

    [HttpGet("listening/completed/{userId}")]
    public async Task<int> GetListeningCompleted(int userId)
    {
        return await _db.UserListeningProgresses
            .CountAsync(x => x.UserId == userId && x.IsCompleted == true);
    }
    [HttpPost("flashcard/master")]
    public async Task<IActionResult> MasterFlashcard([FromBody] UserFlashcardProgress req)
    {
        req.IsMastered = true;
        req.LastReview = DateTime.UtcNow;
        req.ReviewCount++;

        _db.UserFlashcardProgresses.Add(req);
        await _db.SaveChangesAsync();

        return Ok(new { message = "Flashcard mastered", data = req });
    }

    [HttpGet("flashcard/mastered/{userId}")]
    public async Task<int> GetFlashcardMastered(int userId)
    {
        return await _db.UserFlashcardProgresses
            .CountAsync(x => x.UserId == userId && x.IsMastered == true);
    }
}

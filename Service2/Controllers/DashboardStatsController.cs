using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service2.Data;
using Service2.Models;
namespace Service2.Controllers;
[ApiController]
[Route("api/dashboard-stats")]
public class DashboardStatsController : ControllerBase
{
    private readonly EngAceDbContext _db;
    private readonly Service2DbContext _service2Db;

    public DashboardStatsController(EngAceDbContext db,Service2DbContext service2)
    {
        _db = db;
        _service2Db = service2;
    }
    [HttpGet("vocabulary/{userId}")]
    public async Task<int> GetVocabularyCount(int userId)
    {
        return await _db.UserVocabularies
            .CountAsync(x => x.UserId == userId);
    }
    [HttpGet("notes/{userId}")]
    public async Task<int> GetNotesCount(int userId)
    {
        return await _db.UserNotes
            .CountAsync(x => x.UserId == userId);
    }


    [HttpGet("highlights/{userId}")]
    public async Task<int> GetHighlightCount(int userId)
    {
        return await _db.UserHighlights
            .CountAsync(x => x.UserId == userId);
    }
    [HttpGet("grammar-completed/{userId}")]
    public async Task<int> GetGrammarCompleted(int userId)
    {
        return await _service2Db.UserGrammarProgresses
            .CountAsync(x => x.UserId == userId && x.IsCompleted == true);
    }

    [HttpGet("reading-completed/{userId}")]
    public async Task<int> GetReadingCompleted(int userId)
    {
        return await _service2Db.UserReadingProgresses
            .CountAsync(x => x.UserId == userId && x.IsCompleted == true);
    }
    [HttpGet("listening-completed/{userId}")]
    public async Task<int> GetListeningCompleted(int userId)
    {
        return await _service2Db.UserListeningProgresses
            .CountAsync(x => x.UserId == userId && x.IsCompleted == true);
    }


    [HttpGet("flashcard-mastered/{userId}")]
    public async Task<int> GetFlashcardMastered(int userId)
    {
        return await _db.UserFlashcardProgresses
            .CountAsync(x => x.UserId == userId && x.IsMastered == true);
    }
    [HttpGet("summary/{userId}")]
    public async Task<IActionResult> GetSummary(int userId)
    {
        var vocab = await GetVocabularyCount(userId);
        var notes = await GetNotesCount(userId);
        var highlights = await GetHighlightCount(userId);

        var grammar = await GetGrammarCompleted(userId);
        var reading = await GetReadingCompleted(userId);
        var listening = await GetListeningCompleted(userId);
        var flashcards = await GetFlashcardMastered(userId);

        return Ok(new
        {
            vocabulary = vocab,
            notes,
            highlights,
            grammarCompleted = grammar,
            readingCompleted = reading,
            listeningCompleted = listening,
            flashcardMastered = flashcards
        });
    }
}

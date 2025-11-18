using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LoTriinhHoc.Data;


[ApiController]
[Route("api/dashboard")]
public class DashboardController : ControllerBase
{
    private readonly LotrinhhocDbContext _db;
    private readonly IHttpClientFactory _clientFactory;

    public DashboardController(LotrinhhocDbContext db, IHttpClientFactory clientFactory)
    {
        _db = db;
        _clientFactory = clientFactory;
    }

    [HttpGet("home/{userId}")]
    public async Task<IActionResult> GetDashboard(int userId)
    {
        // === 1) DỮ LIỆU SERVICE 1 (LOCAL DB) ===

        var totalLessons = await _db.Lessons.CountAsync();
        var completedLessons = await _db.UserLessons.CountAsync(x => x.UserId == userId && x.IsCompleted == true);

        var avgScore = await _db.UserLessons
            .Where(x => x.UserId == userId && x.Score != null)
            .AverageAsync(x => (double?)x.Score) ?? 0;

        var lastAccess = await _db.UserLessons
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.LastAccess)
            .Select(x => x.LastAccess)
            .FirstOrDefaultAsync();

        var plans = await _db.LearningPlans
            .Where(x => x.UserId == userId && x.IsActive == true)
            .ToListAsync();


        var client = _clientFactory.CreateClient("Service2");

        var vocabRes = await client.GetAsync($"api/user-vocabulary/count/{userId}");
        int vocabCount = vocabRes.IsSuccessStatusCode
            ? int.Parse(await vocabRes.Content.ReadAsStringAsync())
            : 0;

        var notesRes = await client.GetAsync($"api/user-notes/count/{userId}");
        int notesCount = notesRes.IsSuccessStatusCode
            ? int.Parse(await notesRes.Content.ReadAsStringAsync())
            : 0;

        // 2.3 Highlights count
        var hlRes = await client.GetAsync($"api/user-highlights/count/{userId}");
        int highlightCount = hlRes.IsSuccessStatusCode
            ? int.Parse(await hlRes.Content.ReadAsStringAsync())
            : 0;

        var grammarDone = await GetInt(client, $"api/stats/grammar-completed/{userId}");
        var listeningDone = await GetInt(client, $"api/stats/listening-completed/{userId}");
        var readingDone = await GetInt(client, $"api/stats/reading-completed/{userId}");

        var nextLesson = await _db.Lessons.Where(l => !_db.UserLessons
           .Any(u => u.UserId == userId
                && u.IsCompleted == true
                && u.LessonId == l.Id))
           .OrderBy(l => l.Id) 
          .FirstOrDefaultAsync();


        return Ok(new
        {
            summary = new
            {
                totalLessons,
                completedLessons,
                progressPercent = totalLessons == 0 ? 0 : completedLessons * 100 / totalLessons,
                avgScore,
                lastAccess
            },

            skills = new
            {
                grammar = grammarDone,
                listening = listeningDone,
                reading = readingDone,
                vocabulary = vocabCount
            },

            userData = new
            {
                notesCount,
                highlightCount
            },

            plans = plans,

            recommendation = nextLesson == null ? null : new
            {
                nextLesson.Id,
                nextLesson.Title
            }
        });
    }

    private async Task<int> GetInt(HttpClient client, string url)
    {
        var res = await client.GetAsync(url);
        return res.IsSuccessStatusCode ? int.Parse(await res.Content.ReadAsStringAsync()) : 0;
    }
}

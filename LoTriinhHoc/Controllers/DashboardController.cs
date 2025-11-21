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
        var client = _clientFactory.CreateClient("Service2");
        var notesRes = await client.GetAsync($"api/dashboard-stats/notes/{userId}");
        int notesCount = notesRes.IsSuccessStatusCode
            ? int.Parse(await notesRes.Content.ReadAsStringAsync())
            : 0;
        var hlRes = await client.GetAsync($"api/dashboard-stats/highlights/{userId}");
        int highlightCount = hlRes.IsSuccessStatusCode
            ? int.Parse(await hlRes.Content.ReadAsStringAsync())
            : 0;

        var vocabRes = await client.GetAsync($"api/dashboard-stats/vocabulary/{userId}");
        int vocabCount = vocabRes.IsSuccessStatusCode
            ? int.Parse(await vocabRes.Content.ReadAsStringAsync())
            : 0;

        var grammarDone = await GetInt(client, $"api/dashboard-stats/grammar-completed/{userId}");
        var listeningDone = await GetInt(client, $"api/dashboard-stats/listening-completed/{userId}");
        var readingDone = await GetInt(client, $"api/dashboard-stats/reading-completed/{userId}");


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
    [HttpGet("courses/{userId}")]
    public async Task<IActionResult> GetCourses(int userId)
    {
        var courses = await _db.Courses
            .Select(course => new
            {
                course.Id,
                course.Name,

                TotalLessons = course.Modules
                    .SelectMany(m => m.Lessons)
                    .Count(),

                CompletedLessons = course.Modules
                    .SelectMany(m => m.Lessons)
                    .Count(l => _db.UserLessons
                        .Any(ul => ul.UserId == userId &&
                                   ul.LessonId == l.Id &&
                                   ul.IsCompleted == true))
            })
            .ToListAsync();

        var result = courses.Select(c => new
        {
            courseId = c.Id,
            name = c.Name,
            totalLessons = c.TotalLessons,
            completedLessons = c.CompletedLessons,
            progressPercent = c.TotalLessons == 0 ? 0 :
                (int)(c.CompletedLessons * 100 / c.TotalLessons)
        });

        return Ok(result);
    }
    [HttpGet("course/{courseId}/modules/{userId}")]
    public async Task<IActionResult> GetModules(int courseId, int userId)
    {
        var course = await _db.Courses
            .Where(c => c.Id == courseId)
            .Select(c => new
            {
                c.Id,
                c.Name,
                Modules = c.Modules.Select(m => new
                {
                    m.Id,
                    m.Name,

                    TotalLessons = m.Lessons.Count(),

                    CompletedLessons = m.Lessons.Count(l =>
                        _db.UserLessons.Any(ul =>
                            ul.UserId == userId &&
                            ul.LessonId == l.Id &&
                            ul.IsCompleted == true))
                })
            })
            .FirstOrDefaultAsync();

        if (course == null)
            return NotFound();

        var modules = course.Modules.Select(m => new
        {
            moduleId = m.Id,
            name = m.Name,
            m.TotalLessons,
            m.CompletedLessons,
            progressPercent = m.TotalLessons == 0 ? 0 :
                (int)(m.CompletedLessons * 100 / m.TotalLessons)
        });

        return Ok(new
        {
            courseId = course.Id,
            course.Name,
            modules
        });
    }
    [HttpGet("module/{moduleId}/lessons/{userId}")]
    public async Task<IActionResult> GetModuleLessons(int moduleId, int userId)
    {
        var module = await _db.Modules
            .Where(m => m.Id == moduleId)
            .Select(m => new
            {
                m.Id,
                m.Name,
                Lessons = m.Lessons.Select(l => new
                {
                    l.Id,
                    l.Title,

                    IsCompleted = _db.UserLessons.Any(ul =>
                        ul.UserId == userId &&
                        ul.LessonId == l.Id &&
                        ul.IsCompleted == true),

                    ProgressPercent = _db.UserLessons
                        .Where(ul => ul.UserId == userId && ul.LessonId == l.Id)
                        .Select(ul => ul.ProgressPercent)
                        .FirstOrDefault() ?? 0
                })
            })
            .FirstOrDefaultAsync();

        if (module == null)
            return NotFound();

        return Ok(module);
    }


}

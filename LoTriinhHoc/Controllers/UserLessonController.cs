using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LoTriinhHoc.Data;
using LoTriinhHoc.Models;
namespace LoTriinhHoc.Api.Controllers;
[ApiController]
[Route("api/user-lessons")]
public class UserLessonController : ControllerBase
{
    private readonly LotrinhhocDbContext _db;

    public UserLessonController(LotrinhhocDbContext db)
    {
        _db = db;
    }

    [HttpGet("{userId}/{lessonId}")]
    public async Task<IActionResult> GetProgress(int userId, int lessonId)
    {
        var item = await _db.UserLessons
            .FirstOrDefaultAsync(x => x.UserId == userId && x.LessonId == lessonId);

        if (item == null)
            return Ok(null);

        return Ok(item);
    }

    [HttpPost("start")]
    public async Task<IActionResult> StartLesson([FromBody] UserLesson request)
    {
        request.LastAccess = DateTime.UtcNow;

        _db.UserLessons.Add(request);
        await _db.SaveChangesAsync();

        return Ok(new { message = "Lesson started", data = request });
    }

    [HttpPut("update-progress/{userId}/{lessonId}")]
    public async Task<IActionResult> UpdateProgress(
        int userId, int lessonId, [FromBody] UserLesson request)
    {
        var item = await _db.UserLessons
            .FirstOrDefaultAsync(x => x.UserId == userId && x.LessonId == lessonId);

        if (item == null)
            return NotFound("Lesson progress not found.");

        item.ProgressPercent = request.ProgressPercent;
        item.LastAccess = DateTime.UtcNow;

        await _db.SaveChangesAsync();

        return Ok(new { message = "Progress updated", data = item });
    }

    
    [HttpPut("finish/{userId}/{lessonId}")]
    public async Task<IActionResult> FinishLesson(
        int userId, int lessonId, [FromBody] UserLesson request)
    {
        var item = await _db.UserLessons
            .FirstOrDefaultAsync(x => x.UserId == userId && x.LessonId == lessonId);

        if (item == null)
            return NotFound("Lesson progress not found.");

        item.IsCompleted = true;
        item.Score = request.Score;
        item.LastAccess = DateTime.UtcNow;

        await _db.SaveChangesAsync();

        return Ok(new { message = "Lesson completed", data = item });
    }

    [HttpPut("watch/{userId}/{lessonId}")]
    public async Task<IActionResult> SaveWatchPosition(
        int userId, int lessonId,
        [FromBody] UserLesson request)
    {
        var item = await _db.UserLessons
            .FirstOrDefaultAsync(x => x.UserId == userId && x.LessonId == lessonId);

        if (item == null)
            return NotFound("Lesson progress not found.");

        item.LastWatchedSecond = request.LastWatchedSecond;
        item.LastAccess = DateTime.UtcNow;

        await _db.SaveChangesAsync();

        return Ok(new
        {
            message = "Watch position saved",
            data = item
        });
    }
}

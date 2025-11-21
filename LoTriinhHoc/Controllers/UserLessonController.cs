using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LoTriinhHoc.Data;
using LoTriinhHoc.Models;
using LoTriinhHoc.DTOs;

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

    // 🔹 Lấy tiến độ hiện tại của user ở 1 lesson
    [HttpGet("{userId}/{lessonId}")]
    public async Task<IActionResult> GetProgress(int userId, int lessonId)
    {
        var progress = await _db.UserLessons
            .Where(x => x.UserId == userId && x.LessonId == lessonId)
            .Select(x => new
            {
                x.UserId,
                x.LessonId,
                x.ProgressPercent,
                x.IsCompleted,
                x.LastAccess
            })
            .FirstOrDefaultAsync();

        return Ok(progress);
    }

    // 🔹 Khởi tạo tiến độ khi bắt đầu học lesson
    [HttpPost("{userId}/{lessonId}/start")]
    public async Task<IActionResult> StartLesson(int userId, int lessonId)
    {
        var existing = await _db.UserLessons
            .FirstOrDefaultAsync(x => x.UserId == userId && x.LessonId == lessonId);

        if (existing != null)
            return Ok(existing); 

        var newProgress = new UserLesson
        {
            UserId = userId,
            LessonId = lessonId,
            ProgressPercent = 0,
            IsCompleted = true,
            LastAccess = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified)
        };
        _db.UserLessons.Add(newProgress);
        await _db.SaveChangesAsync();
        return Ok(newProgress);
    }

    [HttpPut("{userId}/{lessonId}/progress")]
    public async Task<IActionResult> UpdateProgress(
        int userId,
        int lessonId,
        [FromBody] UpdateLessonProgressRequest request)
    {
        var progress = await _db.UserLessons
            .FirstOrDefaultAsync(x => x.UserId == userId && x.LessonId == lessonId);

        if (progress == null)
            return NotFound("Progress not found.");

        progress.ProgressPercent = request.ProgressPercent;
        progress.LastAccess = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified);

        await _db.SaveChangesAsync();

        return Ok(new
        {
            message = "Progress updated",
            progress.ProgressPercent,
            progress.LastAccess
        });
    }

    // 🔹 Hoàn thành lesson
    [HttpPut("{userId}/{lessonId}/finish")]
    public async Task<IActionResult> FinishLesson(
        int userId,
        int lessonId,
        [FromBody] FinishLessonRequest request)
    {
        var progress = await _db.UserLessons
            .FirstOrDefaultAsync(x => x.UserId == userId && x.LessonId == lessonId);

        if (progress == null)
            return NotFound("Progress not found.");

        progress.ProgressPercent = 100;
        progress.IsCompleted = true;
        progress.Score = request.Score;
        progress.LastAccess = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified);

        await _db.SaveChangesAsync();

        return Ok(new
        {
            message = "Lesson completed",
            progress.ProgressPercent,
            progress.IsCompleted,
            progress.Score
        });
    }
}

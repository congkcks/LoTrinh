using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service2.Data;
using Service2.Models;
namespace Service2.Controllers;
[ApiController]
[Route("api/user-highlights")]
public class UserHighlightsController : ControllerBase
{
    private readonly EngAceDbContext _db;

    public UserHighlightsController(EngAceDbContext db)
    {
        _db = db;
    }

    [HttpGet("{userId}/{lessonId}")]
    public async Task<IActionResult> GetHighlights(int userId, int lessonId)
    {
        var highlights = await _db.UserHighlights
            .Where(h => h.UserId == userId && h.LessonId == lessonId)
            .ToListAsync();

        return Ok(highlights);
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddHighlight([FromBody] UserHighlight request)
    {
        request.CreatedAt = DateTime.UtcNow;

        _db.UserHighlights.Add(request);
        await _db.SaveChangesAsync();

        return Ok(new
        {
            message = "Highlight added successfully",
            highlight = request
        });
    }

    [HttpPut("update/{id}")]
    public async Task<IActionResult> UpdateHighlight(int id, [FromBody] UserHighlight request)
    {
        var highlight = await _db.UserHighlights.FindAsync(id);

        if (highlight == null)
            return NotFound("Highlight not found");

        highlight.StartIndex = request.StartIndex;
        highlight.EndIndex = request.EndIndex;
        highlight.Color = request.Color;

        await _db.SaveChangesAsync();

        return Ok(new
        {
            message = "Highlight updated successfully",
            highlight
        });
    }

    // DELETE highlight
    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteHighlight(int id)
    {
        var highlight = await _db.UserHighlights.FindAsync(id);

        if (highlight == null)
            return NotFound("Highlight not found");

        _db.UserHighlights.Remove(highlight);
        await _db.SaveChangesAsync();

        return Ok(new { message = "Highlight deleted successfully" });
    }
}

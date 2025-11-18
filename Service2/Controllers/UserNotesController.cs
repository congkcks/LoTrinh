using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service2.Data;
using Service2.Models;
namespace Service2.Controllers;
[ApiController]
[Route("api/user-notes")]
public class UserNotesController : ControllerBase
{
    private readonly EngAceDbContext _db;

    public UserNotesController(EngAceDbContext db)
    {
        _db = db;
    }

    // GET notes by userId & lessonId
    [HttpGet("{userId}/{lessonId}")]
    public async Task<IActionResult> GetNotes(int userId, int lessonId)
    {
        var notes = await _db.UserNotes
            .Where(n => n.UserId == userId && n.LessonId == lessonId)
            .ToListAsync();

        return Ok(notes);
    }

    // POST add note
    [HttpPost("add")]
    public async Task<IActionResult> AddNote([FromBody] UserNote request)
    {
        request.CreatedAt = DateTime.UtcNow;

        _db.UserNotes.Add(request);
        await _db.SaveChangesAsync();

        return Ok(new
        {
            message = "Note added successfully",
            note = request
        });
    }

    // PUT update note
    [HttpPut("update/{id}")]
    public async Task<IActionResult> UpdateNote(int id, [FromBody] UserNote request)
    {
        var note = await _db.UserNotes.FindAsync(id);
        if (note == null) return NotFound("Note not found.");

        note.Note = request.Note;
        note.CreatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return Ok(new { message = "Note updated", note });
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteNote(int id)
    {
        var note = await _db.UserNotes.FindAsync(id);
        if (note == null) return NotFound();

        _db.UserNotes.Remove(note);
        await _db.SaveChangesAsync();

        return Ok(new { message = "Note deleted" });
    }
}

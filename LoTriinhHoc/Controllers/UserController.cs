using LoTriinhHoc.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LoTriinhHoc.Controllers;

[ApiController]
[Route("api/user")]
public class UserController : ControllerBase
{
    private readonly LotrinhhocDbContext _db;

    public UserController(LotrinhhocDbContext db)
    {
        _db = db;
    }

    [HttpGet("profile/{userId}")]
    public async Task<IActionResult> GetProfile(int userId)
    {
        var user = await _db.Users
            .Where(x => x.Id == userId)
            .Select(x => new
            {
                x.Id,
                x.Username,
                x.Email
            })
            .FirstOrDefaultAsync();

        if (user == null)
            return NotFound("User not found.");

        return Ok(user);
    }
}

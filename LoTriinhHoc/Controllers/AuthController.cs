using LoTriinhHoc.Data;
using LoTriinhHoc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RegisterRequest = LoTriinhHoc.DTOs.RegisterRequest;
using LoginRequest = LoTriinhHoc.DTOs.LoginRequest;
[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly LotrinhhocDbContext _db;

    public AuthController(LotrinhhocDbContext db)
    {
        _db = db;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        // Kiểm tra username tồn tại
        if (await _db.Users.AnyAsync(x => x.Username == request.Username))
            return BadRequest("Username already exists.");

        // Lưu trực tiếp password (DEMO)
        var user = new User
        {
            Username = request.Username,
            Email = request.Email,
            PasswordHash = request.Password
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        return Ok(new { message = "Registered successfully!", userId = user.Id });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var user = await _db.Users
            .FirstOrDefaultAsync(x => x.Username == request.Username);

        if (user == null)
            return Unauthorized("Wrong username or password.");

        if (user.PasswordHash != request.Password)
            return Unauthorized("Wrong username or password.");

        return Ok(new
        {
            message = "Login success",
            user = new
            {
                user.Id,
                user.Username,
                user.Email
            }
        });
    }
}

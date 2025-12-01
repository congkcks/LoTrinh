using LoTriinhHoc.Data;
using LoTriinhHoc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LoginRequest = LoTriinhHoc.DTOs.LoginRequest;
using RegisterRequest = LoTriinhHoc.DTOs.RegisterRequest;

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
        if (await _db.Users.AnyAsync(x => x.Username == request.Username))
            return BadRequest("Username already exists.");

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
        if (string.IsNullOrWhiteSpace(request.Email))
            return BadRequest("Email là bắt buộc!");

        var user = await _db.Users
            .FirstOrDefaultAsync(x => x.Email == request.Email);

        if (user == null)
            return Unauthorized("Không tìm thấy tài khoản với email này.");

        return Ok(new
        {
            message = "Đăng nhập thành công",
            user = new
            {
                id = user.Id,
                email = user.Email,
                username = user.Username
            }
        });
    }


}

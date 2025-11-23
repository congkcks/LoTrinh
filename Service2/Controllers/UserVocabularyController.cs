using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service2.Data;
using Service2.Models;
namespace Service2.Controllers;
[ApiController]
[Route("api/user-vocabulary")]
public class UserVocabularyController : ControllerBase
{
    private readonly EngAceDbContext _db;

    public UserVocabularyController(EngAceDbContext db)
    {
        _db = db;
    }
    [HttpGet("{userId}")]
    public async Task<IActionResult> GetByUser(int userId)
    {
        var vocab = await _db.UserVocabularies
            .Where(x => x.UserId == userId)
            .ToListAsync();

        return Ok(vocab);
    }

    // ✅ Lấy thông tin từ vựng cụ thể
    [HttpGet("{userId}/{vocabId}")]
    public async Task<IActionResult> GetOne(int userId, int vocabId)
    {
        var item = await _db.UserVocabularies
            .FirstOrDefaultAsync(x => x.UserId == userId && x.VocabId == vocabId);

        if (item == null)
            return Ok(null);

        return Ok(item);
    }

    // ✅ Thêm từ mới vào danh sách học
    [HttpPost("add")]
    public async Task<IActionResult> AddVocabulary([FromBody] UserVocabulary request)
    {
        var exists = await _db.UserVocabularies
            .FirstOrDefaultAsync(x =>
                x.UserId == request.UserId &&
                x.VocabId == request.VocabId);

        if (exists != null)
            return BadRequest("Từ vựng này đã tồn tại.");

        request.Status = request.Status ?? "learning";
        request.AddedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified);

        _db.UserVocabularies.Add(request);
        await _db.SaveChangesAsync();

        return Ok(new
        {
            message = "Đã thêm từ vựng cho user",
            data = request
        });
    }

    [HttpPut("{userId}/{vocabId}/status")]
    public async Task<IActionResult> UpdateStatus(
        int userId,
        int vocabId,
        [FromBody] string status)
    {
        var item = await _db.UserVocabularies
            .FirstOrDefaultAsync(x =>
                x.UserId == userId &&
                x.VocabId == vocabId);

        if (item == null)
        {
            // Nếu chưa có thì tạo mới
            item = new UserVocabulary
            {
                UserId = userId,
                VocabId = vocabId,
                Status = status,
                AddedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified)
            };

            _db.UserVocabularies.Add(item);
        }
        else
        {
            item.Status = status;
        }

        await _db.SaveChangesAsync();

        return Ok(new
        {
            message = "Cập nhật trạng thái từ vựng thành công",
            data = item
        });
    }

    // ✅ Xóa một từ khỏi danh sách user
    [HttpDelete("{userId}/{vocabId}")]
    public async Task<IActionResult> Remove(int userId, int vocabId)
    {
        var item = await _db.UserVocabularies
            .FirstOrDefaultAsync(x =>
                x.UserId == userId &&
                x.VocabId == vocabId);

        if (item == null)
            return NotFound("Không tồn tại bản ghi.");

        _db.UserVocabularies.Remove(item);
        await _db.SaveChangesAsync();

        return Ok("Xóa thành công.");
    }
}

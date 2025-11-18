using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LoTriinhHoc.Models;
using LoTriinhHoc.Data;
namespace LoTriinhHoc.Api.Controllers;
[ApiController]
[Route("api/learning-plan")]
public class LearningPlanController : ControllerBase
{
    private readonly LotrinhhocDbContext _db;

    public LearningPlanController(LotrinhhocDbContext db)
    {
        _db = db;
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetPlans(int userId)
    {
        var plans = await _db.LearningPlans
            .Where(p => p.UserId == userId)
            .ToListAsync();

        return Ok(plans);
    }

    // POST: tạo plan mới
    [HttpPost("create")]
    public async Task<IActionResult> CreatePlan([FromBody] LearningPlan request)
    {
        request.CreatedAt = DateTime.UtcNow;
        request.IsActive = true;

        _db.LearningPlans.Add(request);
        await _db.SaveChangesAsync();

        return Ok(new { message = "Learning plan created", request });
    }

    // PUT: cập nhật plan
    [HttpPut("update/{id}")]
    public async Task<IActionResult> UpdatePlan(int id, [FromBody] LearningPlan request)
    {
        var plan = await _db.LearningPlans.FindAsync(id);

        if (plan == null) return NotFound("Plan not found.");

        plan.Title = request.Title;
        plan.Description = request.Description;
        plan.Goal = request.Goal;
        plan.StartDate = request.StartDate;
        plan.EndDate = request.EndDate;

        await _db.SaveChangesAsync();

        return Ok(new { message = "Plan updated", plan });
    }

    // DELETE plan
    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeletePlan(int id)
    {
        var plan = await _db.LearningPlans.FindAsync(id);
        if (plan == null) return NotFound();

        _db.LearningPlans.Remove(plan);
        await _db.SaveChangesAsync();

        return Ok(new { message = "Learning plan deleted" });
    }
}

using LoTriinhHoc.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LoTriinhHoc.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CoursesController : ControllerBase
{
    private readonly LotrinhhocDbContext _context;

    public CoursesController(LotrinhhocDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCourses()
    {
        var courses = await _context.Courses
            .Select(c => new
            {
                c.Id,
                c.Name
            })
            .ToListAsync();

        return Ok(courses);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCourseDetail(int id)
    {
        var course = await _context.Courses.FirstOrDefaultAsync(c => c.Id == id);
        if (course == null) return NotFound();

        var modules = await _context.Modules
            .Where(m => m.CourseId == id)
            .Select(m => new
            {
                m.Id,
                m.Name,
                Lessons = _context.Lessons
                    .Where(l => l.ModuleId == m.Id)
                    .Select(l => new
                    {
                        l.Id,
                        l.Title
                    })
                    .ToList()
            })
            .ToListAsync();

        return Ok(new
        {
            course.Id,
            course.Name,
            Modules = modules
        });
    }
}

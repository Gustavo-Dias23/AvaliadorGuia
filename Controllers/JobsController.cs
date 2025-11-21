using AvaliadorGuia.Api.Data;
using AvaliadorGuia.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AvaliadorGuia.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class JobsController : ControllerBase
{
    private readonly AppDbContext _context;

    public JobsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Job>>> GetAll()
    {
        var jobs = await _context.Jobs
            .AsNoTracking()
            .ToListAsync();

        return Ok(jobs);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Job>> GetById(int id)
    {
        var job = await _context.Jobs.FindAsync(id);
        if (job is null) return NotFound();

        return Ok(job);
    }

    [HttpPost]
    public async Task<ActionResult<Job>> Create([FromBody] Job job)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        _context.Jobs.Add(job);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = job.Id }, job);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] Job job)
    {
        if (id != job.Id) return BadRequest("Id do corpo difere do parâmetro.");
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var exists = await _context.Jobs.AnyAsync(j => j.Id == id);
        if (!exists) return NotFound();

        _context.Entry(job).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var job = await _context.Jobs.FindAsync(id);
        if (job is null) return NotFound();

        _context.Jobs.Remove(job);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}

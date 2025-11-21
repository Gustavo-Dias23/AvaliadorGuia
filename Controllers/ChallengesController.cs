using AvaliadorGuia.Api.Data;
using AvaliadorGuia.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AvaliadorGuia.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class ChallengesController : ControllerBase
{
    private readonly AppDbContext _context;

    public ChallengesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Challenge>>> GetAll()
    {
        var list = await _context.Challenges
            .Include(c => c.Job)
            .AsNoTracking()
            .ToListAsync();

        return Ok(list);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Challenge>> GetById(int id)
    {
        var challenge = await _context.Challenges
            .Include(c => c.Job)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (challenge is null) return NotFound();

        return Ok(challenge);
    }

    [HttpPost]
    public async Task<ActionResult<Challenge>> Create([FromBody] Challenge challenge)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var jobExists = await _context.Jobs.AnyAsync(j => j.Id == challenge.JobId);
        if (!jobExists) return BadRequest("JobId inválido.");

        _context.Challenges.Add(challenge);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = challenge.Id }, challenge);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] Challenge challenge)
    {
        if (id != challenge.Id) return BadRequest("Id do corpo difere do parâmetro.");
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var exists = await _context.Challenges.AnyAsync(c => c.Id == id);
        if (!exists) return NotFound();

        _context.Entry(challenge).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var challenge = await _context.Challenges.FindAsync(id);
        if (challenge is null) return NotFound();

        _context.Challenges.Remove(challenge);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}

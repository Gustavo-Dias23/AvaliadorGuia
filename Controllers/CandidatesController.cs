using AvaliadorGuia.Api.Data;
using AvaliadorGuia.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AvaliadorGuia.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class CandidatesController : ControllerBase
{
    private readonly AppDbContext _context;

    public CandidatesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Candidate>>> GetAll()
    {
        var candidates = await _context.Candidates
            .AsNoTracking()
            .ToListAsync();

        return Ok(candidates);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Candidate>> GetById(int id)
    {
        var candidate = await _context.Candidates.FindAsync(id);
        if (candidate is null) return NotFound();

        return Ok(candidate);
    }

    [HttpPost]
    public async Task<ActionResult<Candidate>> Create([FromBody] Candidate candidate)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        _context.Candidates.Add(candidate);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = candidate.Id }, candidate);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] Candidate candidate)
    {
        if (id != candidate.Id) return BadRequest("Id do corpo difere do parâmetro.");
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var exists = await _context.Candidates.AnyAsync(c => c.Id == id);
        if (!exists) return NotFound();

        _context.Entry(candidate).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var candidate = await _context.Candidates.FindAsync(id);
        if (candidate is null) return NotFound();

        _context.Candidates.Remove(candidate);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}

using AvaliadorGuia.Api.Data;
using AvaliadorGuia.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AvaliadorGuia.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class SessionsController : ControllerBase
{
    private readonly AppDbContext _context;

    public SessionsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Session>>> GetAll()
    {
        var sessions = await _context.Sessions
            .Include(s => s.Candidate)
            .Include(s => s.Challenge)
            .Include(s => s.Hints)
            .AsNoTracking()
            .ToListAsync();

        return Ok(sessions);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Session>> GetById(int id)
    {
        var session = await _context.Sessions
            .Include(s => s.Candidate)
            .Include(s => s.Challenge)
            .Include(s => s.Hints)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (session is null) return NotFound();

        return Ok(session);
    }

    public record CreateSessionRequest(int CandidateId, int ChallengeId);

    [HttpPost]
    public async Task<ActionResult<Session>> Create([FromBody] CreateSessionRequest request)
    {
        var candidateExists = await _context.Candidates.AnyAsync(c => c.Id == request.CandidateId);
        var challengeExists = await _context.Challenges.AnyAsync(c => c.Id == request.ChallengeId);

        if (!candidateExists) return BadRequest("CandidateId inválido.");
        if (!challengeExists) return BadRequest("ChallengeId inválido.");

        var session = new Session
        {
            CandidateId = request.CandidateId,
            ChallengeId = request.ChallengeId,
            Status = SessionStatus.EmAndamento,
            StartedAt = DateTime.UtcNow
        };

        _context.Sessions.Add(session);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = session.Id }, session);
    }

    public record FinishSessionRequest(string FinalCode);

    [HttpPut("{id:int}/finish")]
    public async Task<IActionResult> Finish(int id, [FromBody] FinishSessionRequest request)
    {
        var session = await _context.Sessions.FindAsync(id);
        if (session is null) return NotFound();

        if (session.Status == SessionStatus.Finalizada)
            return BadRequest("Sessão já finalizada.");

        session.FinalCode = request.FinalCode;
        session.Status = SessionStatus.Finalizada;
        session.FinishedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return NoContent();
    }
}

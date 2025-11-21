using AvaliadorGuia.Api.Data;
using AvaliadorGuia.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AvaliadorGuia.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class HintsController : ControllerBase
{
    private readonly AppDbContext _context;

    public HintsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("session/{sessionId:int}")]
    public async Task<ActionResult<IEnumerable<Hint>>> GetBySession(int sessionId)
    {
        var exists = await _context.Sessions.AnyAsync(s => s.Id == sessionId);
        if (!exists) return NotFound("Sessão não encontrada.");

        var hints = await _context.Hints
            .Where(h => h.SessionId == sessionId)
            .AsNoTracking()
            .ToListAsync();

        return Ok(hints);
    }

    public record CreateHintRequest(
        int SessionId,
        HintDifficultyArea DifficultyArea,
        string CandidateQuestion
    );

    [HttpPost]
    public async Task<ActionResult<Hint>> Create([FromBody] CreateHintRequest request)
    {
        var session = await _context.Sessions.FindAsync(request.SessionId);
        if (session is null) return BadRequest("SessionId inválido.");

        if (session.Status == SessionStatus.Finalizada)
            return BadRequest("Não é possível solicitar dica para sessão finalizada.");

        string tutorAnswer = request.DifficultyArea switch
        {
            HintDifficultyArea.Logica =>
                "Pense na estrutura do problema: quais são as entradas, saídas e passos intermediários?",
            HintDifficultyArea.Otimizacao =>
                "Reflita sobre a complexidade: há algum loop aninhado que pode ser substituído por uma estrutura mais eficiente?",
            HintDifficultyArea.EdgeCases =>
                "Quais são os casos extremos? Lista vazia, valores nulos, tamanho máximo de entrada, etc.",
            _ => "Tente explicar em voz alta o que seu código faz, linha a linha."
        };

        var hint = new Hint
        {
            SessionId = request.SessionId,
            DifficultyArea = request.DifficultyArea,
            CandidateQuestion = request.CandidateQuestion,
            TutorAnswer = tutorAnswer,
            CreatedAt = DateTime.UtcNow
        };

        _context.Hints.Add(hint);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetBySession), new { sessionId = request.SessionId }, hint);
    }
}

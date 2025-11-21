using System.ComponentModel.DataAnnotations;

namespace AvaliadorGuia.Api.Models;

public class Session
{
    public int Id { get; set; }

    [Required]
    public int CandidateId { get; set; }
    public Candidate Candidate { get; set; } = null!;

    [Required]
    public int ChallengeId { get; set; }
    public Challenge Challenge { get; set; } = null!;

    public DateTime StartedAt { get; set; } = DateTime.UtcNow;
    public DateTime? FinishedAt { get; set; }

    [Required]
    public SessionStatus Status { get; set; } = SessionStatus.EmAndamento;

    public string? FinalCode { get; set; }

    public ICollection<Hint> Hints { get; set; } = new List<Hint>();
}

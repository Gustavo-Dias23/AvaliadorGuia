using System.ComponentModel.DataAnnotations;

namespace AvaliadorGuia.Api.Models;

public class Hint
{
    public int Id { get; set; }

    [Required]
    public int SessionId { get; set; }
    public Session Session { get; set; } = null!;

    [Required]
    public HintDifficultyArea DifficultyArea { get; set; }

    [Required, MaxLength(500)]
    public string CandidateQuestion { get; set; } = null!;

    [Required, MaxLength(1000)]
    public string TutorAnswer { get; set; } = null!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

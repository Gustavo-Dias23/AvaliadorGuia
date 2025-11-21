using System.ComponentModel.DataAnnotations;

namespace AvaliadorGuia.Api.Models;

public class Challenge
{
    public int Id { get; set; }

    [Required, MaxLength(150)]
    public string Title { get; set; } = null!;

    [Required]
    public string Statement { get; set; } = null!;

    [MaxLength(50)]
    public string? Language { get; set; }

    [MaxLength(50)]
    public string? Difficulty { get; set; }

    [Required]
    public int JobId { get; set; }
    public Job Job { get; set; } = null!;

    public ICollection<Session> Sessions { get; set; } = new List<Session>();
}

using System.ComponentModel.DataAnnotations;

namespace AvaliadorGuia.Api.Models;

public class Job
{
    public int Id { get; set; }

    [Required, MaxLength(150)]
    public string Title { get; set; } = null!;

    [Required, MaxLength(500)]
    public string Description { get; set; } = null!;

    [MaxLength(200)]
    public string? TechStack { get; set; }

    public ICollection<Challenge> Challenges { get; set; } = new List<Challenge>();
}

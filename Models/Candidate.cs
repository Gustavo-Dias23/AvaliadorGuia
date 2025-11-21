using System.ComponentModel.DataAnnotations;

namespace AvaliadorGuia.Api.Models;

public class Candidate
{
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public string Name { get; set; } = null!;

    [Required, EmailAddress, MaxLength(150)]
    public string Email { get; set; } = null!;

    [Required]
    public SeniorityLevel Seniority { get; set; }

    public ICollection<Session> Sessions { get; set; } = new List<Session>();
}

using System.ComponentModel.DataAnnotations;

namespace TrainingTracker.Domain.Entities;

public abstract class AuditableEntity : BaseEntity
{
    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }
}

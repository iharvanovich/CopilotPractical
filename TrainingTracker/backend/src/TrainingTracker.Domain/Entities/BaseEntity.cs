using System.ComponentModel.DataAnnotations;

namespace TrainingTracker.Domain.Entities;

public abstract class BaseEntity
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
}

namespace SHS.StudentPortal.Domain.Models;

public class OutboxMessage : BaseModel<Guid>
{
    public string Type { get; set; }

    public string Data { get; set; }

    public DateTime OccuredOn { get; set; }

    public DateTime? ProcessedOn { get; set; }

    public string? Error { get; set; }
}

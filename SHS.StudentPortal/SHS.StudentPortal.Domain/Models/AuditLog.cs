namespace SHS.StudentPortal.Domain.Models;

public class AuditLog : BaseModel<Guid>
{
    public string ObjectId { get; set; }

    public string ObjectType { get; set; }

    public string Event { get; set; }

    public string ActionById { get; set; }

    public DateTime OccurredOn { get; set; }

    public string Data { get; set; }
}

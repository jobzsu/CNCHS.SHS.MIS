namespace SHS.StudentPortal.Domain.Models;

public class BaseModel<TId>
{
    public TId Id { get; set; }

    readonly List<IDomainEvent> _domainEvents = new();

    public IReadOnlyCollection<IDomainEvent> DomainEvents() => _domainEvents.AsReadOnly().ToList();

    protected void RegisterEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);

    public void ClearDomainEvents() => _domainEvents.Clear();
}

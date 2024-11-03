namespace SHS.StudentPortal.Domain.Events.User;

public sealed record UserCreatedEvent(Domain.Models.User user)  : IDomainEvent;

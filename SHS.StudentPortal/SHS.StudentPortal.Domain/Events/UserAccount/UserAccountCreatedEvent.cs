namespace SHS.StudentPortal.Domain.Events.UserAccount;

public sealed record UserAccountCreatedEvent(Models.UserAccount userAccount) : IDomainEvent;

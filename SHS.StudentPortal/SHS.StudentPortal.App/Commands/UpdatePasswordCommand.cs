using SHS.StudentPortal.App.Abstractions.Messaging;

namespace SHS.StudentPortal.App.Commands;

public sealed record UpdatePasswordCommand(Guid userId, string newPassword)
    : ICommand;

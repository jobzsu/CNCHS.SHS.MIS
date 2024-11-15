using SHS.StudentPortal.App.Abstractions.Messaging;

namespace SHS.StudentPortal.App.Commands;

public sealed record UpdateInstructorPasswordCommand(string newPassword, Guid instructorId, Guid updatedBy)
    : ICommand;

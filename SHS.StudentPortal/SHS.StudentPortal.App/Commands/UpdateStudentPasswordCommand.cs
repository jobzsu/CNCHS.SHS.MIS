using SHS.StudentPortal.App.Abstractions.Messaging;

namespace SHS.StudentPortal.App.Commands;

public sealed record UpdateStudentPasswordCommand(string newPassword, Guid studentId, Guid updatedBy)
    : ICommand;

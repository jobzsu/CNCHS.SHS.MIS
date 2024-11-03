using SHS.StudentPortal.App.Abstractions.Messaging;

namespace SHS.StudentPortal.App.Commands;

public sealed record MarkStudentForEnrollmentCommand(Guid studentId, Guid verifiedBy)
    : ICommand;

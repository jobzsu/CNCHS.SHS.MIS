using SHS.StudentPortal.App.Abstractions.Messaging;

namespace SHS.StudentPortal.App.Commands;

public sealed record ApproveStudentCommand(Guid studentId, Guid approvedById)
    : ICommand;

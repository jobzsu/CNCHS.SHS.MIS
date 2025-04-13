using SHS.StudentPortal.App.Abstractions.Messaging;

namespace SHS.StudentPortal.App.Commands;

public sealed record UpdateSystemSettingsCommand(string academicYear, string semester, Guid actionById)
    : ICommand;

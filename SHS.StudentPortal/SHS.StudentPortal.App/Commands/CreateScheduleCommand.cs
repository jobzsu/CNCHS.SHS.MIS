using SHS.StudentPortal.App.Abstractions.Messaging;
using SHS.StudentPortal.Common.Models;

namespace SHS.StudentPortal.App.Commands;

public sealed record CreateScheduleCommand(CreateScheduleViewModel view, Guid createdById)
    : ICommand;

using SHS.StudentPortal.App.Abstractions.Messaging;
using SHS.StudentPortal.Common.Models;

namespace SHS.StudentPortal.App.Commands;

public sealed record UpdateDepartmentCommand(DepartmentViewModel view, Guid updatedById)
 : ICommand;

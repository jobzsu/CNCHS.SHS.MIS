using SHS.StudentPortal.App.Abstractions.Messaging;
using SHS.StudentPortal.Common.Models.Student;

namespace SHS.StudentPortal.App.Queries;

public sealed record GetStudentProfileByUserIdQuery(Guid studentUserId)
    : IQuery<StudentInfoAdminViewDTO>;

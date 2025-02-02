using SHS.StudentPortal.App.Abstractions.Messaging;
using SHS.StudentPortal.Common.Models;

namespace SHS.StudentPortal.App.Queries;

public sealed record GetSubjectByIdQuery(int subjectId)
    : IQuery<SubjectViewModel>;

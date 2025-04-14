using SHS.StudentPortal.App.Abstractions.Messaging;

namespace SHS.StudentPortal.App.Queries;

public sealed record GetInstructorDropdownListQuery :
    IQuery<List<KeyValuePair<Guid, string>>>;

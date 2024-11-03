using SHS.StudentPortal.App.Abstractions.Messaging;

namespace SHS.StudentPortal.App.Queries;

public sealed record GetSectionListQuery() : IQuery<List<KeyValuePair<string, string>>>;

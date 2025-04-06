using SHS.StudentPortal.App.Abstractions.Messaging;

namespace SHS.StudentPortal.App.Queries;

public sealed record GetSectionListQuery(bool includeNotApplicable = false) : IQuery<List<KeyValuePair<Guid, string>>>;

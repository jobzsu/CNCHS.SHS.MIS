﻿using SHS.StudentPortal.App.Abstractions.Messaging;

namespace SHS.StudentPortal.App.Queries;

public sealed record GetSubjectDropdownListQuery(bool includeOther = true)
    : IQuery<List<KeyValuePair<int, string>>>;

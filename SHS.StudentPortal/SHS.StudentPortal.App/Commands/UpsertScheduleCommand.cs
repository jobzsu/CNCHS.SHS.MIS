﻿using SHS.StudentPortal.App.Abstractions.Messaging;
using SHS.StudentPortal.Common.Models;

namespace SHS.StudentPortal.App.Commands;

public sealed record UpsertScheduleCommand(UpsertScheduleViewModel view, Guid actionById)
    : ICommand;

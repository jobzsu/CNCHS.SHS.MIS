﻿using SHS.StudentPortal.App.Abstractions.Messaging;
using SHS.StudentPortal.Common.Models;

namespace SHS.StudentPortal.App.Commands;

public sealed record RegisterStudentCommand(RegisterStudentViewModel model)
    : ICommand;

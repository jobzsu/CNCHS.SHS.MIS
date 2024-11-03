using SHS.StudentPortal.App.Abstractions.Messaging;
using SHS.StudentPortal.Common.Models;

namespace SHS.StudentPortal.App.Commands;

public sealed record AuthenticateLoginViewModelCommand(string userAccountType, string roleType, string username, string password) 
    : ICommand<UserPrincipalViewModel>;

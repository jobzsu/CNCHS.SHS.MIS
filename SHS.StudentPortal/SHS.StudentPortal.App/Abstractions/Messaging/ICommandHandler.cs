using MediatR;
using SHS.StudentPortal.Common.Models;

namespace SHS.StudentPortal.App.Abstractions.Messaging;

public interface ICommandHandler<TCommand> : IRequestHandler<TCommand, ResultModel>
    where TCommand : ICommand
{ }

public interface ICommandHandler<TCommand, TResponse> : IRequestHandler<TCommand, ResultModel<TResponse>>
    where TCommand : ICommand<TResponse>
{ }

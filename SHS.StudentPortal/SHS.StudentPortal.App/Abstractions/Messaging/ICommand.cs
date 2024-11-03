using MediatR;
using SHS.StudentPortal.Common.Models;

namespace SHS.StudentPortal.App.Abstractions.Messaging;

public interface ICommand : IRequest<ResultModel>
{
}

public interface ICommand<TResponse> : IRequest<ResultModel<TResponse>>
{
}
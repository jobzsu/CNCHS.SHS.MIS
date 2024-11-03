using MediatR;
using SHS.StudentPortal.Common.Models;

namespace SHS.StudentPortal.App.Abstractions.Messaging;

public interface IQuery<TResponse> : IRequest<ResultModel<TResponse>>
{
}

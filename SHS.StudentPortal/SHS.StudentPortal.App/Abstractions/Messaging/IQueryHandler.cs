using MediatR;
using SHS.StudentPortal.Common.Models;

namespace SHS.StudentPortal.App.Abstractions.Messaging;

public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, ResultModel<TResponse>>
    where TQuery: IQuery<TResponse>
{ 
}

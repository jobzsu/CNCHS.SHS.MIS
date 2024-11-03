using Microsoft.Extensions.Logging;
using SHS.StudentPortal.App.Abstractions.Messaging;
using SHS.StudentPortal.App.Abstractions.Repositories;
using SHS.StudentPortal.Common.Models;
using SHS.StudentPortal.Common.Utilities.Models;

namespace SHS.StudentPortal.App.Queries;

internal sealed class GetPaginatedDepartmentListQueryHandler
    : IQueryHandler<GetPaginatedDepartmentListQuery, PaginatedList<DepartmentListViewModel>>
{
    private readonly ILogger<GetPaginatedDepartmentListQueryHandler> _logger;
    private readonly IDepartmentRepository _departmentRepository;

    public GetPaginatedDepartmentListQueryHandler(ILogger<GetPaginatedDepartmentListQueryHandler> logger, 
        IDepartmentRepository departmentRepository)
    {
        _logger = logger;
        _departmentRepository = departmentRepository;
    }

    public async Task<ResultModel<PaginatedList<DepartmentListViewModel>>> Handle(GetPaginatedDepartmentListQuery request, CancellationToken cancellationToken)
    {
        ErrorModel error;

        try
        {
            var departments = await _departmentRepository.GetListViaFilter(request.filter.SearchKeyword,
                cancellationToken: cancellationToken);

            if(departments is null || departments.Count() == 0)
            {
                var paginatedList = PaginatedList<DepartmentListViewModel>
                    .Create(new List<DepartmentListViewModel>().AsQueryable(), 1, 10);

                return ResultModel<PaginatedList<DepartmentListViewModel>>.Success(paginatedList);
            }
            else
            {
                var departmentListViewModel = new List<DepartmentListViewModel>();

                departmentListViewModel.AddRange(departments.Select(x => new DepartmentListViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    EmployeeCount = x.Instructors.Count()
                }));

                var paginatedList = PaginatedList<DepartmentListViewModel>
                    .Create(departmentListViewModel.AsQueryable(), request.filter.PageNumber, 10);

                return ResultModel<PaginatedList<DepartmentListViewModel>>.Success(paginatedList);
            }

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving Department List");

            error = new(nameof(Exception), "Error retrieving Department List.");

            return ResultModel<PaginatedList<DepartmentListViewModel>>.Fail(error);
        }
    }
}

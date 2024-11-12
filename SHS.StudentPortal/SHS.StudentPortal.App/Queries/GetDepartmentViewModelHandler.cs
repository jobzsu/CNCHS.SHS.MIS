using Microsoft.Extensions.Logging;
using SHS.StudentPortal.App.Abstractions.Messaging;
using SHS.StudentPortal.App.Abstractions.Repositories;
using SHS.StudentPortal.Common.Models;

namespace SHS.StudentPortal.App.Queries;

internal sealed class GetDepartmentViewModelHandler
    : IQueryHandler<GetDepartmentViewModel, DepartmentViewModel>
{
    private readonly ILogger<GetDepartmentViewModelHandler> _logger;
    private readonly IDepartmentRepository _departmentRepository;
    private readonly IInstructorInfoRepository _instructorInfoRepository;

    public GetDepartmentViewModelHandler(ILogger<GetDepartmentViewModelHandler> logger,
        IDepartmentRepository departmentRepository,
        IInstructorInfoRepository instructorInfoRepository)
    {
        _logger = logger;
        _departmentRepository = departmentRepository;
        _instructorInfoRepository = instructorInfoRepository;
    }

    public async Task<ResultModel<DepartmentViewModel>> Handle(GetDepartmentViewModel request, CancellationToken cancellationToken)
    {
        ErrorModel error;

        try
        {
            var department = await _departmentRepository.GetDepartmentById(request.departmentId, cancellationToken: cancellationToken);

            if (department is null)
            {
                error = new(nameof(KeyNotFoundException), "Department not found.");

                return ResultModel<DepartmentViewModel>.Fail(error);
            }

            var instructors = await _instructorInfoRepository.GetInstructorsByDepartment(request.departmentId, cancellationToken: cancellationToken);

            var departmentViewModel = new DepartmentViewModel()
            {
                Id = department.Id,
                Name = department.Name,
                Instructors = new()
            };

            if(instructors is not null && instructors.Any())
            {
                departmentViewModel.Instructors.AddRange(instructors.Select(i =>
                {
                    return i.User.MiddleName is null ?
                        $"Prof. {i.User.FirstName} {i.User.LastName}" :
                        $"Prof. {i.User.FirstName} {i.User.MiddleName} {i.User.LastName}";
                }));
            }

            return ResultModel<DepartmentViewModel>.Success(departmentViewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error Viewing Department");

            error = new(nameof(Exception), ex.Message);

            return ResultModel<DepartmentViewModel>.Fail(error);
        }
    }
}

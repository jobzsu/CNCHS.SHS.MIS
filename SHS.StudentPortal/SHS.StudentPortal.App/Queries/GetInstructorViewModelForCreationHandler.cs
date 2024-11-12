using Microsoft.Extensions.Logging;
using SHS.StudentPortal.App.Abstractions.Messaging;
using SHS.StudentPortal.App.Abstractions.Repositories;
using SHS.StudentPortal.Common.Models;
using SHS.StudentPortal.Domain.Models;

namespace SHS.StudentPortal.App.Queries;

internal sealed class GetInstructorViewModelForCreationHandler
    : IQueryHandler<GetInstructorViewModelForCreation, ForCreateInstructorViewModel>
{
	private readonly ILogger<GetInstructorViewModelForCreationHandler> _logger;
    private readonly IDepartmentRepository _departmentRepository;
    private readonly ISectionRepository _sectionRepository;

    public GetInstructorViewModelForCreationHandler(ILogger<GetInstructorViewModelForCreationHandler> logger,
        IDepartmentRepository departmentRepository,
        ISectionRepository sectionRepository)
    {
        _logger = logger;
        _departmentRepository = departmentRepository;
        _sectionRepository = sectionRepository;
    }

    public async Task<ResultModel<ForCreateInstructorViewModel>> Handle(GetInstructorViewModelForCreation request, CancellationToken cancellationToken)
    {
        ErrorModel error;

		try
		{
            var retVal = new ForCreateInstructorViewModel()
            {
                Id = Guid.Empty,
                EmployeeId = string.Empty,
                Major = string.Empty,
                FirstName = string.Empty,
                MiddleName = string.Empty,
                LastName = string.Empty,
                ContactInfo = string.Empty,
                DepartmentId = 0,
                DepartmentList = new(),
                AdvisorySectionId = Guid.Empty,
                SectionList = new(),
                Username = string.Empty,
                Password = string.Empty,
                ConfirmPassword = string.Empty,
                EmailAddress = string.Empty,
            };

            var departments = await _departmentRepository.GetAllDepartments(cancellationToken: cancellationToken);

            var sections = await _sectionRepository.GetAllSections(cancellationToken: cancellationToken);

            if(departments is not null && departments.Count > 0)
            {
                retVal.DepartmentList.AddRange(departments.Select(x => new KeyValuePair<int, string>(x.Id, x.Name)));
            }

            if(sections is not null && sections.Count > 0)
            {
                retVal.SectionList.AddRange(sections.Select(x => new KeyValuePair<Guid, string>(x.Id, x.Name)));
            }

            return ResultModel<ForCreateInstructorViewModel>.Success(retVal);
		}
		catch (Exception ex)
		{
            _logger.LogError(ex, "Error preparing Instructor View Model");

            error = new(nameof(Exception), "Error preparing Instructor View Model");

            return ResultModel<ForCreateInstructorViewModel>.Fail(error);
		}
    }
}

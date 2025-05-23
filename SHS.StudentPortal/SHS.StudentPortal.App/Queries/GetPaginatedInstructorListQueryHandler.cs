﻿using Microsoft.Extensions.Logging;
using SHS.StudentPortal.App.Abstractions.Messaging;
using SHS.StudentPortal.App.Abstractions.Repositories;
using SHS.StudentPortal.Common.Models;
using SHS.StudentPortal.Common.Utilities.Models;

namespace SHS.StudentPortal.App.Queries;

internal sealed class GetPaginatedInstructorListQueryHandler
    : IQueryHandler<GetPaginatedInstructorListQuery, InstructorPaginatedList>
{
	private readonly ILogger<GetPaginatedInstructorListQueryHandler> _logger;
    private readonly IInstructorInfoRepository _instructorInfoRepository;
    private readonly IDepartmentRepository _departmentRepository;
    private readonly ISectionRepository _sectionRepository;

    public GetPaginatedInstructorListQueryHandler(ILogger<GetPaginatedInstructorListQueryHandler> logger,
        IInstructorInfoRepository instructorInfoRepository,
        IDepartmentRepository departmentRepository,
        ISectionRepository sectionRepository)
    {
        _logger = logger;
        _instructorInfoRepository = instructorInfoRepository;
        _departmentRepository = departmentRepository;
        _sectionRepository = sectionRepository;
    }

    public async Task<ResultModel<InstructorPaginatedList>> Handle(GetPaginatedInstructorListQuery request, CancellationToken cancellationToken)
    {
        ErrorModel error;

		try
		{
            var instructorList = await _instructorInfoRepository.GetListViaFilter(request.filter.SearchKeyword,
                request.filter.DepartmentId,
                cancellationToken);

            var departmentList = await _departmentRepository.GetAllDepartments(cancellationToken: cancellationToken);
            var departmentListKeyValuePair = departmentList is null || departmentList.Count() == 0 ?
                [] : departmentList.Select(x => new KeyValuePair<int, string>(x.Id, x.Name.ToUpper())).ToList();

            var sectionList = await _sectionRepository.GetAllSections(true, cancellationToken: cancellationToken);

            if(sectionList is null || sectionList.Count() == 0)
            {
                error = new(nameof(Exception), "No Sections Found");

                return ResultModel<InstructorPaginatedList>.Fail(error);
            }

            if(instructorList is null || instructorList.Count() == 0)
            {
                var paginatedList = PaginatedList<InstructorListViewModel>
                    .Create(new List<InstructorListViewModel>().AsQueryable(), 1, 999999);

                var instructorPaginatedList = new InstructorPaginatedList
                {
                    DepartmentList = departmentListKeyValuePair,
                    InstructorList = paginatedList
                };

                return ResultModel<InstructorPaginatedList>.Success(instructorPaginatedList);
            }
            else
            {
                var instructorListViewModel = new List<InstructorListViewModel>();

                instructorListViewModel.AddRange(instructorList.Select(x => new InstructorListViewModel() {
                    Id = x.Id.ToString(),
                    EmployeeId = x.EmployeeId.PadLeft(7, '0'),
                    Name = string.IsNullOrWhiteSpace(x.User.MiddleName) ? $"{x.User.FirstName} {x.User.LastName}" : $"{x.User.FirstName} {x.User.MiddleName} {x.User.LastName}",
                    Department = x.Department.Name.ToUpper(),
                    AdvisorySection = sectionList.FirstOrDefault(s => s.AdviserId == x.Id)?.Name ?? "N/A"
                }));

                var paginatedList = PaginatedList<InstructorListViewModel>
                    .Create(instructorListViewModel.AsQueryable(), request.filter.PageNumber, 999999);

                var instructorPaginatedList = new InstructorPaginatedList
                {
                    DepartmentList = departmentListKeyValuePair,
                    InstructorList = paginatedList
                };

                return ResultModel<InstructorPaginatedList>.Success(instructorPaginatedList);
            }
		}
		catch (Exception ex)
		{
            _logger.LogError($"Error retrieving instructor list: {ex.Message}");

            error = new(nameof(ex), "Error retrieving instructor list.");

            return ResultModel<InstructorPaginatedList>.Fail(error);

        }
    }
}

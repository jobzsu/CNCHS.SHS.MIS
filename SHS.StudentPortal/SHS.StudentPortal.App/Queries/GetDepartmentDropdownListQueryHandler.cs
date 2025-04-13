using Microsoft.Extensions.Logging;
using SHS.StudentPortal.App.Abstractions.Messaging;
using SHS.StudentPortal.App.Abstractions.Repositories;
using SHS.StudentPortal.Common.Models;
using System.Collections.Generic;

namespace SHS.StudentPortal.App.Queries;

internal sealed class GetDepartmentDropdownListQueryHandler
    : IQueryHandler<GetDepartmentDropdownListQuery, List<KeyValuePair<int, string>>>
{
    private readonly ILogger<GetDepartmentDropdownListQueryHandler> _logger;
    private readonly IDepartmentRepository _departmentRepository;

    public GetDepartmentDropdownListQueryHandler(ILogger<GetDepartmentDropdownListQueryHandler> logger, 
        IDepartmentRepository departmentRepository)
    {
        _logger = logger;
        _departmentRepository = departmentRepository;
    }

    public async Task<ResultModel<List<KeyValuePair<int, string>>>> Handle(GetDepartmentDropdownListQuery request, CancellationToken cancellationToken)
    {
        ErrorModel error;

        try
        {
            var departmentList = await _departmentRepository.GetAllDepartments(cancellationToken: cancellationToken);

            var retVal = new List<KeyValuePair<int, string>>();

            if(departmentList is not null && departmentList.Any())
            {
                retVal = departmentList
                    .Select(x => new KeyValuePair<int, string>(x.Id, x.Name.ToUpper()))
                    .ToList();
            }

            return ResultModel<List<KeyValuePair<int, string>>>.Success(retVal);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retreiving Department List");

            error = new(nameof(Exception), "Error retreiving Department List");

            return ResultModel<List<KeyValuePair<int, string>>>.Fail(error);
        }
    }
}

using Microsoft.Extensions.Logging;
using SHS.StudentPortal.App.Abstractions.Messaging;
using SHS.StudentPortal.App.Abstractions.Repositories;
using SHS.StudentPortal.Common.Models;

namespace SHS.StudentPortal.App.Queries;

internal sealed class GetSubjectDropdownListQueryHandler
    : IQueryHandler<GetSubjectDropdownListQuery, List<KeyValuePair<int, string>>>
{
    private readonly ILogger<GetSubjectDropdownListQueryHandler> _logger;
    private readonly ISubjectRepository _subjectRepository;

    public GetSubjectDropdownListQueryHandler(ILogger<GetSubjectDropdownListQueryHandler> logger, 
        ISubjectRepository subjectRepository)
    {
        _logger = logger;
        _subjectRepository = subjectRepository;
    }

    public async Task<ResultModel<List<KeyValuePair<int, string>>>> Handle(GetSubjectDropdownListQuery request, CancellationToken cancellationToken)
    {
        ErrorModel error;

        try
        {
            var subjects = await _subjectRepository.GetAllSubjects(request.includeOther, cancellationToken);

            List<KeyValuePair<int, string>> retVal = new();

            if(subjects is not null && subjects.Any())
            {
                retVal.AddRange(subjects.Select(s =>
                {
                    return new KeyValuePair<int, string>(s.Id, s.Name);
                }));
            }

            return ResultModel<List<KeyValuePair<int, string>>>.Success(retVal);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving Subject Dropdown List");

            error = new(nameof(Exception), "Error retrieving Subject Dropdown List");

            return ResultModel<List<KeyValuePair<int, string>>>.Fail(error);
        }
    }
}

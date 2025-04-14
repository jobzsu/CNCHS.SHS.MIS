using Microsoft.Extensions.Logging;
using SHS.StudentPortal.App.Abstractions.Messaging;
using SHS.StudentPortal.App.Abstractions.Repositories;
using SHS.StudentPortal.Common;
using SHS.StudentPortal.Common.Models;

namespace SHS.StudentPortal.App.Queries;

internal sealed class GetInstructorDropdownListQueryHandler
    : IQueryHandler<GetInstructorDropdownListQuery, List<KeyValuePair<Guid, string>>>
{
    private readonly ILogger<GetInstructorDropdownListQueryHandler> _logger;
    private readonly IInstructorInfoRepository _instructorInfoRepository;

    public GetInstructorDropdownListQueryHandler(ILogger<GetInstructorDropdownListQueryHandler> logger, 
        IInstructorInfoRepository instructorInfoRepository)
    {
        _logger = logger;
        _instructorInfoRepository = instructorInfoRepository;
    }

    public async Task<ResultModel<List<KeyValuePair<Guid, string>>>> Handle(GetInstructorDropdownListQuery request, CancellationToken cancellationToken)
    {
        ErrorModel error;

        try
        {
            var instructors = await _instructorInfoRepository.GetList(true, cancellationToken: cancellationToken);

            List<KeyValuePair<Guid, string>> retVal = new();

            if(instructors is not null && instructors.Any())
            {
                retVal.AddRange(instructors.Select(i =>
                {
                    return new KeyValuePair<Guid, string>(i.Id, i.User.FirstName == Constants.NotApplicable ? Constants.NotApplicable : $"Prof. {i.User.FirstName} {i.User.LastName}");
                }));
            }

            return ResultModel<List<KeyValuePair<Guid, string>>>.Success(retVal);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving Instructor List");

            error = new(nameof(Exception), "Error retrieving Instructor List");

            return ResultModel<List<KeyValuePair<Guid, string>>>.Fail(error);
        }
    }
}

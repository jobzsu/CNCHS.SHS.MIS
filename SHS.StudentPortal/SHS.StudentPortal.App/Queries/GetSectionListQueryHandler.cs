using Microsoft.Extensions.Logging;
using SHS.StudentPortal.App.Abstractions.Messaging;
using SHS.StudentPortal.App.Abstractions.Repositories;
using SHS.StudentPortal.Common.Models;

namespace SHS.StudentPortal.App.Queries;

internal sealed class GetSectionListQueryHandler : 
    IQueryHandler<GetSectionListQuery, List<KeyValuePair<Guid, string>>>
{
    private readonly ILogger<GetSectionListQueryHandler> _logger;
    private readonly ISectionRepository _sectionRepository;

    public GetSectionListQueryHandler(ILogger<GetSectionListQueryHandler> logger, 
        ISectionRepository sectionRepository)
    {
        _logger = logger;
        _sectionRepository = sectionRepository;
    }

    public async Task<ResultModel<List<KeyValuePair<Guid, string>>>> Handle(GetSectionListQuery request, 
        CancellationToken cancellationToken)
    {
        ErrorModel error;

        try
        {
            var sectionList = new List<KeyValuePair<Guid, string>>();

            var sections = await _sectionRepository.GetAllSections(includeNotApplicable: request.includeNotApplicable, cancellationToken: cancellationToken);

            if(sections is not null || sections.Any())
            {
                var sortedSections = sections.OrderBy(s => s.Name).ToList();

                sectionList.AddRange(sortedSections.Select(x => new KeyValuePair<Guid, string>(x.Id, x.Name)));
            }

            return ResultModel<List<KeyValuePair<Guid, string>>>.Success(sectionList);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            error = new(nameof(ex), "Error while getting section list.");

            return ResultModel<List<KeyValuePair<Guid, string>>>.Fail(error);
        }
    }
}

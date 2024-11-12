using Microsoft.Extensions.Logging;
using SHS.StudentPortal.App.Abstractions.Messaging;
using SHS.StudentPortal.App.Abstractions.Repositories;
using SHS.StudentPortal.Common.Models;

namespace SHS.StudentPortal.App.Queries;

internal sealed class GetSectionListQueryHandler : 
    IQueryHandler<GetSectionListQuery, List<KeyValuePair<string, string>>>
{
    private readonly ILogger<GetSectionListQueryHandler> _logger;
    private readonly ISectionRepository _sectionRepository;

    public GetSectionListQueryHandler(ILogger<GetSectionListQueryHandler> logger, 
        ISectionRepository sectionRepository)
    {
        _logger = logger;
        _sectionRepository = sectionRepository;
    }

    public async Task<ResultModel<List<KeyValuePair<string, string>>>> Handle(GetSectionListQuery request, 
        CancellationToken cancellationToken)
    {
        ErrorModel error;

        try
        {
            var sectionList = new List<KeyValuePair<string, string>>();

            var sections = await _sectionRepository.GetAllSections(includeNotApplicable: true, cancellationToken: cancellationToken);

            if(sections is null || sections.Count == 0)
            {
                sectionList.Add(new KeyValuePair<string, string>(string.Empty, "N/A"));
            }
            else
            {
                var itemToMove = sections.FirstOrDefault(x => x.Name.ToLower() == "n/a");

                var sortedSections = sections.OrderByDescending(x => x.Id).ToList();

                // remove from list
                sortedSections.Remove(itemToMove);
                // add to list
                sortedSections.Add(itemToMove);

                sectionList.AddRange(sortedSections.Select(x => new KeyValuePair<string, string>(x.Id.ToString(), x.Name)));
            }

            return ResultModel<List<KeyValuePair<string, string>>>.Success(sectionList);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            error = new(nameof(ex), "Error while getting section list.");

            return ResultModel<List<KeyValuePair<string, string>>>.Fail(error);
        }
    }
}

using Microsoft.Extensions.Logging;
using SHS.StudentPortal.App.Abstractions.Messaging;
using SHS.StudentPortal.App.Abstractions.Repositories;
using SHS.StudentPortal.Common.Models;
using SHS.StudentPortal.Common.Utilities.Models;

namespace SHS.StudentPortal.App.Queries;

internal sealed class GetPaginatedSubjectListQueryHandler
    : IQueryHandler<GetPaginatedSubjectListQuery, PaginatedList<SubjectListViewModel>>
{
    private readonly ILogger<GetPaginatedSubjectListQueryHandler> _logger;
    private readonly ISubjectRepository _subjectRepository;

    public GetPaginatedSubjectListQueryHandler(ILogger<GetPaginatedSubjectListQueryHandler> logger, 
        ISubjectRepository subjectRepository)
    {
        _logger = logger;
        _subjectRepository = subjectRepository;
    }

    public async Task<ResultModel<PaginatedList<SubjectListViewModel>>> Handle(GetPaginatedSubjectListQuery request, CancellationToken cancellationToken)
    {
        ErrorModel error = null;

        try
        {
            var track = Track.GetTrack(request.filter.Track ?? string.Empty).Id;
            var strand = Strand.GetStrand(request.filter.Strand ?? string.Empty).Id;

            var subjectList = await _subjectRepository
                .GetListViaFilter(request.filter.SearchKeyword, track, strand, cancellationToken);

            if(subjectList is null || subjectList.Count() == 0)
            {
                var paginatedList = PaginatedList<SubjectListViewModel>
                    .Create(new List<SubjectListViewModel>().AsQueryable(), 1, 10);

                return ResultModel<PaginatedList<SubjectListViewModel>>.Success(paginatedList);
            }
            else
            {
                var subjectListViewModel = new List<SubjectListViewModel>();

                subjectListViewModel.AddRange(subjectList.Select(x => {

                    var strandStr = "";

                    if (Strand.GetStrand(x.TrackAndStrand.Split('-')[1]).IsPlaceholder)
                        strandStr = "N/A";
                    else
                        strandStr = Strand.GetStrand(x.TrackAndStrand.Split('-')[1]).Id.ToUpper();

                    return new SubjectListViewModel
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Code = x.Code,
                        Units = x.Units.ToString(),
                        TrackAndStrand = $"{x.TrackAndStrand.Split('-')[0].ToUpper()} - {strandStr}"
                    };
                }));

                var paginatedList = PaginatedList<SubjectListViewModel>
                    .Create(subjectListViewModel.AsQueryable(), request.filter.PageNumber, 10);

                return ResultModel<PaginatedList<SubjectListViewModel>>.Success(paginatedList);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting Subject List");

            if(error is null)
                error = new(nameof(Exception), "Error getting Subject List");

            return ResultModel<PaginatedList<SubjectListViewModel>>.Fail(error);
        }
    }
}

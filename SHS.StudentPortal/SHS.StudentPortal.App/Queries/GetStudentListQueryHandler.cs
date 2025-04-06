using Microsoft.Extensions.Logging;
using SHS.StudentPortal.App.Abstractions.Messaging;
using SHS.StudentPortal.App.Abstractions.Repositories;
using SHS.StudentPortal.Common.Models;

namespace SHS.StudentPortal.App.Queries;

internal sealed class GetStudentListQueryHandler : IQueryHandler<GetStudentListQuery, IEnumerable<StudentListViewModel>>
{
    private readonly ILogger<GetStudentListQueryHandler> _logger;
    private readonly IStudentInfoRepository _studentInfoRepository;

    public GetStudentListQueryHandler(ILogger<GetStudentListQueryHandler> logger, 
        IStudentInfoRepository studentInfoRepository)
    {
        _logger = logger;
        _studentInfoRepository = studentInfoRepository;
    }

    public async Task<ResultModel<IEnumerable<StudentListViewModel>>> Handle(GetStudentListQuery request, 
        CancellationToken cancellationToken)
    {
        ErrorModel error;

        try
        {
            var retVal = new List<StudentListViewModel>();

            var sectionId = string.IsNullOrWhiteSpace(request.filter.SectionId) ? Guid.Empty : Guid.Parse(request.filter.SectionId);

            var track = string.IsNullOrWhiteSpace(request.filter.Track) ? string.Empty : request.filter.Track.ToLower().Trim();

            var strand = string.IsNullOrWhiteSpace(request.filter.Strand) ? string.Empty : request.filter.Strand.ToLower().Trim();

            var students = await _studentInfoRepository.GetListViaFilter(request.filter.SearchKeyword,
                request.filter.YearLevel,
                sectionId,
                track,
                strand,
                cancellationToken);

            if(students is not null && students.Any())
            {
                retVal.AddRange(students.Select(s =>
                {
                    var trackStrandSplitStr = s.TrackAndStrand.Split('-');

                    var trackId = trackStrandSplitStr![0];

                    var trackStr = string.Empty;
                    if (trackId == Track.AcademicTrack.Id)
                        trackStr = "Academic";
                    else if (trackId == Track.ArtsAndDesignTrack.Id)
                        trackStr = "Arts & Design";
                    else if (trackId == Track.SportsTrack.Id)
                        trackStr = "Sports";
                    else
                        trackStr = "TVL";

                    var strandStr = Strand.GetStrand(trackStrandSplitStr![1]).IsPlaceholder ?
                        Strand.Placeholder.Name :
                        Strand.GetStrand(trackStrandSplitStr![1]).Name;

                    return new StudentListViewModel()
                    {
                        Id = s.Id.ToString(),
                        StudentId = s.IdNumber.ToString().PadLeft(7, '0'),
                        Name = s.User.MiddleName is null ? $"{s.User.FirstName} {s.User.LastName}" : $"{s.User.FirstName} {s.User.MiddleName} {s.User.LastName}",
                        Gender = s.Gender.ToUpper(),
                        YearLevel = $"Grade {s.YearLevel}",
                        TrackAndStrand = $"{trackStr} - {strandStr}",
                        Status = StudentStatuses.Get(s.StudentStatus).Name.ToUpper()
                    };
                }));
            }

            return ResultModel<IEnumerable<StudentListViewModel>>.Success(retVal.AsEnumerable());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving student list");

            error = new("ErrorRetrievingStudentListException", "Error retrieving Student List");

            return ResultModel<IEnumerable<StudentListViewModel>>.Fail(error);
        }
    }
}

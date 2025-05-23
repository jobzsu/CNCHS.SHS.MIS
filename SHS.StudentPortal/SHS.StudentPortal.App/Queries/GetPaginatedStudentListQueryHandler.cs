﻿using Microsoft.Extensions.Logging;
using SHS.StudentPortal.App.Abstractions.Messaging;
using SHS.StudentPortal.App.Abstractions.Repositories;
using SHS.StudentPortal.Common.Models;
using SHS.StudentPortal.Common.Utilities.Models;
using System.Linq;

namespace SHS.StudentPortal.App.Queries;

internal sealed class GetPaginatedStudentListQueryHandler :
    IQueryHandler<GetPaginatedStudentListQuery, PaginatedList<StudentListViewModel>>
{
	private readonly ILogger<GetPaginatedStudentListQueryHandler> _logger;
    private readonly IStudentInfoRepository _studentInfoRepository;

    public GetPaginatedStudentListQueryHandler(ILogger<GetPaginatedStudentListQueryHandler> logger, IStudentInfoRepository studentInfoRepository)
    {
        _logger = logger;
        _studentInfoRepository = studentInfoRepository;
    }

    public async Task<ResultModel<PaginatedList<StudentListViewModel>>> Handle(GetPaginatedStudentListQuery request, CancellationToken cancellationToken)
    {
        ErrorModel error;

		try
		{
            var sectionId = string.IsNullOrWhiteSpace(request.filter.SectionId) ? Guid.Empty : Guid.Parse(request.filter.SectionId);
            var track = Track.GetTrack(request.filter.Track ?? string.Empty).Id;
            var strand = Strand.GetStrand(request.filter.Strand ?? string.Empty).Id;

            var studentList = await _studentInfoRepository.GetListViaFilter(request.filter.SearchKeyword,
                request.filter.YearLevel,
                sectionId,
                track,
                strand,
                cancellationToken);

            if(studentList is null || studentList.Count() == 0)
            {
                var paginatedList = PaginatedList<StudentListViewModel>
                    .Create(new List<StudentListViewModel>().AsQueryable(), 1, 999999);

                return ResultModel<PaginatedList<StudentListViewModel>>.Success(paginatedList);
            }
            else
            {
                var studentListViewModel = new List<StudentListViewModel>();

                studentListViewModel.AddRange(studentList.Select(x =>
                {
                    var trackStrandSplitStr = x.TrackAndStrand.Split('-');

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
                        Id = x.Id.ToString(),
                        StudentId = x.IdNumber.ToString().PadLeft(7, '0'),
                        Name = x.User.MiddleName is null ? $"{x.User.FirstName} {x.User.LastName}" : $"{x.User.FirstName} {x.User.MiddleName} {x.User.LastName}",
                        Gender = x.Gender.ToUpper(),
                        YearLevel = $"Grade {x.YearLevel}",
                        TrackAndStrand = $"{trackStr} - {strandStr}",
                        Status = StudentStatuses.Get(x.StudentStatus).Name.ToUpper()
                    };
                }));

                var paginatedList = PaginatedList<StudentListViewModel>
                    .Create(studentListViewModel.AsQueryable(), request.filter.PageNumber, 999999);

                return ResultModel<PaginatedList<StudentListViewModel>>.Success(paginatedList);
            }
		}
		catch (Exception ex)
		{
            _logger.LogError($"Error Getting Student List: {ex}");

            error = new(nameof(ex), "Error querying student list");

            return await Task.FromResult(ResultModel<PaginatedList<StudentListViewModel>>.Fail(error));
		}

        throw new NotImplementedException();
    }
}

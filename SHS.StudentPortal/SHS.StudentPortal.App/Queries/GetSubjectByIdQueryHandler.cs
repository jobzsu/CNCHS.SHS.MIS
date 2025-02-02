using Microsoft.Extensions.Logging;
using SHS.StudentPortal.App.Abstractions.Messaging;
using SHS.StudentPortal.App.Abstractions.Repositories;
using SHS.StudentPortal.Common.Models;

namespace SHS.StudentPortal.App.Queries;

internal sealed class GetSubjectByIdQueryHandler
    : IQueryHandler<GetSubjectByIdQuery, SubjectViewModel>
{
    private readonly ILogger<GetSubjectByIdQueryHandler> _logger;
    private readonly ISubjectRepository _subjectRepository;

    public GetSubjectByIdQueryHandler(ILogger<GetSubjectByIdQueryHandler> logger, 
        ISubjectRepository subjectRepository)
    {
        _logger = logger;
        _subjectRepository = subjectRepository;
    }

    public async Task<ResultModel<SubjectViewModel>> Handle(GetSubjectByIdQuery request, CancellationToken cancellationToken)
    {
        ErrorModel error;

        try
        {
            var subject = await _subjectRepository.GetSubjectById(request.subjectId, false, cancellationToken);

            if(subject is null)
            {
                error = new(nameof(KeyNotFoundException), "Subject not found");

                return ResultModel<SubjectViewModel>.Fail(error);
            }

            var trackId = subject.TrackAndStrand.Split('-')[0];
            var strandId = subject.TrackAndStrand.Split('-')[1];

            var subjectViewModel = new SubjectViewModel()
            {
                Id = subject.Id,
                Code = subject.Code,
                Name = subject.Name,
                Hours = subject.Hours,
                Days = subject.Days,
                Description = subject.Description ?? string.Empty,
                Units = subject.Units,
                TrackId = trackId,
                StrandId = strandId,
                Semester = subject.Semester,
                AcademicYear = subject.AcademicYear
            };

            return ResultModel<SubjectViewModel>.Success(subjectViewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error while getting subject by id: {ex}", ex);

            error = new(nameof(Exception), "Error while getting Subject");

            return ResultModel<SubjectViewModel>.Fail(error);
        }
    }
}

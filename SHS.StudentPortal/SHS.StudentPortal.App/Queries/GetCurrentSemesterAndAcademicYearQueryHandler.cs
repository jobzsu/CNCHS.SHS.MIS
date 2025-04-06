using Microsoft.Extensions.Logging;
using SHS.StudentPortal.App.Abstractions.Messaging;
using SHS.StudentPortal.App.Abstractions.Repositories;
using SHS.StudentPortal.Common;
using SHS.StudentPortal.Common.Models;

namespace SHS.StudentPortal.App.Queries;

internal sealed class GetCurrentSemesterAndAcademicYearQueryHandler
    : IQueryHandler<GetCurrentSemesterAndAcademicYearQuery, Tuple<string, string>>
{
    private readonly ILogger<GetCurrentSemesterAndAcademicYearQueryHandler> _logger;
    private readonly ISettingRepository _settingRepository;

    public GetCurrentSemesterAndAcademicYearQueryHandler(ILogger<GetCurrentSemesterAndAcademicYearQueryHandler> logger, 
        ISettingRepository settingRepository)
    {
        _logger = logger;
        _settingRepository = settingRepository;
    }

    public async Task<ResultModel<Tuple<string, string>>> Handle(GetCurrentSemesterAndAcademicYearQuery request, CancellationToken cancellationToken)
    {
        ErrorModel error;

        try
        {
            var currentSemester = await _settingRepository.GetSettingByName(Constants.CurrentSemester, cancellationToken: cancellationToken);
            var currentAcademicYear = await _settingRepository.GetSettingByName(Constants.CurrentAcademicYear, cancellationToken: cancellationToken);

            if(currentSemester is null ||  currentAcademicYear is null)
            {
                error = new(nameof(KeyNotFoundException), "Current semester or academic year not found.");

                return ResultModel<Tuple<string, string>>.Fail(error);
            }

            var retVal = new Tuple<string, string>(currentSemester.Value, currentAcademicYear.Value);

            return ResultModel<Tuple<string, string>>.Success(retVal);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving Current Semester and Academic Year");

            error = new(nameof(Exception), "An error occurred while retrieving the current semester and academic year.");

            return ResultModel<Tuple<string, string>>.Fail(error);
        }
    }
}

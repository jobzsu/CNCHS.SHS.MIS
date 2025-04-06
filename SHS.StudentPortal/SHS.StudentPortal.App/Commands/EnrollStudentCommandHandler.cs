using Microsoft.Extensions.Logging;
using SHS.StudentPortal.App.Abstractions.Messaging;
using SHS.StudentPortal.App.Abstractions.Persistence;
using SHS.StudentPortal.App.Abstractions.Repositories;
using SHS.StudentPortal.Common;
using SHS.StudentPortal.Common.Models;
using SHS.StudentPortal.Domain.Models;

namespace SHS.StudentPortal.App.Commands;

internal sealed class EnrollStudentCommandHandler
    : ICommandHandler<EnrollStudentCommand>
{
    private readonly ILogger<EnrollStudentCommandHandler> _logger;
    private readonly IStudentInfoRepository _studentInfoRepository;
    private readonly ISectionRepository _sectionRepository;
    private readonly IScheduleRepository _scheduleRepository;
    private readonly IStudentScheduleRepository _studentScheduleRepository;
    private readonly IBaseUnitOfWork _baseUnitOfWork;
    private readonly ISettingRepository _settingRepository;

    public EnrollStudentCommandHandler(ILogger<EnrollStudentCommandHandler> logger,
        IStudentInfoRepository studentInfoRepository,
        ISectionRepository sectionRepository,
        IScheduleRepository scheduleRepository,
        IStudentScheduleRepository studentScheduleRepository,
        IBaseUnitOfWork baseUnitOfWork,
        ISettingRepository settingRepository)
    {
        _logger = logger;
        _studentInfoRepository = studentInfoRepository;
        _sectionRepository = sectionRepository;
        _scheduleRepository = scheduleRepository;
        _studentScheduleRepository = studentScheduleRepository;
        _baseUnitOfWork = baseUnitOfWork;
        _settingRepository = settingRepository;
    }

    public async Task<ResultModel> Handle(EnrollStudentCommand request, CancellationToken cancellationToken)
    {
        ErrorModel error;

        try
        {
            var studentInfo = await _studentInfoRepository.GetById(request.view.StudentId, true, cancellationToken);
            var currentSemester = await _settingRepository.GetSettingByName(Constants.CurrentSemester, cancellationToken: cancellationToken);
            var currentAcademicYear = await _settingRepository.GetSettingByName(Constants.CurrentAcademicYear, cancellationToken: cancellationToken);

            if (studentInfo is null)
            {
                error = new(nameof(KeyNotFoundException), "Student not found.");

                return ResultModel.Fail(error);
            }

            if(currentSemester is null || currentAcademicYear is null)
            {
                error = new(nameof(KeyNotFoundException), "Current semester or academic year not found.");

                return ResultModel.Fail(error);
            }

            var sectionId = request.view.DesignatedSectionId;

            var section = await _sectionRepository.GetById(sectionId, cancellationToken: cancellationToken);

            if (section is null)
            {
                error = new(nameof(KeyNotFoundException), "Selected Section not found.");

                return ResultModel.Fail(error);
            }

            var schedules = await _scheduleRepository.GetSelectedSchedules(request.view.SelectedSchedules, cancellationToken: cancellationToken);

            if(schedules is null || !schedules.Any())
            {
                error = new(nameof(KeyNotFoundException), "Selected Schedules were not found.");

                return ResultModel.Fail(error);
            }

            if(schedules.Any() && schedules.Count() != request.view.SelectedSchedules.Count())
            {
                error = new(nameof(KeyNotFoundException), "Some Schedules were not found.");

                return ResultModel.Fail(error);
            }

            studentInfo.YearLevel = request.view.DesignatedGradeLevel;
            studentInfo.SectionId = request.view.DesignatedSectionId;
            studentInfo.StudentStatus = StudentStatuses.Get(request.view.DesignatedStatus).Id;

            await _studentInfoRepository.Update(studentInfo, cancellationToken);

            foreach(var schedule in schedules)
            {
                var newSched = new StudentSchedule()
                    .Create(request.view.StudentId,
                        schedule.Id,
                        currentSemester!.Value,
                        currentAcademicYear!.Value,
                        request.enrolledById);

                await _studentScheduleRepository.CreateStudentSchedule(newSched, cancellationToken);
            }

            await _baseUnitOfWork.SaveChangesAsync(cancellationToken);

            return ResultModel.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error enrolling student.");

            error = new(nameof(ex), "Error enrolling student");

            return ResultModel.Fail(error);
        }
    }
}

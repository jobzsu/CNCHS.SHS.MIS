using Microsoft.Extensions.Logging;
using SHS.StudentPortal.App.Abstractions.Messaging;
using SHS.StudentPortal.App.Abstractions.Repositories;
using SHS.StudentPortal.Common;
using SHS.StudentPortal.Common.Models;
using SHS.StudentPortal.Common.Models.Student;

namespace SHS.StudentPortal.App.Queries;

internal sealed class GetStudentInfoAdminViewQueryHandler
    : IQueryHandler<GetStudentInfoAdminViewQuery, StudentInfoAdminViewDTO>
{
    private readonly ILogger<GetStudentInfoAdminViewQueryHandler> _logger;
    private readonly IStudentInfoRepository _studentInfoRepository;
    private readonly IAcademicRecordRepository _academicRecordRepository;
    private readonly IUserRepository _userRepository;
    private readonly ISettingRepository _settingRepository;
    private readonly IStudentScheduleRepository _studentScheduleRepository;
    private readonly ISubjectRepository _subjectRepository;

    public GetStudentInfoAdminViewQueryHandler(ILogger<GetStudentInfoAdminViewQueryHandler> logger,
        IStudentInfoRepository studentInfoRepository,
        IAcademicRecordRepository academicRecordRepository,
        IUserRepository userRepository,
        ISettingRepository settingRepository,
        IStudentScheduleRepository studentScheduleRepository,
        ISubjectRepository subjectRepository)
    {
        _logger = logger;
        _studentInfoRepository = studentInfoRepository;
        _academicRecordRepository = academicRecordRepository;
        _userRepository = userRepository;
        _settingRepository = settingRepository;
        _studentScheduleRepository = studentScheduleRepository;
        _subjectRepository = subjectRepository;
    }

    public async Task<ResultModel<StudentInfoAdminViewDTO>> Handle(GetStudentInfoAdminViewQuery request, CancellationToken cancellationToken)
    {
        ErrorModel error;

        try
        {
            var studentInfo = await _studentInfoRepository.GetById(request.studentId, cancellationToken: cancellationToken);

            if (studentInfo is null)
            {
                error = new("StudentInfoNotFound", "Student Info not found");

                return ResultModel<StudentInfoAdminViewDTO>.Fail(error);
            }

            var currentSemester = await _settingRepository.GetSettingByName(Constants.CurrentSemester, cancellationToken: cancellationToken);
            var currentAcademicYear = await _settingRepository.GetSettingByName(Constants.CurrentAcademicYear, cancellationToken: cancellationToken);

            if (currentSemester is null || currentAcademicYear is null)
            {
                error = new(nameof(KeyNotFoundException), "Current semester or academic year not found.");

                return ResultModel<StudentInfoAdminViewDTO>.Fail(error);
            }

            StudentInfoAdminViewDTO retVal = new()
            {
                Id = studentInfo.Id,
                StudentIdNum = studentInfo.IdNumber.ToString().PadLeft(7, '0'),
                Status = studentInfo.StudentStatus,
                Track = studentInfo.TrackAndStrand.Split('-')[0],
                Strand = studentInfo.TrackAndStrand.Split('-')[1],
                YearLevel = studentInfo.YearLevel,
                SectionId = studentInfo.SectionId,
                Username = studentInfo.User.UserAccount.Username,
                LastLogin = studentInfo.User.UserAccount.LastLogin,
                FirstName = studentInfo.User.FirstName,
                MiddleName = studentInfo.User.MiddleName,
                LastName = studentInfo.User.LastName,
                Gender = studentInfo.Gender,
                DateOfBirth = studentInfo.DateOfBirth,
                PlaceOfBirth = studentInfo.PlaceOfBirth,
                CivilStatus = studentInfo.CivilStatus,
                Nationality = studentInfo.Nationality,
                Religion = studentInfo.Religion,
                ContactInfo = studentInfo.ContactInformation,
                Address = studentInfo.Address,
                AcademicRecords = new(),
                CurrentSchedules = new()
                {
                    Semester = currentSemester.Value,
                    AcademicYear = currentAcademicYear.Value,
                    Schedules = new()
                }
            };

            var academicRecords = await _academicRecordRepository.GetAcademicRecordsByStudentId(studentInfo.Id, cancellationToken);

            if (academicRecords != null && academicRecords.Any())
            {
                var subjects = await _subjectRepository.GetAllSubjects(cancellationToken: cancellationToken);
                var otherSubject = await _subjectRepository.GetSubjectById(0, false, cancellationToken);

                if (subjects is not null && subjects.Any())
                {
                    if (otherSubject is not null)
                        subjects.Add(otherSubject!);
                }

                var users = await _userRepository.GetAllList(cancellationToken);

                retVal.AcademicRecords.AddRange(academicRecords
                    .OrderBy(ar => ar.AcademicYear)
                    .ThenBy(ar => ar.Semester)
                    .Select(a =>
                    {
                        var subjectNameToDisplay = a.SubjectId == 0 ?
                            $"EXT - {a.OtherSubjectName}" :
                            ($"({subjects!.FirstOrDefault(s => s.Id == a.SubjectId)!.Code}) {subjects!.FirstOrDefault(s => s.Id == a.SubjectId)!.Name}");

                        var encodedBy = users?.FirstOrDefault(u => u.UserAccountId == a.EncodedById!.Value);
                        var verifiedBy = a.VerifiedById is null ?
                            null : users?.FirstOrDefault(u => u.UserAccountId == a.VerifiedById!.Value);

                        return new AcademicRecordAdminViewDTO()
                        {
                            Id = a.Id,
                            Rating = a.Rating,
                            Semester = a.Semester,
                            SubjectId = a.SubjectId,
                            SubjectName = subjectNameToDisplay,
                            AcademicYear = a.AcademicYear,
                            EncodedBy = $"Prof. {encodedBy?.FirstName} {encodedBy?.LastName}" ?? "N/A",
                            EncodedById = encodedBy!.Id,
                            EncodedDate = a.EncodedDate,
                            VerifiedBy = verifiedBy is not null ?
                                $"Prof. {verifiedBy?.FirstName} {verifiedBy?.LastName}" : "N/A",
                            VerifiedById = verifiedBy?.Id ?? null,
                            VerifiedDate = a.VerifiedDate ?? null,
                            TempId = 0,
                        };
                    }));
            }

            if (studentInfo.StudentStatus == StudentStatuses.Regular.Id || studentInfo.StudentStatus == StudentStatuses.Irregular.Id)
            {
                var schedules = await _studentScheduleRepository.GetByStudentIdCurrentSemesterAcademicYear(request.studentId,
                        currentSemester.Value,
                        currentAcademicYear.Value,
                        cancellationToken: cancellationToken);

                if (schedules != null && schedules.Any())
                {
                    retVal.CurrentSchedules.Schedules.AddRange(schedules.Select(x =>
                    {
                        var trackStrandSplitStr = x.Schedule.Subject.TrackAndStrand.Split('-');

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

                        var timeStartStr = x.Schedule.TimeStart.Hour < 12 ?
                            $"{x.Schedule.TimeStart.Hour}:{x.Schedule.TimeStart.Minute} AM" :
                            (x.Schedule.TimeStart.Hour == 12 ? $"{x.Schedule.TimeStart.Hour}:{x.Schedule.TimeStart.Minute} PM" : $"{x.Schedule.TimeStart.Hour - 12}:{x.Schedule.TimeStart.Minute} PM");

                        var timeEndStr = x.Schedule.TimeEnd.Hour < 12 ?
                            $"{x.Schedule.TimeEnd.Hour}:{x.Schedule.TimeEnd.Minute} AM" :
                            (x.Schedule.TimeEnd.Hour == 12 ? $"{x.Schedule.TimeEnd.Hour}:{x.Schedule.TimeEnd.Minute} PM" : $"{x.Schedule.TimeEnd.Hour - 12}:{x.Schedule.TimeEnd.Minute} PM");

                        var daysArr = x.Schedule.Day.Split(',').ToList();
                        var days = new List<string>();

                        foreach (var d in daysArr)
                        {
                            days.Add(SchoolDays.Get(d).Name);
                        }

                        return new ScheduleListViewModel()
                        {
                            Id = x.ScheduleId,
                            Days = string.Join(',', days),
                            Subject = x.Schedule.Subject.Name,
                            TrackAndStrand = $"{trackStr} - {strandStr}",
                            Instructor = x.Schedule.Instructor.User.LastName == null ?
                                    $"Prof. {x.Schedule.Instructor.User.FirstName} {x.Schedule.Instructor.User.LastName}" :
                                    $"Prof. {x.Schedule.Instructor.User.FirstName} {x.Schedule.Instructor.User.MiddleName} {x.Schedule.Instructor.User.LastName}",
                            Time = $"{timeStartStr} - {timeEndStr}",
                            RoomNumber = x.Schedule.RoomNumber
                        };
                    }));
                }
            }

            return ResultModel<StudentInfoAdminViewDTO>.Success(retVal);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving Student Info Admin View");

            error = new("ErrorRetrievingException", "Error retrieving Student Info Admin View");

            return ResultModel<StudentInfoAdminViewDTO>.Fail(error);
        }
    }
}

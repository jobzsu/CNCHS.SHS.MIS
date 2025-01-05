using Microsoft.Extensions.Logging;
using SHS.StudentPortal.App.Abstractions.Messaging;
using SHS.StudentPortal.App.Abstractions.Repositories;
using SHS.StudentPortal.Common;
using SHS.StudentPortal.Common.Models;
using SHS.StudentPortal.Domain.Models;

namespace SHS.StudentPortal.App.Queries;

internal sealed class GetStudentForAdminApprovalQueryHandler :
    IQueryHandler<GetStudentForAdminApprovalQuery, StudentInfoForAdminViewingViewModel>
{
    private readonly ILogger<GetStudentForAdminApprovalQueryHandler> _logger;
    private readonly IStudentInfoRepository _studentInfoRepository;
    private readonly ICourseRepository _courseRepository;
    private readonly ISectionRepository _sectionRepository;
    private readonly IExternalAcademicRecordRepository _externalAcademicRecordRepository;
    private readonly ISettingRepository _settingRepository;
    private readonly IStudentScheduleRepository _studentScheduleRepository;
    private readonly IAcademicRecordRepository _academicRecordRepository;
    private readonly IUserRepository _userRepository;
    private readonly ISubjectRepository _subjectRepository;

    public GetStudentForAdminApprovalQueryHandler(ILogger<GetStudentForAdminApprovalQueryHandler> logger,
        IStudentInfoRepository studentInfoRepository,
        ICourseRepository courseRepository,
        ISectionRepository sectionRepository,
        IExternalAcademicRecordRepository externalAcademicRecordRepository,
        ISettingRepository settingRepository,
        IStudentScheduleRepository studentScheduleRepository,
        IAcademicRecordRepository academicRecordRepository,
        IUserRepository userRepository,
        ISubjectRepository subjectRepository)
    {
        _logger = logger;
        _studentInfoRepository = studentInfoRepository;
        _courseRepository = courseRepository;
        _sectionRepository = sectionRepository;
        _externalAcademicRecordRepository = externalAcademicRecordRepository;
        _settingRepository = settingRepository;
        _studentScheduleRepository = studentScheduleRepository;
        _academicRecordRepository = academicRecordRepository;
        _userRepository = userRepository;
        _subjectRepository = subjectRepository;
    }

    public async Task<ResultModel<StudentInfoForAdminViewingViewModel>> Handle(GetStudentForAdminApprovalQuery request, CancellationToken cancellationToken)
    {
        ErrorModel error;

        try
        {
            var studentInfo = await _studentInfoRepository.GetById(request.studentId,  cancellationToken: cancellationToken);

            if (studentInfo is null)
            {
                error = new(nameof(KeyNotFoundException), "Student was not found.");

                return ResultModel<StudentInfoForAdminViewingViewModel>.Fail(error);
            }
            else
            {
                var sections = await _sectionRepository.GetAllSections(cancellationToken: cancellationToken);

                if (sections is null || sections.Count == 0)
                {
                    error = new(nameof(NullReferenceException), "No Section available.");

                    return ResultModel<StudentInfoForAdminViewingViewModel>.Fail(error);
                }

                var kvpSectionList = new List<KeyValuePair<Guid, string>>();
                kvpSectionList.AddRange(sections.Select(x => new KeyValuePair<Guid, string>(x.Id, x.Name)));

                var academicRecords = await _academicRecordRepository.GetAcademicRecordsByStudentId(request.studentId, cancellationToken);

                StudentInfoForAdminViewingViewModel viewModel = new()
                {
                    Id = studentInfo.Id,
                    StudentIdNum = studentInfo.IdNumber.ToString().PadLeft(7, '0'),
                    Status = studentInfo.StudentStatus.ToUpper(),
                    Track = studentInfo.TrackAndStrand.Split('-')[0].ToUpper(),
                    Strand = studentInfo.TrackAndStrand.Split('-')[1].ToUpper(),
                    YearLevel = studentInfo.YearLevel,
                    SectionId = studentInfo.SectionId,
                    Username = studentInfo.User.UserAccount.Username.ToLower(),
                    LastLogin = studentInfo.User.UserAccount.LastLogin?.ToLocalTime().ToString() ?? "Never Logged In",
                    SectionList = kvpSectionList,
                    FirstName = studentInfo.User.FirstName,
                    MiddleName = studentInfo.User.MiddleName,
                    LastName = studentInfo.User.LastName,
                    Gender = studentInfo.Gender,
                    DateOfBirth = studentInfo.DateOfBirth.ToString("yyyy-MM-dd"),
                    PlaceOfBirth = studentInfo.PlaceOfBirth,
                    CivilStatus = studentInfo.CivilStatus,
                    Nationality = studentInfo.Nationality,
                    Religion = studentInfo.Religion,
                    ContactInfo = studentInfo.ContactInformation,
                    Address = studentInfo.Address,
                    AcademicRecords = new(),
                    CurrentSchedules = null
                };

                if(academicRecords is not null && academicRecords.Any())
                {
                    foreach(var record in academicRecords)
                    {
                        var academicRecordToDisplay = new AcademicRecordViewModel()
                        {
                            Rating = record.Rating,
                            Semester = record.Semester,
                            AcademicYear = record.AcademicYear,
                            EncodedDate = record.EncodedDate is null ? "N/A" : record.EncodedDate.Value.ToString("MM/dd/yyyy"),
                            VerifiedDate = record.VerifiedDate is null ? "N/A" : record.VerifiedDate.Value.ToString("MM/dd/yyyy"),
                            TempId = 0
                        };

                        if(record.SubjectId > 0)
                        {
                            var subject = await _subjectRepository.GetSubjectById(record.SubjectId, cancellationToken: cancellationToken);

                            academicRecordToDisplay.SubjectName = subject is null ? record.OtherSubjectName : subject.Name;
                        }

                        if (record.EncodedById is not null)
                        {
                            var encodedByProf = await _userRepository.GetUserByUserAccountId(record.EncodedById.Value, cancellationToken);

                            academicRecordToDisplay.EncodedBy = encodedByProf is null ?
                                "N/A" :
                                $"Prof. {encodedByProf.LastName}, {encodedByProf.FirstName.First().ToString().ToUpper()}.";
                        }

                        if (record.VerifiedById is not null)
                        {
                            var verifiedByProf = await _userRepository.GetUserByUserAccountId(record.VerifiedById.Value, cancellationToken);

                            academicRecordToDisplay.VerifiedBy = verifiedByProf is null ?
                                "N/A" :
                                $"Prof. {verifiedByProf.LastName}, {verifiedByProf.FirstName.First().ToString().ToUpper()}.";
                        }

                        viewModel.AcademicRecords.Add(academicRecordToDisplay);
                    }
                }

                if(studentInfo.StudentStatus == StudentStatuses.Regular.Id || studentInfo.StudentStatus == StudentStatuses.Irregular.Id)
                {
                    var currentSemester = await _settingRepository.GetSettingByName(Constants.CurrentSemester, cancellationToken: cancellationToken);
                    var currentAcademicYear = await _settingRepository.GetSettingByName(Constants.CurrentAcademicYear, cancellationToken: cancellationToken);

                    if (currentSemester is null || currentAcademicYear is null)
                    {
                        error = new(nameof(KeyNotFoundException), "Current semester or academic year not found.");

                        return ResultModel<StudentInfoForAdminViewingViewModel>.Fail(error);
                    }

                    viewModel.CurrentSchedules = new()
                    {
                        Semester = currentSemester.Value,
                        AcademicYear = currentAcademicYear.Value,
                        Schedules = new()
                    };

                    var schedules = await _studentScheduleRepository.GetByStudentIdCurrentSemesterAcademicYear(request.studentId,
                        currentSemester.Value,
                        currentAcademicYear.Value,
                        cancellationToken: cancellationToken);

                    if(schedules is not null && schedules.Count > 0)
                    {
                        schedules.ForEach(x =>
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

                            viewModel.CurrentSchedules.Schedules.Add(new()
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
                            });
                        });
                    }
                }

                return ResultModel<StudentInfoForAdminViewingViewModel>.Success(viewModel);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving student for admin approval: {ex.Message}");

            error = new(nameof(ex), $"Error retrieving student for approval.");

            return ResultModel<StudentInfoForAdminViewingViewModel>.Fail(error);
        }
    }
}

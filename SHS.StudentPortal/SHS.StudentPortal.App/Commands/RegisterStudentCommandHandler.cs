using MediatR;
using Microsoft.Extensions.Logging;
using SHS.StudentPortal.App.Abstractions.Messaging;
using SHS.StudentPortal.App.Abstractions.Persistence;
using SHS.StudentPortal.App.Abstractions.Repositories;
using SHS.StudentPortal.App.Abstractions.Security;
using SHS.StudentPortal.Common;
using SHS.StudentPortal.Common.Models;
using SHS.StudentPortal.Domain.Models;
using System.Data;

namespace SHS.StudentPortal.App.Commands;

internal sealed class RegisterStudentCommandHandler : ICommandHandler<RegisterStudentCommand>
{
    private readonly ILogger<RegisterStudentCommandHandler> _logger;
    private readonly IUserAccountRepository _userAccountRepository;
    private readonly IBCryptAuthProvider _bCryptAuthProvider;
    private readonly IUserRepository _userRepository;
    private readonly ISectionRepository _sectionRepository;
    private readonly ICourseRepository _courseRepository;
    private readonly IStudentInfoRepository _studentInfoRepository;
    private readonly IExternalAcademicRecordRepository _externalAcademicRecordRepository;
    private readonly IBaseUnitOfWork _baseUnitOfWork;
    private readonly IPublisher _publisher;

    public RegisterStudentCommandHandler(ILogger<RegisterStudentCommandHandler> logger,
        IUserAccountRepository userAccountRepository,
        IBCryptAuthProvider bCryptAuthProvider,
        IUserRepository userRepository,
        ISectionRepository sectionRepository,
        ICourseRepository courseRepository,
        IStudentInfoRepository studentInfoRepository,
        IExternalAcademicRecordRepository externalAcademicRecordRepository,
        IBaseUnitOfWork baseUnitOfWork,
        IPublisher publisher)
    {
        _logger = logger;
        _userAccountRepository = userAccountRepository;
        _bCryptAuthProvider = bCryptAuthProvider;
        _userRepository = userRepository;
        _sectionRepository = sectionRepository;
        _courseRepository = courseRepository;
        _studentInfoRepository = studentInfoRepository;
        _externalAcademicRecordRepository = externalAcademicRecordRepository;
        _baseUnitOfWork = baseUnitOfWork;
        _publisher = publisher;
    }

    public async Task<ResultModel> Handle(RegisterStudentCommand request, CancellationToken cancellationToken = default)
    {
        ErrorModel error;
        
        bool studentUserAccountInserted = false;
        UserAccount studentUserAccount = null;
        bool studentUserInserted = false;
        User studentUser = null;
        bool studentInfoInserted = false;
        StudentInfo studentInfo = null;

        try
        {
            if (request.model is null)
            {
                error = new(nameof(ArgumentNullException), "Student details cannot be null");

                return ResultModel.Fail(error);
            }

            //if(request.model.ExternalAcademicRecords is null || request.model.ExternalAcademicRecords.Count == 0)
            //{
            //    error = new(nameof(ArgumentNullException), "External Academic Records cannot be null or empty");

            //    return ResultModel.Fail(error);
            //}

            var usernameHit = await _userAccountRepository.GetUserAccountByUsername(request.model.Username, cancellationToken);

            if (usernameHit is not null)
            {
                error = new(nameof(ArgumentException), "Username already exists");

                return ResultModel.Fail(error);
            }
            else
            {
                var passwordHash = _bCryptAuthProvider.EncryptPassword(request.model.Password);

                var newStudentUserAccount = new UserAccount()
                    .Create(request.model.Username, 
                        passwordHash, 
                        request.model.EmailAddress, 
                        Constants.SystemGuid);

                var insertedStudentUserAccount = await _userAccountRepository.CreateUserAccount(newStudentUserAccount, cancellationToken);

                await _baseUnitOfWork.SaveChangesAsync(cancellationToken);

                studentUserAccountInserted = true;
                studentUserAccount = insertedStudentUserAccount;

                var newStudentUser = new User()
                    .Create(request.model.FirstName, 
                        request.model.MiddleName, 
                        request.model.LastName, 
                        RoleTypes.Student.Id,
                        insertedStudentUserAccount.Id,
                        Constants.SystemGuid);

                var insertedStudentUser = await _userRepository.CreateUser(newStudentUser, cancellationToken);

                await _baseUnitOfWork.SaveChangesAsync(cancellationToken);

                studentUserInserted = true;
                studentUser = insertedStudentUser;

                var placeHolderSection = await _sectionRepository.GetSectionByName(Constants.NotApplicable, cancellationToken: cancellationToken);
                //var placeHolderCourse = await _courseRepository.GetByName(Constants.NotApplicable, cancellationToken: cancellationToken);

                var dobStrArr = request.model.DateOfBirth.Split('-');

                var newStudentInfo = new StudentInfo()
                    .Create(
                        StudentStatuses.Pending.Id,
                        request.model.YearLevel,
                        placeHolderSection!.Id,
                        $"{Track.GetTrack(request.model.Track).Id}-{Strand.GetStrand(request.model.Strand ?? string.Empty).Id}",
                        insertedStudentUser.Id,
                        request.model.Gender,
                        new DateOnly(int.Parse(dobStrArr[0]), int.Parse(dobStrArr[1]), int.Parse(dobStrArr[2])),
                        request.model.PlaceOfBirth,
                        request.model.CivilStatus,
                        request.model.Nationality,
                        request.model.Religion,
                        request.model.ContactInfo,
                        request.model.Address,
                        Constants.SystemGuid);

                var insertedStudentInfo = await _studentInfoRepository
                    .Create(newStudentInfo, cancellationToken);

                await _baseUnitOfWork.SaveChangesAsync(cancellationToken);

                studentInfoInserted = true;
                studentInfo = insertedStudentInfo;

                //foreach(var externalAcademicRecord in request.model.ExternalAcademicRecords)
                //{
                //    var newExternalAcademicRecord = new ExternalAcademicRecord().Create(
                //        externalAcademicRecord.Rating,
                //        externalAcademicRecord.SubjectName,
                //        externalAcademicRecord.Semester,
                //        externalAcademicRecord.AcademicYear,
                //        insertedStudentInfo.Id,
                //        Constants.SystemGuid);

                //    await _externalAcademicRecordRepository.CreateExternalAcademicRecord(newExternalAcademicRecord, cancellationToken);
                //}

                await _baseUnitOfWork.SaveChangesAsync(cancellationToken);

                //await _publisher.Publish();

                return ResultModel.Success();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error saving student info: {ex.Message}");

            // Rollback inserted accounts
            if(studentUserAccountInserted && studentUserAccount is not null)
            {
                await _userAccountRepository.DeleteUserAccount(studentUserAccount, cancellationToken);
            }

            if(studentUserInserted && studentUser is not null)
            {
                await _userRepository.DeleteUser(studentUser, cancellationToken);
            }

            if(studentInfoInserted && studentInfo is not null)
            {
                await _studentInfoRepository.Delete(studentInfo, cancellationToken);
            }

            await _baseUnitOfWork.SaveChangesAsync(cancellationToken);

            error = new(nameof(DataException), "Error occurred while saving Student Info");

            return ResultModel.Fail(error);
        }
    }
}

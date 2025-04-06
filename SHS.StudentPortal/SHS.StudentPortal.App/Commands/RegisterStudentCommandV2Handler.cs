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

internal sealed class RegisterStudentCommandV2Handler : ICommandHandler<RegisterStudentCommandV2>
{
    private readonly ILogger<RegisterStudentCommandV2Handler> _logger;
    private readonly IUserAccountRepository _userAccountRepository;
    private readonly IBCryptAuthProvider _bCryptAuthProvider;
    private readonly IUserRepository _userRepository;
    private readonly ISectionRepository _sectionRepository;
    private readonly ICourseRepository _courseRepository;
    private readonly IStudentInfoRepository _studentInfoRepository;
    private readonly IExternalAcademicRecordRepository _externalAcademicRecordRepository;
    private readonly IBaseUnitOfWork _baseUnitOfWork;

    public RegisterStudentCommandV2Handler(ILogger<RegisterStudentCommandV2Handler> logger,
        IUserAccountRepository userAccountRepository,
        IBCryptAuthProvider bCryptAuthProvider,
        IUserRepository userRepository,
        ISectionRepository sectionRepository,
        ICourseRepository courseRepository,
        IStudentInfoRepository studentInfoRepository,
        IExternalAcademicRecordRepository externalAcademicRecordRepository,
        IBaseUnitOfWork baseUnitOfWork)
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
    }

    public async Task<ResultModel> Handle(RegisterStudentCommandV2 request, CancellationToken cancellationToken)
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
            if (request.view is null)
            {
                error = new(nameof(ArgumentNullException), "Student details cannot be null");

                return ResultModel.Fail(error);
            }
            else
            {
                var passwordHash = _bCryptAuthProvider.EncryptPassword(request.view.Password);

                var newStudentUserAccount = new UserAccount()
                    .Create(request.view.Username,
                        passwordHash,
                        request.view.EmailAddress,
                        Constants.SystemGuid);

                var insertedStudentUserAccount = await _userAccountRepository.CreateUserAccount(newStudentUserAccount, cancellationToken);

                await _baseUnitOfWork.SaveChangesAsync(cancellationToken);

                studentUserAccountInserted = true;
                studentUserAccount = insertedStudentUserAccount;

                var newStudentUser = new User()
                    .Create(request.view.FirstName,
                        request.view.MiddleName,
                        request.view.LastName,
                        RoleTypes.Student.Id,
                        insertedStudentUserAccount.Id,
                        Constants.SystemGuid);

                var insertedStudentUser = await _userRepository.CreateUser(newStudentUser, cancellationToken);

                await _baseUnitOfWork.SaveChangesAsync(cancellationToken);

                studentUserInserted = true;
                studentUser = insertedStudentUser;

                var placeHolderSection = await _sectionRepository.GetSectionByName(Constants.NotApplicable, cancellationToken: cancellationToken);
              
                var newStudentInfo = new StudentInfo()
                    .Create(
                        StudentStatuses.Pending.Id,
                        request.view.YearLevel,
                        placeHolderSection!.Id,
                        $"{Track.GetTrack(request.view.Track).Id}-{Strand.GetStrand(request.view.Strand ?? string.Empty).Id}",
                        insertedStudentUser.Id,
                        request.view.Gender,
                        DateOnly.FromDateTime(request.view.DateOfBirth!.Value),
                        request.view.PlaceOfBirth,
                        request.view.CivilStatus,
                        request.view.Nationality,
                        request.view.Religion,
                        request.view.ContactInfo,
                        request.view.Address,
                        Constants.SystemGuid);

                var insertedStudentInfo = await _studentInfoRepository
                    .Create(newStudentInfo, cancellationToken);

                await _baseUnitOfWork.SaveChangesAsync(cancellationToken);

                studentInfoInserted = true;
                studentInfo = insertedStudentInfo;

                await _baseUnitOfWork.SaveChangesAsync(cancellationToken);

                return ResultModel.Success();
            }

        }
        catch (Exception ex) 
        {
            _logger.LogError($"Error saving student info: {ex.Message}");

            // Rollback inserted accounts
            if (studentUserAccountInserted && studentUserAccount is not null)
            {
                await _userAccountRepository.DeleteUserAccount(studentUserAccount, cancellationToken);
            }

            if (studentUserInserted && studentUser is not null)
            {
                await _userRepository.DeleteUser(studentUser, cancellationToken);
            }

            if (studentInfoInserted && studentInfo is not null)
            {
                await _studentInfoRepository.Delete(studentInfo, cancellationToken);
            }

            await _baseUnitOfWork.SaveChangesAsync(cancellationToken);

            error = new(nameof(DataException), "Error occurred while saving Student Info");

            return ResultModel.Fail(error);
        }
    }
}

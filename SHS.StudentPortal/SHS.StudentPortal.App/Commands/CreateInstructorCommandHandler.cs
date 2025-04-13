using Microsoft.Extensions.Logging;
using SHS.StudentPortal.App.Abstractions.Messaging;
using SHS.StudentPortal.App.Abstractions.Persistence;
using SHS.StudentPortal.App.Abstractions.Repositories;
using SHS.StudentPortal.App.Abstractions.Security;
using SHS.StudentPortal.Common.Models;
using SHS.StudentPortal.Domain.Models;
using System.Data;

namespace SHS.StudentPortal.App.Commands;

internal sealed class CreateInstructorCommandHandler
    : ICommandHandler<CreateInstructorCommand>
{
    private readonly ILogger<CreateInstructorCommandHandler> _logger;
    private readonly IUserAccountRepository _userAccountRepository;
    private readonly IBCryptAuthProvider _bCryptAuthProvider;
    private readonly IUserRepository _userRepository;
    private readonly IInstructorInfoRepository _instructorInfoRepository;
    private readonly ISectionRepository _sectionRepository;
    private readonly IBaseUnitOfWork _baseUnitOfWork;

    public CreateInstructorCommandHandler(ILogger<CreateInstructorCommandHandler> logger,
        IUserAccountRepository userAccountRepository,
        IBCryptAuthProvider bCryptAuthProvider,
        IUserRepository userRepository,
        IInstructorInfoRepository instructorInfoRepository,
        ISectionRepository sectionRepository,
        IBaseUnitOfWork baseUnitOfWork)
    {
        _logger = logger;
        _userAccountRepository = userAccountRepository;
        _bCryptAuthProvider = bCryptAuthProvider;
        _userRepository = userRepository;
        _instructorInfoRepository = instructorInfoRepository;
        _sectionRepository = sectionRepository;
        _baseUnitOfWork = baseUnitOfWork;
    }

    public async Task<ResultModel> Handle(CreateInstructorCommand request, CancellationToken cancellationToken)
    {
        ErrorModel error;

        try
        {
            using (var txn = _baseUnitOfWork.BeginTransaction())
            {
                try
                {
                    var usernameHitUserAccount = await _userAccountRepository.GetUserAccountByUsername(request.view.Username, cancellationToken);

                    if (usernameHitUserAccount is not null)
                    {
                        error = new(nameof(DuplicateNameException), "Username already exists.");

                        return ResultModel.Fail(error);
                    }

                    var employeeIdHitInstructorInfo = await _instructorInfoRepository
                        .GetInstructorInfoByEmployeeId(request.view.EmployeeId.ToLower().Trim(), cancellation: cancellationToken);

                    if (employeeIdHitInstructorInfo is not null)
                    {
                        error = new(nameof(DuplicateNameException), "Employee with similar Id exists.");

                        return ResultModel.Fail(error);
                    }

                    var hashedPassword = _bCryptAuthProvider.EncryptPassword(request.view.Password);

                    var newInstrUserAccount = new UserAccount()
                        .CreateAdminOrInstr(request.view.Username,
                            hashedPassword,
                            request.view.EmailAddress,
                            request.createdById);

                    var insertedUserAccount = await _userAccountRepository.CreateUserAccount(newInstrUserAccount, cancellationToken);

                    await _baseUnitOfWork.SaveChangesAsync(cancellationToken);

                    var newInstrUser = new User()
                        .Create(request.view.FirstName,
                            string.IsNullOrWhiteSpace(request.view.MiddleName) ? null : request.view.MiddleName,
                            request.view.LastName,
                            RoleTypes.Instructor.Id,
                            insertedUserAccount.Id,
                            request.createdById);

                    var insertedUser = await _userRepository.CreateUser(newInstrUser, cancellationToken);

                    await _baseUnitOfWork.SaveChangesAsync(cancellationToken);

                    var newInstrInfo = new InstructorInfo()
                        .Create(request.view.EmployeeId,
                            request.view.Major,
                            request.view.ContactInfo,
                            insertedUser.Id,
                            request.view.DepartmentId!.Value!,
                            request.createdById);

                    var insertedInstrInfo = await _instructorInfoRepository.CreateInstructorInfo(newInstrInfo, cancellationToken);

                    await _baseUnitOfWork.SaveChangesAsync(cancellationToken);

                    var section = await _sectionRepository.GetById(request.view.AdvisorySectionId!.Value!, true, cancellationToken);

                    if (section is not null)
                    {
                        section.AdviserId = insertedInstrInfo.Id;
                        section.ModifiedById = request.createdById;
                        section.ModifiedDate = DateTime.UtcNow;

                        await _sectionRepository.UpdateSection(section, cancellationToken);

                        await _baseUnitOfWork.SaveChangesAsync(cancellationToken);
                    }

                    await txn.CommitAsync(cancellationToken);
                }
                catch (Exception)
                {
                    await txn.RollbackAsync(cancellationToken);

                    throw;
                }
            }

            return ResultModel.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating Instructor");

            error = new(nameof(Exception), "Error creating Instructor");

            return ResultModel.Fail(error);
        }
    }
}

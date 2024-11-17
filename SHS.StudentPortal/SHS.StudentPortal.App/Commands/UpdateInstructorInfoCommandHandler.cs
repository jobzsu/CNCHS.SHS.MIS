using Microsoft.Extensions.Logging;
using SHS.StudentPortal.App.Abstractions.Messaging;
using SHS.StudentPortal.App.Abstractions.Persistence;
using SHS.StudentPortal.App.Abstractions.Repositories;
using SHS.StudentPortal.Common;
using SHS.StudentPortal.Common.Models;

namespace SHS.StudentPortal.App.Commands;

internal sealed class UpdateInstructorInfoCommandHandler
    : ICommandHandler<UpdateInstructorInfoCommand>
{
    private readonly ILogger<UpdateInstructorInfoCommandHandler> _logger;
    private readonly IInstructorInfoRepository _instructorInfoRepository;
    private readonly ISectionRepository _sectionRepository;
    private readonly IBaseUnitOfWork _baseUnitOfWork;
    private readonly IUserRepository _userRepository;

    public UpdateInstructorInfoCommandHandler(ILogger<UpdateInstructorInfoCommandHandler> logger,
        IInstructorInfoRepository instructorInfoRepository,
        ISectionRepository sectionRepository,
        IBaseUnitOfWork baseUnitOfWork,
        IUserRepository userRepository)
    {
        _logger = logger;
        _instructorInfoRepository = instructorInfoRepository;
        _sectionRepository = sectionRepository;
        _baseUnitOfWork = baseUnitOfWork;
        _userRepository = userRepository;
    }

    public async Task<ResultModel> Handle(UpdateInstructorInfoCommand request, CancellationToken cancellationToken)
    {
        ErrorModel error = null;

        try
        {
            using(var txn = _baseUnitOfWork.BeginTransaction())
            {
                try
                {
                    var instructorInfo = await _instructorInfoRepository.GetInstructorInfoById(request.instructorId, true, cancellationToken);

                    if (instructorInfo == null)
                    {
                        error = new(nameof(KeyNotFoundException), "Instructor not found");

                        throw new KeyNotFoundException("Instructor not found");
                    }

                    var notApplicableSection = await _sectionRepository
                        .GetSectionByName(Constants.NotApplicable, cancellationToken: cancellationToken);
                    var previousadvisorySection = await _sectionRepository
                        .GetByAdviserId(request.instructorId, cancellationToken: cancellationToken);
                    var notApplicableUser = await _userRepository
                            .GetUserByFirstAndLastName(Constants.NotApplicable.ToLower(), Constants.NotApplicable.ToLower(), cancellationToken: cancellationToken);
                    if (notApplicableUser == null)
                    {
                        error = new(nameof(KeyNotFoundException), "Not Applicable User not found");

                        throw new KeyNotFoundException("Not Applicable User not found");
                    }
                    var notApplicableInstructorInfo = await _instructorInfoRepository.GetByUserId(notApplicableUser.Id, cancellationToken: cancellationToken);
                    if (notApplicableInstructorInfo is null)
                    {
                        error = new(nameof(KeyNotFoundException), "Not Applicable Instructor Info not found");

                        throw new KeyNotFoundException("Not Applicable Instructor Info not found");
                    }
                    var instructorInfoUser = await _userRepository.GetUserById(instructorInfo.UserId, true, cancellationToken);
                    if (instructorInfoUser is null)
                    {
                        error = new(nameof(KeyNotFoundException), "User for this Instructor not found");

                        throw new KeyNotFoundException("User for this Instructor not found");
                    }

                    if (previousadvisorySection is not null)
                    {
                        // We set the previous advisory section adviserId to NotApplicable Instructor Id
                        previousadvisorySection.AdviserId = notApplicableInstructorInfo.Id;
                        previousadvisorySection.ModifiedById = request.updatedById;
                        previousadvisorySection.ModifiedDate = DateTime.UtcNow;

                        await _sectionRepository.UpdateSection(previousadvisorySection, cancellationToken);
                        await _baseUnitOfWork.SaveChangesAsync(cancellationToken);
                    }

                    // Update new advisory section with Instructor Id as AdviserId
                    var newadvisorySection = await _sectionRepository.GetById(request.model.AdvisorySectionId, true, cancellationToken);
                    if (newadvisorySection == null)
                    {
                        error = new(nameof(KeyNotFoundException), "Section not found");

                        throw new KeyNotFoundException("Section not found");
                    }

                    // If new advisory section is not NotApplicable, update details
                    if(newadvisorySection.Id != notApplicableSection!.Id)
                    {
                        newadvisorySection.AdviserId = request.instructorId;
                        newadvisorySection.ModifiedById = request.updatedById;
                        newadvisorySection.ModifiedDate = DateTime.UtcNow;

                        await _sectionRepository.UpdateSection(newadvisorySection, cancellationToken);
                        await _baseUnitOfWork.SaveChangesAsync(cancellationToken);
                    }

                    instructorInfoUser.FirstName = request.model.FirstName;
                    if (!string.IsNullOrWhiteSpace(request.model.MiddleName))
                        instructorInfoUser.MiddleName = request.model.MiddleName;
                    instructorInfoUser.LastName = request.model.LastName;

                    instructorInfoUser.ModifiedById = request.updatedById;
                    instructorInfoUser.ModifiedDate = DateTime.UtcNow;

                    await _userRepository.UpdateUser(instructorInfoUser, cancellationToken);
                    await _baseUnitOfWork.SaveChangesAsync(cancellationToken);

                    instructorInfo.Major = request.model.Major;
                    instructorInfo.DepartmentId = request.model.DepartmentId;
                    instructorInfo.ContactInformation = request.model.ContactInfo;

                    instructorInfo.ModifiedById = request.updatedById;
                    instructorInfo.ModifiedDate = DateTime.UtcNow;

                    await _instructorInfoRepository.UpdateInstructorInfo(instructorInfo, cancellationToken);
                    await _baseUnitOfWork.SaveChangesAsync(cancellationToken);

                    await txn.CommitAsync(cancellationToken);

                    return ResultModel.Success();
                }
                catch (Exception)
                {
                    await txn.RollbackAsync(cancellationToken);

                    throw;
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating Instructor Info");

            if (error == null)
                error = new(nameof(Exception), "Error updating Instructor Info");

            return ResultModel.Fail(error);
        }
    }
}

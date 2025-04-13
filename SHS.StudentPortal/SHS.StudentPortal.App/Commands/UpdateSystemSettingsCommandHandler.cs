using Microsoft.Extensions.Logging;
using SHS.StudentPortal.App.Abstractions.Messaging;
using SHS.StudentPortal.App.Abstractions.Persistence;
using SHS.StudentPortal.App.Abstractions.Repositories;
using SHS.StudentPortal.Common;
using SHS.StudentPortal.Common.Models;

namespace SHS.StudentPortal.App.Commands;

internal sealed class UpdateSystemSettingsCommandHandler
    : ICommandHandler<UpdateSystemSettingsCommand>
{
    private readonly ILogger<UpdateSystemSettingsCommandHandler> _logger;
    private readonly IBaseUnitOfWork _unitOfWork;
    private readonly ISettingRepository _settingRepository;

    public UpdateSystemSettingsCommandHandler(ILogger<UpdateSystemSettingsCommandHandler> logger,
        IBaseUnitOfWork unitOfWork,
        ISettingRepository settingRepository)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _settingRepository = settingRepository;
    }

    public async Task<ResultModel> Handle(UpdateSystemSettingsCommand request, CancellationToken cancellationToken)
    {
        ErrorModel error;

        try
        {
            var academicYearSettings = await _settingRepository.GetSettingByName(Constants.CurrentAcademicYear, true, cancellationToken);

            if(academicYearSettings == null)
            {
                error = new(nameof(Exception), "Academic Year Settings not found");

                return ResultModel.Fail(error);
            }

            academicYearSettings.Value = request.academicYear;
            academicYearSettings.ModifiedById = request.actionById;
            academicYearSettings.ModifiedDate = DateTime.UtcNow;

            var semesterSettings = await _settingRepository.GetSettingByName(Constants.CurrentSemester, true, cancellationToken);

            if (semesterSettings == null)
            {
                error = new(nameof(Exception), "Semester Settings not found");

                return ResultModel.Fail(error);
            }

            semesterSettings.Value = request.semester;
            semesterSettings.ModifiedById = request.actionById;
            semesterSettings.ModifiedDate = DateTime.UtcNow;

            using var txn = _unitOfWork.BeginTransaction();

            try
            {
                await _settingRepository.UpdateSystemSetting(academicYearSettings, cancellationToken);

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                await _settingRepository.UpdateSystemSetting(semesterSettings, cancellationToken);

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                await txn.CommitAsync(cancellationToken);
            }
            catch (Exception)
            {
                await txn.RollbackAsync(cancellationToken);

                throw;
            }

            return ResultModel.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating System Settings");

            error = new(nameof(Exception), "Error updating System Settings");

            return ResultModel.Fail(error);
        }
    }
}

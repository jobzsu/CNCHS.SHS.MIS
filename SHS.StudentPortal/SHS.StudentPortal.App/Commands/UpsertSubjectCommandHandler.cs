using Microsoft.Extensions.Logging;
using SHS.StudentPortal.App.Abstractions.Messaging;
using SHS.StudentPortal.App.Abstractions.Persistence;
using SHS.StudentPortal.App.Abstractions.Repositories;
using SHS.StudentPortal.Common.Models;
using SHS.StudentPortal.Domain.Models;

namespace SHS.StudentPortal.App.Commands;

internal sealed class UpsertSubjectCommandHandler
    : ICommandHandler<UpsertSubjectCommand>
{
    private readonly ILogger<UpsertSubjectCommandHandler> _logger;
    private readonly IBaseUnitOfWork _unitOfWork;
    private readonly ISubjectRepository _subjectRepository;

    public UpsertSubjectCommandHandler(ILogger<UpsertSubjectCommandHandler> logger,
        IBaseUnitOfWork unitOfWork,
        ISubjectRepository subjectRepository)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _subjectRepository = subjectRepository;
    }

    public async Task<ResultModel> Handle(UpsertSubjectCommand request, CancellationToken cancellationToken)
    {
        ErrorModel error;

        try
        {

            if (request.view.Id == 0)
            {
                var subjectHitSameName = await _subjectRepository.GetSubjectByName(request.view.Name.ToLower().Trim(), true, cancellationToken);

                if (subjectHitSameName is not null)
                {
                    error = new(nameof(InvalidOperationException), "Subject with same name already exists");

                    return ResultModel.Fail(error);
                }

                var subjectHitSameCode = await _subjectRepository.GetSubjectByCode(request.view.Code.ToLower().Trim(), true, cancellationToken);

                if (subjectHitSameCode is not null)
                {
                    error = new(nameof(InvalidOperationException), "Subject with same code already exists");

                    return ResultModel.Fail(error);
                }

                var trackId = Track.GetTrack(request.view.TrackId ?? string.Empty);

                if (trackId is null)
                {
                    error = new(nameof(KeyNotFoundException), "Track not found");

                    return ResultModel.Fail(error);
                }

                var strandId = Strand.GetStrand(request.view.StrandId ?? string.Empty);

                if (strandId is null)
                {
                    error = new(nameof(KeyNotFoundException), "Strand not found");

                    return ResultModel.Fail(error);
                }

                var trackAndStrand = $"{trackId.Id}-{strandId.Id}";

                var newSubject = new Subject().Create(request.view.Code,
                    request.view.Name,
                    request.view.Hours,
                    request.view.Days,
                    string.IsNullOrWhiteSpace(request.view.Description) ? string.Empty : request.view.Description,
                    request.view.Units,
                    request.view.Semester,
                    request.view.AcademicYear,
                    trackAndStrand,
                    request.actionById);

                using var txn = _unitOfWork.BeginTransaction();

                try
                {
                    await _subjectRepository.CreateSubject(newSubject, cancellationToken);

                    await _unitOfWork.SaveChangesAsync(cancellationToken);

                    await txn.CommitAsync(cancellationToken);
                }
                catch (Exception)
                {
                    await txn.RollbackAsync(cancellationToken);

                    throw;
                }
            }
            else
            {
                var subject = await _subjectRepository.GetSubjectById(request.view.Id, true, cancellationToken);

                if (subject is null)
                {
                    error = new(nameof(KeyNotFoundException), "Subject not found");

                    return ResultModel.Fail(error);
                }

                var subjectHitSameName = await _subjectRepository.GetSubjectByName(request.view.Name.ToLower().Trim(), true, cancellationToken);

                if (subjectHitSameName is not null && subjectHitSameName.Id != subject.Id)
                {
                    error = new(nameof(InvalidOperationException), "Subject with same name already exists");

                    return ResultModel.Fail(error);
                }

                var subjectHitSameCode = await _subjectRepository.GetSubjectByCode(request.view.Code.ToLower().Trim(), true, cancellationToken);

                if (subjectHitSameCode is not null && subjectHitSameCode.Id != subject.Id)
                {
                    error = new(nameof(InvalidOperationException), "Subject with same code already exists");

                    return ResultModel.Fail(error);
                }

                var trackId = Track.GetTrack(request.view.TrackId ?? string.Empty);

                if (trackId is null)
                {
                    error = new(nameof(KeyNotFoundException), "Track not found");

                    return ResultModel.Fail(error);
                }

                var strandId = Strand.GetStrand(request.view.StrandId ?? string.Empty);

                if (strandId is null)
                {
                    error = new(nameof(KeyNotFoundException), "Strand not found");

                    return ResultModel.Fail(error);
                }

                var trackAndStrand = $"{trackId.Id}-{strandId.Id}";

                // Update subject
                subject.Update(request.view.Code,
                    request.view.Name,
                    request.view.Hours,
                    request.view.Days,
                    request.view.Description,
                    request.view.Units,
                    request.view.Semester,
                    request.view.AcademicYear,
                    trackAndStrand,
                    request.actionById);

                using var txn = _unitOfWork.BeginTransaction();

                try
                {
                    await _subjectRepository.UpdateSubject(subject, cancellationToken);

                    await _unitOfWork.SaveChangesAsync(cancellationToken);

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
            _logger.LogError(ex, "Error Saving Subject");

            error = new(nameof(Exception), "Error Saving Subject");

            return ResultModel.Fail(error);
        }
    }
}

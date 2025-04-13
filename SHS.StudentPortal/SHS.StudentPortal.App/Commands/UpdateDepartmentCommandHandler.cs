using Microsoft.Extensions.Logging;
using SHS.StudentPortal.App.Abstractions.Messaging;
using SHS.StudentPortal.App.Abstractions.Persistence;
using SHS.StudentPortal.App.Abstractions.Repositories;
using SHS.StudentPortal.Common.Models;
using System.Data;

namespace SHS.StudentPortal.App.Commands;

internal sealed class UpdateDepartmentCommandHandler
    : ICommandHandler<UpdateDepartmentCommand>
{
    private readonly ILogger<UpdateDepartmentCommandHandler> _logger;
    private readonly IDepartmentRepository _departmentRepository;
    private readonly IBaseUnitOfWork _baseUnitOfWork;

    public UpdateDepartmentCommandHandler(ILogger<UpdateDepartmentCommandHandler> logger,
        IDepartmentRepository departmentRepository,
        IBaseUnitOfWork baseUnitOfWork)
    {
        _logger = logger;
        _departmentRepository = departmentRepository;
        _baseUnitOfWork = baseUnitOfWork;
    }

    public async Task<ResultModel> Handle(UpdateDepartmentCommand request, CancellationToken cancellationToken)
    {
        ErrorModel error = null;

        try
        {
            using (var txn = _baseUnitOfWork.BeginTransaction())
            {
                try
                {
                    var existingDepartment = await _departmentRepository
                        .GetDepartmentByName(request.view.Name.ToLower(), cancellationToken: cancellationToken);

                    if (existingDepartment is not null && existingDepartment.Id != request.view.Id)
                    {
                        error = new(nameof(DuplicateNameException), "Department name already exists");

                        throw new DuplicateNameException("Department name already exists.");
                    }

                    var department = await _departmentRepository
                        .GetDepartmentById(request.view.Id, true, cancellationToken);

                    if(department is null)
                    {
                        error = new(nameof(KeyNotFoundException), "Department not found");

                        throw new KeyNotFoundException("Department not found");
                    }

                    department.Name = request.view.Name;
                    department.Description = string.IsNullOrWhiteSpace(request.view.Description) ? null : request.view.Description;

                    department.ModifiedById = request.updatedById;
                    department.ModifiedDate = DateTime.UtcNow;

                    await _departmentRepository.UpdateDepartment(department, cancellationToken);

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
            _logger.LogError(ex, "Error updating Department");

            if (error is null)
                error = new(nameof(Exception), "Error updating Department");

            return ResultModel.Fail(error);
        }
    }
}

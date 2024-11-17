using Microsoft.Extensions.Logging;
using SHS.StudentPortal.App.Abstractions.Messaging;
using SHS.StudentPortal.App.Abstractions.Persistence;
using SHS.StudentPortal.App.Abstractions.Repositories;
using SHS.StudentPortal.Common.Models;
using SHS.StudentPortal.Domain.Models;
using System.Data;

namespace SHS.StudentPortal.App.Commands;

internal sealed class CreateDepartmentCommandHandler
    : ICommandHandler<CreateDepartmentCommand>
{
    private readonly ILogger<CreateDepartmentCommandHandler> _logger;
    private readonly IDepartmentRepository _departmentRepository;
    private readonly IBaseUnitOfWork _baseUnitOfWork;

    public CreateDepartmentCommandHandler(ILogger<CreateDepartmentCommandHandler> logger,
        IDepartmentRepository departmentRepository,
        IBaseUnitOfWork baseUnitOfWork)
    {
        _logger = logger;
        _departmentRepository = departmentRepository;
        _baseUnitOfWork = baseUnitOfWork;
    }

    public async Task<ResultModel> Handle(CreateDepartmentCommand request, CancellationToken cancellationToken)
    {
        ErrorModel error = null;

        try
        {
            using(var txn = _baseUnitOfWork.BeginTransaction())
            {
                try
                {
                    // Check if name exists
                    var existingDept = await _departmentRepository.GetDepartmentByName(request.view.Name, cancellationToken: cancellationToken);

                    if(existingDept is not null)
                    {
                        error = new(nameof(DuplicateNameException), "Department name already exists.");

                        throw new DuplicateNameException("Department name already exists.");
                    }

                    var newDept = new Department()
                        .Create(request.view.Name, request.view.Description, request.createdById);

                    await _departmentRepository.CreateDepartment(newDept, cancellationToken);

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
            _logger.LogError(ex, "Error creating department.");

            if (error is null)
                error = new(nameof(Exception), "Error creating department.");

            return ResultModel.Fail(error);
        }
    }
}

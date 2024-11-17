using SHS.StudentPortal.Domain.Models;

namespace SHS.StudentPortal.App.Abstractions.Repositories;

public interface IInstructorInfoRepository
{
    Task<List<InstructorInfo>> GetListViaFilter(string? keyword,
        int departmentId,
        CancellationToken cancellationToken = default);

    Task<List<InstructorInfo>?> GetList(bool includePlaceholder,
        bool shouldTrack = false,
        CancellationToken cancellationToken = default);

    Task<InstructorInfo?> GetInstructorInfoById(Guid instructorId,
        bool shouldTrack = false,
        CancellationToken cancellationToken = default);

    Task<InstructorInfo?> GetInstructorInfoByEmployeeId(string employeeId,
        bool shouldTrack = false,
        CancellationToken cancellation = default);

    Task<List<InstructorInfo>?> GetInstructorsByDepartment(int departmentId, bool shouldTrack = false, CancellationToken cancellationToken = default);

    Task<InstructorInfo> CreateInstructorInfo(InstructorInfo instructorInfo, CancellationToken cancellationToken = default);

    Task<InstructorInfo> UpdateInstructorInfo(InstructorInfo instructorInfo, CancellationToken cancellationToken = default);

    Task<InstructorInfo?> GetByUserId(Guid userId, bool shouldTrack = false, CancellationToken cancellationToken = default);
}

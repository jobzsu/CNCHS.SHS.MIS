using SHS.StudentPortal.Domain.Models;

namespace SHS.StudentPortal.App.Abstractions.Repositories;

public interface IStudentInfoRepository
{
    Task<StudentInfo> Create(StudentInfo studentInfo, CancellationToken cancellationToken = default);

    Task<StudentInfo> Update(StudentInfo studentInfo, CancellationToken cancellationToken = default);

    Task Delete(StudentInfo studentInfo, CancellationToken cancellationToken = default);

    Task<StudentInfo?> GetById(Guid id, 
        bool shouldTrack = false, 
        CancellationToken cancellationToken = default);

    Task<List<StudentInfo>> GetListViaFilter(string? keyword, 
        int yearLevel,
        Guid sectionId,
        string track,
        string strand,
        CancellationToken cancellationToken = default);

    Task<List<StudentInfo>?> GetList(CancellationToken cancellationToken = default);
}

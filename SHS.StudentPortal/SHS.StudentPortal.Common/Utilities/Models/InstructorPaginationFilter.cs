namespace SHS.StudentPortal.Common.Utilities.Models;

public class InstructorPaginationFilter: PaginationFilter
{
    public int DepartmentId { get; private set; }

    private InstructorPaginationFilter(string? searchKeyword,
        int departmentId,
        int pageNumber)
    {
        SearchKeyword = searchKeyword;
        DepartmentId = departmentId;
        PageNumber = pageNumber;
    }

    public static InstructorPaginationFilter NewInstructorListSearch(string? searchKeyword,
        int departmentId,
        int pageNumber)
    {
        searchKeyword = !string.IsNullOrWhiteSpace(searchKeyword) ? searchKeyword.Trim().ToLower() : searchKeyword;

        return new InstructorPaginationFilter(searchKeyword, departmentId, pageNumber);
    }
}

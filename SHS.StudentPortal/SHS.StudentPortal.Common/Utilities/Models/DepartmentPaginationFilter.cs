namespace SHS.StudentPortal.Common.Utilities.Models;

public class DepartmentPaginationFilter : PaginationFilter
{
    private DepartmentPaginationFilter(string? searchKeyword,
        int pageNumber)
    {
        SearchKeyword = searchKeyword;
        PageNumber = pageNumber;
    }

    public static DepartmentPaginationFilter NewDepartmentListSearch(string? searchKeyword, int pageNumber)
    {
        searchKeyword = !string.IsNullOrWhiteSpace(searchKeyword) ? 
            searchKeyword.Trim().ToLower() : searchKeyword;
        
        return new DepartmentPaginationFilter(searchKeyword, pageNumber);
    }
}

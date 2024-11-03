using SHS.StudentPortal.Common.Models;

namespace SHS.StudentPortal.Common.Utilities.Models;

public class InstructorPaginatedList
{
    public PaginatedList<InstructorListViewModel> InstructorList { get; set; }

    public List<KeyValuePair<int, string>> DepartmentList { get; set; }
}

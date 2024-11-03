namespace SHS.StudentPortal.Common.Utilities.Models;

public class StudentPaginationFilter : PaginationFilter
{
    public int YearLevel { get; private set; }

    public string? SectionId { get; private set; }

    public string? Track { get; set; }

    public string? Strand { get; set; }

    private StudentPaginationFilter(string? searchKeyword,
         int yearLevel,
         string? sectionId,
         string? track,
         string? strand,
         int pageNumber)
    {
        SearchKeyword = searchKeyword;

        YearLevel = yearLevel;
        SectionId = sectionId;
        Track = track;
        Strand = strand;

        PageNumber = pageNumber;
    }

    public static StudentPaginationFilter NewStudentListSearch(string? searchKeyword,
         int yearLevel,
         string? sectionId,
         string? track,
         string? strand,
         int pageNumber)
    {
        searchKeyword = !string.IsNullOrWhiteSpace(searchKeyword) ? searchKeyword.Trim().ToLower() : searchKeyword;

        sectionId = !string.IsNullOrWhiteSpace(sectionId) ? sectionId.Trim().ToLower() : sectionId;
        track = !string.IsNullOrWhiteSpace(track) ? track.Trim().ToLower() : track;
        strand = !string.IsNullOrWhiteSpace(strand) ? strand.Trim().ToLower() : strand;

        return new StudentPaginationFilter(searchKeyword, yearLevel, sectionId, track, strand, pageNumber);
    }
}

namespace SHS.StudentPortal.Common.Utilities.Models;

public class SubjectsPaginationFilter : PaginationFilter
{
    public string? Track { get; set; }

    public string? Strand { get; set; }

    private SubjectsPaginationFilter(string? searchKeyword, 
        string? track, 
        string? strand, 
        int pageNumber) 
    { 
        SearchKeyword = searchKeyword;

        Track = track; 
        Strand = strand; 

        PageNumber = pageNumber;
    }

    public static SubjectsPaginationFilter NewSubjectsListSearch(string? searchKeyword, 
        string? track, 
        string? strand, 
        int pageNumber) 
    { 
        searchKeyword = !string.IsNullOrWhiteSpace(searchKeyword) ? searchKeyword.Trim().ToLower() : searchKeyword;

        track = !string.IsNullOrWhiteSpace(track) ? track.Trim().ToLower() : track;
        strand = !string.IsNullOrWhiteSpace(strand) ? strand.Trim().ToLower() : strand;

        return new SubjectsPaginationFilter(searchKeyword, track, strand, pageNumber);
    }
}

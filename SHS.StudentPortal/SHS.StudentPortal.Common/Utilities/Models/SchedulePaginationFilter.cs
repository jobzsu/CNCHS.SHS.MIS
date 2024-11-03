using SHS.StudentPortal.Common.Models;

namespace SHS.StudentPortal.Common.Utilities.Models;

public class SchedulePaginationFilter : PaginationFilter
{
    public string Days { get; set; }

    public string? Track { get; set; }

    public string? Strand { get; set; }

    private SchedulePaginationFilter(string? searchKeyword,
        string days,
        string? track,
        string? strand,
        int pageNumber)
    {
        SearchKeyword = searchKeyword;

        Days = days;
        Track = track;
        Strand = strand;

        PageNumber = pageNumber;
    }

    public static SchedulePaginationFilter NewScheduleListSearch(string? searchKeyword,
        string days,
        string? track,
        string? strand,
        int pageNumber)
    {
        searchKeyword = !string.IsNullOrWhiteSpace(searchKeyword) ? searchKeyword.Trim().ToLower() : searchKeyword;

        //days = !string.IsNullOrWhiteSpace(days) ? days.Trim().ToLower() : days;
        track = !string.IsNullOrWhiteSpace(track) ? track.Trim().ToLower() : track;
        strand = !string.IsNullOrWhiteSpace(strand) ? strand.Trim().ToLower() : strand;

        return new SchedulePaginationFilter(searchKeyword, days, track, strand, pageNumber);
    }
}

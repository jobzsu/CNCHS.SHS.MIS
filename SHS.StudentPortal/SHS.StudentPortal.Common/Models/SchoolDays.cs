namespace SHS.StudentPortal.Common.Models;

public class SchoolDays
{
    public string Id { get; private set; }

    public string Name { get; private set; }

    private SchoolDays(string id, string name)
    {
        Id = id;
        Name = name;
    }

    public static SchoolDays Monday => new("monday", "M");
    public static SchoolDays Tuesday => new("tuesday", "TU");
    public static SchoolDays Wednesday => new("wednesday", "W");
    public static SchoolDays Thursday => new("thursday", "TH");
    public static SchoolDays Friday => new("friday", "F");
    public static SchoolDays Saturday => new("saturday", "S");

    public static List<SchoolDays> AllDaysList => 
        new() { Monday, Tuesday, Wednesday, Thursday, Friday, Saturday };

    public static SchoolDays Get(string id) => 
        AllDaysList.FirstOrDefault(x => x.Id == id) ?? 
            throw new NullReferenceException("School Day not found");
}

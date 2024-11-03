namespace SHS.StudentPortal.Common.Models;

public class Track
{
    public string Id { get; set; }
    public string Name { get; set; }
    public List<Strand> Strands { get; set; }

    private Track(string id, string name, List<Strand> strands)
    {
        Id = id;
        Name = name;
        Strands = strands;
    }

    public static Track AcademicTrack => new("academic", 
        "Academic Track", 
        Strand.GetAllAcademicTrackStrands);

    public static Track ArtsAndDesignTrack => new("artsanddesign", 
        "Arts and Design Track",
        Strand.GetAllArtsAndDesignTrackStrands);

    public static Track SportsTrack => new("sports", 
        "Sports Track",
        Strand.GetAllSportsTrackStrands);

    public static Track TechnicalVocationalTrack => new("technicalvocational", 
        "TVL Track",
        Strand.GetAllTVLTrackStrands);

    public static Track Placeholder => new("",
        "N/A",
        new() { Strand.Placeholder });

    public static List<Track> GetAllTracks => new() 
    { 
        AcademicTrack, 
        ArtsAndDesignTrack, 
        SportsTrack, 
        TechnicalVocationalTrack 
    };

    public static List<Track> GetAllTracksIncludePlaceholder => new()
    {
        AcademicTrack,
        ArtsAndDesignTrack,
        SportsTrack,
        TechnicalVocationalTrack,
        Placeholder
    };

    public static Track GetTrack(string id) => GetAllTracksIncludePlaceholder
        .FirstOrDefault(x => x.Id == id) ?? throw new NullReferenceException("Track not found");

    public bool IsAcademicTrack => this == AcademicTrack;
    public bool IsArtsAndDesignTrack => this == ArtsAndDesignTrack;
    public bool IsSportsTrack => this == SportsTrack;
    public bool IsTechnicalVocationalTrack => this == TechnicalVocationalTrack;
}

public class Strand
{
    public string Id { get; set; }
    public string Name { get; set; }

    private Strand(string id, string name)
    {
        Id = id;
        Name = name;
    }

    // Academic Track
    public static Strand ABM => new("abm", "Accountancy, Business and Management (ABM)");
    public static Strand STEM => new("stem", "Science, Technology, Engineering and Mathematics (STEM)");
    public static Strand HUMSS => new("humss", "Humanities and Social Sciences (HUMSS)");
    public static Strand GAS => new("gas", "General Academic Strand (GAS)");

    // Arts and Design Track
    // None

    // Sports Track
    // None

    // Technical-Vocational-Livelihood (TVL) Track
    public static Strand AFA => new("afa", "Agricultural-Fishery Arts (AFA)");
    public static Strand HE => new("he", "Home Economics (HE)");
    public static Strand IA => new("ia", "Industrial Arts (IA)");
    public static Strand ICT => new("ict", "Information and Communications Technology (ICT)");

    public static Strand Placeholder => new("", "N/A");

    public static List<Strand> GetAllStrands => new() { ABM, STEM, HUMSS, GAS, AFA, HE, IA, ICT };
    public static List<Strand> GetAllStrandsIncludePlaceholder => new() { ABM, STEM, HUMSS, GAS, AFA, HE, IA, ICT, Placeholder };

    public static List<Strand> GetAllAcademicTrackStrands => new() { ABM, STEM, HUMSS, GAS };
    public static List<Strand> GetAllAcademicTrackStrandsIncludePlaceholder => new() { ABM, STEM, HUMSS, GAS, Placeholder };

    public static List<Strand> GetAllArtsAndDesignTrackStrands => new() { Placeholder };

    public static List<Strand> GetAllSportsTrackStrands => new() { Placeholder };

    public static List<Strand> GetAllTVLTrackStrands => new() { AFA, HE, IA, ICT };
    public static List<Strand> GetAllTVLTrackStrandsIncludePlaceholder => new() { AFA, HE, IA, ICT, Placeholder };

    public static Strand GetStrand(string id) => 
        GetAllStrandsIncludePlaceholder.FirstOrDefault(x => x.Id == id) ?? 
            throw new NullReferenceException("Strand not found");

    public bool IsABM => Id == ABM.Id;
    public bool IsSTEM => Id == STEM.Id;
    public bool IsHUMSS => Id == HUMSS.Id;
    public bool IsGAS => Id == GAS.Id;
    public bool IsAFA => Id == AFA.Id;
    public bool IsHE => Id == HE.Id;
    public bool IsIA => Id == IA.Id;
    public bool IsICT => Id == ICT.Id;
    public bool IsPlaceholder => Id == Placeholder.Id;
}

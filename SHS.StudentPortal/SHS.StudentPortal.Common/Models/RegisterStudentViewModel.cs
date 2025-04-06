using System.ComponentModel.DataAnnotations;

namespace SHS.StudentPortal.Common.Models;

public class RegisterStudentViewModel
{
    // -- Login Info
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid Email Address")]
    public string EmailAddress { get; set; }

    [Required(ErrorMessage = "Username is required")]
    [MaxLength(20, ErrorMessage = "Username cannot be longer than 20 characters")]
    [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Username can only contain letters and numbers")]
    public string Username { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
    public string Password { get; set; }

    [Required(ErrorMessage = "Confirm Password is required")]
    [Compare("Password", ErrorMessage = "Passwords do not match")]
    public string ConfirmPassword { get; set; }

    // -- Personal Info
    [Required(ErrorMessage = "First Name is required")]
    [MaxLength(100, ErrorMessage = "First Name cannot be longer than 100 characters")]
    public string FirstName { get; set; }

    [MaxLength(100, ErrorMessage = "Middle Name cannot be longer than 100 characters")]
    public string? MiddleName { get; set; }

    [Required(ErrorMessage = "Last Name is required")]
    [MaxLength(100, ErrorMessage = "Last Name cannot be longer than 100 characters")]
    public string LastName { get; set; }

    [Required(ErrorMessage = "Gender is required", AllowEmptyStrings = false)]
    public string Gender { get; set; }

    [Required(ErrorMessage = "Date of Birth is required")]
    public string DateOfBirth { get; set; }

    [Required(ErrorMessage = "Place of Birth is required")]
    public string PlaceOfBirth { get; set; }

    [Required(ErrorMessage = "Civil Status is required", AllowEmptyStrings = false)]
    public string CivilStatus { get; set; }

    [Required(ErrorMessage = "Nationality is required")]
    public string Nationality { get; set; }

    [Required(ErrorMessage = "Religion is required")]
    public string Religion { get; set; }

    [Required(ErrorMessage = "Contact Info is required")]
    public string ContactInfo { get; set; }

    [Required(ErrorMessage = "Address is required")]
    public string Address { get; set; }

    // -- Academic Info
    [Required(ErrorMessage = "Year Level is required")]
    [Range(11, 12, ErrorMessage = "Year Level must be either Grade 11 or 12")]
    public int YearLevel { get; set; }

    [Required(ErrorMessage = "Track is required")]
    public string Track { get; set; }

    public string? Strand { get; set; }

    //[MinLength(1, ErrorMessage = "Missing Academic Records")]
    public List<AcademicRecordViewModel> ExternalAcademicRecords { get; set; } = new();
}

public class RegisterStudentRazorViewModel
{
    // -- Login Info
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid Email Address")]
    public string EmailAddress { get; set; }

    [Required(ErrorMessage = "Username is required")]
    [MaxLength(20, ErrorMessage = "Username cannot be longer than 20 characters")]
    [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Username can only contain letters and numbers")]
    public string Username { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
    public string Password { get; set; }

    [Required(ErrorMessage = "Confirm Password is required")]
    [Compare("Password", ErrorMessage = "Passwords do not match")]
    public string ConfirmPassword { get; set; }

    // -- Personal Info
    [Required(ErrorMessage = "First Name is required")]
    [MaxLength(100, ErrorMessage = "First Name cannot be longer than 100 characters")]
    public string FirstName { get; set; }

    [MaxLength(100, ErrorMessage = "Middle Name cannot be longer than 100 characters")]
    public string? MiddleName { get; set; }

    [Required(ErrorMessage = "Last Name is required")]
    [MaxLength(100, ErrorMessage = "Last Name cannot be longer than 100 characters")]
    public string LastName { get; set; }

    [Required(ErrorMessage = "Gender is required", AllowEmptyStrings = false)]
    public string Gender { get; set; }

    [Required(ErrorMessage = "Date of Birth is required")]
    public DateTime? DateOfBirth { get; set; }

    [Required(ErrorMessage = "Place of Birth is required")]
    public string PlaceOfBirth { get; set; }

    [Required(ErrorMessage = "Civil Status is required", AllowEmptyStrings = false)]
    public string CivilStatus { get; set; }

    [Required(ErrorMessage = "Nationality is required")]
    public string Nationality { get; set; }

    [Required(ErrorMessage = "Religion is required")]
    public string Religion { get; set; }

    [Required(ErrorMessage = "Contact Info is required")]
    public string ContactInfo { get; set; }

    [Required(ErrorMessage = "Address is required")]
    public string Address { get; set; }

    // -- Academic Info
    [Required(ErrorMessage = "Year Level is required")]
    [Range(11, 12, ErrorMessage = "Year Level must be either Grade 11 or 12")]
    public int YearLevel { get; set; }

    [Required(ErrorMessage = "Track is required")]
    public string Track { get; set; }

    public string? Strand { get; set; }

    //[MinLength(1, ErrorMessage = "Missing Academic Records")]
    public List<AcademicRecordViewModel> ExternalAcademicRecords { get; set; } = new();
}
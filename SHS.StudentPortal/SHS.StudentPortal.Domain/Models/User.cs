using System.ComponentModel.DataAnnotations.Schema;

namespace SHS.StudentPortal.Domain.Models;

public class User : BaseModel<Guid>, IAuditable
{
    public string FirstName { get; set; }

    public string? MiddleName { get; set; }

    public string LastName { get; set; }

    public string? ProfilePicture { get; set; }

    public string RoleType { get; set; }

    public Guid UserAccountId { get; set; }

    [NotMapped]
    public UserAccount UserAccount { get; set; }

    [NotMapped]
    public InstructorInfo? InstructorInfo { get; set; }

    [NotMapped]
    public StudentInfo? StudentInfo { get; set; }

    #region > IAuditable Properties
    public Guid? CreatedById { get; set; }

    public DateTime? CreatedDate { get; set; }

    public Guid? ModifiedById { get; set; }

    public DateTime? ModifiedDate { get; set; }
    #endregion

    public User Create(string firstName,
        string? middleName,
        string lastName,
        string roleType,
        Guid userAccountId,
        Guid createdById)
    {
        FirstName = firstName;
        MiddleName = middleName;
        LastName = lastName;
        RoleType = roleType;
        UserAccountId = userAccountId;

        CreatedById = createdById;
        CreatedDate = DateTime.UtcNow;

        return this;
    }

    public User Update(string firstname,
        string? middleName,
        string lastName,
        Guid updatedById)
    {
        FirstName = firstname;
        MiddleName = middleName;
        LastName = lastName;

        ModifiedById = updatedById;
        ModifiedDate = DateTime.UtcNow;

        return this;
    }
}

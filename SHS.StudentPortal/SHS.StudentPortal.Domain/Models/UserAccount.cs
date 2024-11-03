using SHS.StudentPortal.Domain.Events.UserAccount;
using System.ComponentModel.DataAnnotations.Schema;

namespace SHS.StudentPortal.Domain.Models;

public class UserAccount : BaseModel<Guid>, IAuditable
{
    public string Username { get; set; }

    public string PasswordHash { get; set; }

    public string EmailAddress { get; set; }

    public DateTime? LastLogin { get; set; }

    public int LoginRetries { get; set; }

    public bool IsActive { get; set; }

    public string? InactiveReason { get; set; }

    [NotMapped]
    public User User { get; set; }

    #region > IAuditable Properties
    public Guid? CreatedById { get; set; }

    public DateTime? CreatedDate { get; set; }

    public Guid? ModifiedById { get; set; }

    public DateTime? ModifiedDate { get; set; }
    #endregion

    public UserAccount Create(string username,
        string passwordHash,
        string emailAddress,
        Guid createdById)
    {
        Username = username.ToLower().Trim();
        PasswordHash = passwordHash;
        EmailAddress = emailAddress.ToLower().Trim();
        LastLogin = null;
        IsActive = false;
        InactiveReason = "Pending Approval";

        CreatedById = createdById;
        CreatedDate = DateTime.UtcNow;

        RegisterEvent(new UserAccountCreatedEvent(this));

        return this;
    }

    public UserAccount CreateAdminOrInstr(string username,
        string passwordHash,
        string emailAddress,
        Guid createdById)
    {
        Username = username.ToLower().Trim();
        PasswordHash = passwordHash;
        EmailAddress = emailAddress.ToLower().Trim();
        LastLogin = null;
        IsActive = true;
        InactiveReason = null;

        CreatedById = createdById;
        CreatedDate = DateTime.UtcNow;

        // No need to register an event

        return this;
    }

    public UserAccount SetToActive(Guid modifiedById)
    {
        IsActive = true;
        InactiveReason = null;

        ModifiedById = modifiedById;
        ModifiedDate = DateTime.UtcNow;

        return this;
    }

    public UserAccount UpdatePassword(string newPasswordHash, Guid updatedById)
    {
        PasswordHash = newPasswordHash;

        ModifiedById = updatedById;
        ModifiedDate = DateTime.UtcNow;

        return this;
    }
}

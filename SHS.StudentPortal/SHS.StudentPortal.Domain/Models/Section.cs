using System.ComponentModel.DataAnnotations.Schema;

namespace SHS.StudentPortal.Domain.Models;

public class Section : BaseModel<Guid>, IAuditable
{
    public string Name { get; set; }

    public Guid AdviserId { get; set; }

    [NotMapped]
    public ICollection<StudentInfo> Students { get; set; }

    #region > IAuditable Properties
    public Guid? CreatedById { get; set; }

    public DateTime? CreatedDate { get; set; }

    public Guid? ModifiedById { get; set; }

    public DateTime? ModifiedDate { get; set; } 
    #endregion

    public Section Create(string name, Guid adviserId, Guid createdById)
    {
        Name = name;
        AdviserId = adviserId;

        CreatedById = createdById;
        CreatedDate = DateTime.UtcNow;

        return this;
    }
}

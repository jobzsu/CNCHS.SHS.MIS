using System.ComponentModel.DataAnnotations.Schema;

namespace SHS.StudentPortal.Domain.Models;

public class PreRequisite : BaseModel<int>, IAuditable
{
    public int PreRequisiteSubjectId { get; set; }

    [NotMapped]
    public Subject PreRequisiteSubject { get; set; }

    public int SubjectId { get; set; }

    [NotMapped]
    public Subject Subject { get; set; }

    #region > IAuditable Properties
    public Guid? CreatedById { get; set; }

    public DateTime? CreatedDate { get; set; }

    public Guid? ModifiedById { get; set; }

    public DateTime? ModifiedDate { get; set; }
    #endregion
}

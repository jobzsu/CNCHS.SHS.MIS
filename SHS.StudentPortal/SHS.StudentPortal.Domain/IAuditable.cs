namespace SHS.StudentPortal.Domain;

public interface IAuditable
{
    Guid? CreatedById { get; set; }

    DateTime? CreatedDate { get; set; }

    Guid? ModifiedById { get; set; }

    DateTime? ModifiedDate { get; set; }
}

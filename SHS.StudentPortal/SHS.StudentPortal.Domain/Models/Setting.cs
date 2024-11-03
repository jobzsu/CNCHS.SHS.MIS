namespace SHS.StudentPortal.Domain.Models;

public class Setting : BaseModel<Guid>, IAuditable
{
    public string Name { get; set; }

    public string Value { get; set; }

    public Guid? CreatedById { get; set; }
    public DateTime? CreatedDate { get; set; }
    public Guid? ModifiedById { get; set; }
    public DateTime? ModifiedDate { get; set; }

    public Setting Create(string name,
        string value,
        Guid createdById)
    {
        Name = name;
        Value = value;

        CreatedById = createdById;
        CreatedDate = DateTime.UtcNow;

        return this;
    }
}

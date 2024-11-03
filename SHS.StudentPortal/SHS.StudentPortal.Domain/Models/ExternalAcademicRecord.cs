
using System.ComponentModel.DataAnnotations.Schema;

namespace SHS.StudentPortal.Domain.Models
{
    public class ExternalAcademicRecord : BaseModel<long>, IAuditable
    {
        public string Rating { get; set; }

        public string SubjectName { get; set; }

        public string Semester { get; set; }

        public string AcademicYear { get; set; }

        public Guid StudentId { get; set; }

        [NotMapped]
        public StudentInfo Student { get; set; }

        #region > IAuditable Properties
        public Guid? CreatedById { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? ModifiedById { get; set; }
        public DateTime? ModifiedDate { get; set; } 
        #endregion

        public ExternalAcademicRecord Create(string rating,
            string subjectName,
            string semester,
            string academicYear,
            Guid studentId,
            Guid createdById)
        {
            Rating = rating;
            SubjectName = subjectName;
            Semester = semester;
            AcademicYear = academicYear;
            StudentId = studentId;

            CreatedById = createdById;
            CreatedDate = DateTime.UtcNow;

            return this;
        }
    }
}

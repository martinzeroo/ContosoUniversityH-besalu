using System.ComponentModel.DataAnnotations;

namespace ContosoUniversityHõbesalu.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string LastName { get; set; }
        public string FirstMidName { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public ICollection<Enrollment>? Enrollments { get; set; }

        [Display(Name = "Full Name")]
        public string FullName
        {
            get
            { return LastName + ", " + FirstMidName; }
        }

    }
}


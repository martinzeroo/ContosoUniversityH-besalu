using System.ComponentModel.DataAnnotations;

namespace ContosoUniversityHõbesalu.Models
{
    public class CourseAssignment
    {
        [Key]
        public int Id { get; set; }
        public int InstructorID { get; set; }
        public int CourseID { get; set; }
        public Instructor Instructor { get; set; }
        public Course Course { get; set; }

    }
}

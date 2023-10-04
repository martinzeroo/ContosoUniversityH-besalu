namespace ContosoUniversityHõbesalu.Models
{
    public class CourseViewModel
    {
        public Course course { get; set; }
        public IEnumerable<Instructor>? assignedInstructors { get; set; }
        public IEnumerable<Student>? assignedStudents { get; set; }

    }
}

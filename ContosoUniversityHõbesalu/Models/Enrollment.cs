namespace ContosoUniversityHõbesalu.Models
{

    public enum Grade
    {
        A, B, C, D, F
    }
    public class Enrollment
    {
        public int EnrollmentId { get; set; }

        public int CourseID { get; set; }

        public int StudentID { get; set; }

        public Grade? Grade { get; set; }
        
        public Student Student { get; set; }
    }
}

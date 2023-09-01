namespace ContosoUniversityHõbesalu.Models
{
    public class Student
    {
        public int Id { get; set; } 
        public string LastName { get; set; }
        public string FirstMidName { get; set; }
        public  DateTime MyProperty { get; set; }
        public ICollection<Enrollment> EnrollmentsDate { get; set; }
    }
}

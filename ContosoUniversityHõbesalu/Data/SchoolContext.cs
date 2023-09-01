using ContosoUniversityHõbesalu.Models;
using Microsoft.EntityFrameworkCore;


namespace ContosoUniversityHõbesalu.Data
{
    public class SchoolContext : DbContext
    {
        public SchoolContext(DbContextOptions<SchoolContext> options) : base(options)
        {

        }  

        public DbSet<Course> Courses { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; } 
        public DbSet<Student> Students { get; set; }
    }
}

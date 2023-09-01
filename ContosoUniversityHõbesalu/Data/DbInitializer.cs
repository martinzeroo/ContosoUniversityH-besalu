using ContosoUniversityHõbesalu.Models;

namespace ContosoUniversityHõbesalu.Data
{
    public static class DbInitializer
    {
        public static void Initialize(SchoolContext context)
        {
            context.Database.EnsureCreated();

            if (context.Students.Any() )
            {
                return;
            }

            var students = new Student[]
            {
                new Student() {FirstMidName="kaarel-Martin", LastName="Noole", EnrollmentDate = DateTime.Parse("2021-09-01") },

                new Student() {FirstMidName="Palmi", LastName="Lahe", EnrollmentDate = DateTime.Parse("2021-09-01") },

                new Student() {FirstMidName="Martin", LastName="Hõbesalu", EnrollmentDate = DateTime.Parse("2021-09-01") },

                new Student() {FirstMidName="Hannes", LastName="Malter", EnrollmentDate = DateTime.Parse("2021-09-01") },

                new Student() {FirstMidName="Karl", LastName="Umberto", EnrollmentDate = DateTime.Parse("2021-09-01") },

                new Student() {FirstMidName="Mihkel", LastName="Hain", EnrollmentDate = DateTime.Parse("2021-09-01") },

                new Student() {FirstMidName="Kristjan Georg", LastName="Kessel", EnrollmentDate = DateTime.Parse("2021-09-01") },
            };
            context.Students.AddRange(students);
            foreach(Student s in students)
            {
                context.Students.Add(s);
            }
            context.SaveChanges();
            
        }
    }
}

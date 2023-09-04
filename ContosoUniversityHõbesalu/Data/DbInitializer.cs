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
            var courses = new Course[]
            {
                new Course() {CourseId =1050, Title="Programmeerimine",Credits = 160},
            new Course() { CourseId = 8888, Title = "Keemia", Credits = 160 },
                new Course() {CourseId =1111, Title="Matemaatika",Credits = 160},
                new Course() {CourseId =7777, Title="Testimine",Credits = 160},
                new Course() {CourseId =6666, Title="Riigikaitse",Credits = 160}
            };

            foreach (Course c in courses)
            {
                context.Courses.Add(c);
            }
            context.SaveChanges();

            var enrollments = new Enrollment[]
            {
                new Enrollment{StudentId=1,CourseId=1050,Grade=Grade.A},
                new Enrollment{StudentId=2,CourseId=8888,Grade=Grade.B},
                new Enrollment{StudentId=3,CourseId=1111,Grade=Grade.C},
                new Enrollment{StudentId=4,CourseId=7777,Grade=Grade.D},
                new Enrollment{StudentId=5,CourseId=6666,Grade=Grade.F},
                new Enrollment{StudentId=2,CourseId=1050,Grade=Grade.A},
                new Enrollment{StudentId=3,CourseId=8888,Grade=Grade.B},
                new Enrollment{StudentId=4,CourseId=1111,Grade=Grade.C},
                new Enrollment{StudentId=1,CourseId=7777,Grade=Grade.D},
                new Enrollment{StudentId=3,CourseId=6666,Grade=Grade.F},
                new Enrollment{StudentId=2,CourseId=1050,Grade=Grade.A},
                new Enrollment{StudentId=1,CourseId=8888,Grade=Grade.B},
                new Enrollment{StudentId=4,CourseId=1111,Grade=Grade.C},
                new Enrollment{StudentId=3,CourseId=7777,Grade=Grade.D},
                new Enrollment{StudentId=1,CourseId=6666,Grade=Grade.F}
            };

            foreach(Enrollment E in enrollments)
            {
                context.Enrollments.Add(E);
            }
            context.SaveChanges();
        }
    }
}

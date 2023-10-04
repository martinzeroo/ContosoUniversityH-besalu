using ContosoUniversityHõbesalu.Data;
using ContosoUniversityHõbesalu.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ContosoUniversityHõbesalu.Controllers
{
    public class CoursesController : Controller
    {
        private readonly SchoolContext _context;

        public CoursesController(SchoolContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // Retrieve a list of courses including department information
            var courses = await _context.Courses
                .Include(c => c.Department)
                .ToListAsync();

            // Create a list of CourseViewModel objects to hold the data for each course
            var courseViewModels = new List<CourseViewModel>();

            // Populate each CourseViewModel
            foreach (var course in courses)
            {
                var courseViewModel = new CourseViewModel
                {
                    course = course,
                    assignedInstructors = await _context.CourseAssignments
                        .Where(ca => ca.CourseID == course.CourseID)
                        .Select(ca => ca.Instructor)
                        .ToListAsync(),
                    assignedStudents = await _context.Enrollments
                        .Where(ca => ca.CourseID == course.CourseID)
                        .Select(ca => ca.Student)
                        .ToListAsync(),
                };

                courseViewModels.Add(courseViewModel);
            }

            // Pass the list of CourseViewModels to the view
            return View(courseViewModels);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Courses == null)
            {
                return NotFound();
            }

            var course = await _context.Courses
                .Include(e => e.Department)
                .FirstOrDefaultAsync(m => m.CourseID == id);
            if (course == null)
            {
                return NotFound();
            }

            var courseViewModel = new CourseViewModel
            {
                course = course,
                assignedInstructors = await _context.CourseAssignments
                .Where(ca => ca.CourseID == course.CourseID)
                .Select(ca => ca.Instructor)
                .ToListAsync(),
                assignedStudents = await _context.Enrollments
                .Where(ca => ca.CourseID == course.CourseID)
                .Select(ca => ca.Student)
                .ToListAsync(),

            };

            return View(courseViewModel);
        }

        //get create
        public IActionResult Create()
        {
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "DepartmentId", "Name");
            return View();
        }

        //post create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Credits,DepartmentId")] Course course)
        {

            ModelState.Remove("Department");
            ModelState.Remove("Enrollments");
            ModelState.Remove("CourseAssignments");

            if (ModelState.IsValid)
            {
                _context.Add(course);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["DepartmentId"] = new SelectList(_context.Departments, "DepartmentId", "Name", course.DepartmentId);

            return View(course);

        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var course = await _context.Courses
                .Include(c => c.Department)
                .FirstOrDefaultAsync(s => s.CourseID == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int courseId)
        {
            Course course = await _context.Courses
                .SingleAsync(c => c.CourseID == courseId);

            //remove any associated course assignments
            var assignments = await _context.CourseAssignments
            .Where(ca => ca.CourseID == courseId)
            .ToListAsync();

            foreach (var assignment in assignments)
            {
                _context.CourseAssignments.Remove(assignment);
            }

            //remove enrollments
            var enrollments = await _context.Enrollments
                .Where(e => e.CourseID == courseId)
                .ToListAsync();

            foreach (var enrollment in enrollments)
            {
                _context.Enrollments.Remove(enrollment);
            }

            await _context.SaveChangesAsync();


            _context.Courses.Remove(course);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var course = await _context.Courses
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.CourseID == id);
            if (course == null)
            {
                return NotFound();
            }

            ViewData["DepartmentId"] = new SelectList(_context.Departments, "DepartmentId", "Name");
            ViewData["Instructors"] = await _context.Instructors.ToListAsync();

            return View(course);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CourseId,Title,Credits,DepartmentId")] Course course, string[] selectedInstructors)
        {
            if (id != course.CourseID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(course);
                    await _context.SaveChangesAsync();

                    // Assuming selectedInstructors is a list of selected instructor IDs
                    var selectedInstructorIds = selectedInstructors.Select(id => int.Parse(id)).ToList();
                    var courseId = id;

                    // Get all course assignments for the current course
                    var existingAssignments = await _context.CourseAssignments
                        .Where(ca => ca.CourseID == courseId)
                        .ToListAsync();

                    // Remove course assignments for instructors who are not selected
                    foreach (var assignment in existingAssignments.ToList())
                    {
                        if (!selectedInstructorIds.Contains(assignment.InstructorID))
                        {
                            _context.CourseAssignments.Remove(assignment);
                            await _context.SaveChangesAsync();
                        }
                    }

                    // Add course assignments for selected instructors if they don't already have one
                    foreach (var selectedId in selectedInstructorIds)
                    {
                        if (!existingAssignments.Any(ca => ca.InstructorID == selectedId))
                        {
                            var assignment = new CourseAssignment
                            {
                                CourseID = courseId,
                                InstructorID = selectedId
                            };

                            _context.CourseAssignments.Add(assignment);
                            await _context.SaveChangesAsync();
                        }
                    }

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(course.CourseID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            // If ModelState is not valid, redisplay the form with validation errors
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "DepartmentId", "Name", course.DepartmentId);
            return View(course);
        }

        private bool CourseExists(int id)
        {
            return _context.Courses.Any(e => e.CourseID == id);
        }

    }

}

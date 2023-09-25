using ContosoUniversityHõbesalu.Data;
using ContosoUniversityHõbesalu.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

namespace ContosoUniversityHõbesalu.Controllers
{
    public class DepartmentsController : Controller
    {
        private readonly SchoolContext _context;
        public DepartmentsController(SchoolContext context)
        {
            _context = context;
        }

        //get Index
        public async Task<IActionResult> Index()
        {
            var schoolcontext = _context.Departments.Include(d => d.Administrator);
            return View(await schoolcontext.ToListAsync());
        }
        //get Details
        public async Task<IActionResult> Details(int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }
            string query = "SELECT * FROM Department WHERE DepartmentID = {0}";
            var department = await _context.Departments
                .FromSqlRaw(query, Id)
                .Include(d => d.Administrator)
                .AsNoTracking()
                .FirstOrDefaultAsync();
            if (department == null)
            {
                return NotFound();
            }
            return View(department);
        }

        //get create

        public async Task<IActionResult> Create()
        {
            ViewData["InstructorID"] = new SelectList(_context.Instructors, "ID", "FullName");
            return View();
        }

        //post Create
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create([Bind("DepartmentID,Name,Budget,StartDate,RowVersion,InstructorID")] Department department)
        {
            if (ModelState.IsValid)
            {
                _context.Add(department);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["InstructorID"] = new SelectList(_context.Instructors, "ID", "FullName", department.InstructorID);
            return View(department);
        }

        //get edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var depratment = await _context.Departments
                .Include(i => i.Administrator)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.DepartmentID == id);
            if (depratment == null)
            {
                return NotFound();
            }
            ViewData["InstructorID"] = new SelectList(_context.Instructors, "ID", "FullName", depratment.InstructorID);
            return View(depratment);
        }
        //post Edit
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Edit(int? id, byte[] rowversion)
        {
            if (id == null)
            {
                return NotFound();
            }
            var departmentToUpdate = await _context.Departments
               .Include(i => i.Administrator)
               .FirstOrDefaultAsync(m => m.DepartmentID == id);

            if (departmentToUpdate == null)
            {
                Department DeletedDepartment = new Department();
                await TryUpdateModelAsync(DeletedDepartment);
                ModelState.AddModelError(string.Empty, "Unable To Save Changes, The Department was deleted by another user");
                ViewData["InstructorID"] = new SelectList(_context.Instructors, "ID", "FullName", DeletedDepartment.InstructorID);
                return View(DeletedDepartment);
            }
            _context.Entry(departmentToUpdate).Property("RowVersion").OriginalValue = rowversion;

            if (await TryUpdateModelAsync<Department>(departmentToUpdate, "", s => s.Name, s => s.StartDate, s => s.Budget, s => s.InstructorID))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    var exceptionEntry = ex.Entries.Single();
                    var clientValues = (Department)exceptionEntry.Entity;
                    var dataBaseEntry = exceptionEntry.GetDatabaseValues();

                    if (dataBaseEntry != null)
                    {
                        ModelState.AddModelError(string.Empty, "Unable to save Changes. The Deparment has been deleted by another user");
                    }
                }
            }
        }
    }
}

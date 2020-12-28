using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Assign1.Data;
using Assign1.Models;
using Assign1.Models.ViewModels;
using System.Diagnostics;

namespace Assign1.Controllers
{
    public class StudentsController : Controller
    {
        private readonly SchoolCommunityContext _context;

        public StudentsController(SchoolCommunityContext context)
        {
            _context = context;
        }

        // GET: Students
        public async Task<IActionResult> Index(string ID)
        {

            CommunityViewModel communityViewModel = new CommunityViewModel();
            var viewModel = communityViewModel;
            viewModel.Students = await _context.Students // promise that we will return a list/non-generic collection when the time.
                                    .Include(i => i.CommunityMembership) // self explanatory.
                                    .ThenInclude(i => i.Community) // self explanatory.
                                    .AsNoTracking() // disable tracking because we want a read-only table.
                                    .ToListAsync(); // convert communities to a List after finishing our queries, otherwise we're stuck with a IQueryable type
            if (ID != null)
            {
                // ViewData["CommunityID"] = ID;
                // Grab the community equal to the id selected having only one commmunity. If there's more it doesn't make any sense because
                // you should not have more than one community with the same ID. Then grabs the communities membership property/data member.
                viewModel.CommunityMemberships = viewModel.Students.Where(x => x.ID == int.Parse(ID)).Single().CommunityMembership;
            }
            return View(viewModel);
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .FirstOrDefaultAsync(m => m.ID == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,LastName,FirstName,EnrollmentDate")] Student student)
        {
            if (ModelState.IsValid)
            {
                _context.Add(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        public async Task<IActionResult> EditMemberships(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }


            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            StudentViewModel viewModel = new StudentViewModel();
            viewModel.CommunityMemberships = await _context.CommunityMemberships.Where(x => x.StudentID == id).OrderByDescending(x => x.Community.Title).ToListAsync();
            viewModel.Communities = await _context.Communities.ToListAsync();
            viewModel.Student = student;

            return View(viewModel);
        }
        
        [HttpGet("Students/AddMembership")]
        public async Task<IActionResult> AddMembership( int? studentId,  string communityId)
        {
            
            if (studentId == null || communityId == null)
            {
                return NotFound();
            }

            Student student = await _context.Students.FindAsync(studentId);
            
            CommunityMembership newMembership = new CommunityMembership {StudentID = (int)studentId, CommunityID = communityId };
            _context.Add(newMembership);
            await _context.SaveChangesAsync();
            
            return RedirectToAction(nameof(EditMemberships), new { id = studentId });
        }

        [HttpGet("Students/RemoveMembership")]
        public async Task<IActionResult> RemoveMembership(int? studentId, string communityId)
        {
            
            if (studentId == null || communityId == null)
            {
                return NotFound();
            }

            CommunityMembership communityMembership = await _context.CommunityMemberships.FindAsync(studentId,communityId);
            _context.CommunityMemberships.Remove(communityMembership);
            await _context.SaveChangesAsync();
            
            return RedirectToAction(nameof(EditMemberships), new { id = studentId });
        }
        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,LastName,FirstName,EnrollmentDate")] Student student)
        {
            if (id != student.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .FirstOrDefaultAsync(m => m.ID == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Students.FindAsync(id);
            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.ID == id);
        }
    }
}

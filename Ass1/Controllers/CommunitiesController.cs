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
using SQLitePCL;
using System.Dynamic;

namespace Assignment1.Controllers
{
    public class CommunitiesController : Controller
    {
        private readonly SchoolCommunityContext _context;

        public CommunitiesController(SchoolCommunityContext context)
        {
            _context = context;
        }

        // GET: Communities
        public async Task<IActionResult> Index(string ID)
        {
            // grab our created CommunityView model so that we can merge two models together, that we can send to our view.W
            CommunityViewModel communityViewModel = new CommunityViewModel();
            var viewModel = communityViewModel;
            
            viewModel.Communities = await _context.Communities // promise that we will return a list/non-generic collection when the time.
                                    .Include(i => i.CommunityMemberships) // self explanatory.
                                    .ThenInclude(i => i.Student) // self explanatory.
                                    .AsNoTracking() // disable tracking because we want a read-only table.
                                    .OrderBy(i => i.Title) // self explanatory.
                                    .ToListAsync(); // convert communities to a List after finishing our queries, otherwise we're stuck with a IQueryable type
            if (ID != null)
            {
                // ViewData["CommunityID"] = ID;
                // Grab the community equal to the id selected having only one commmunity. If there's more it doesn't make any sense because
                // you should not have more than one community with the same ID. Then grabs the communities membership property/data member.
                viewModel.CommunityMemberships = viewModel.Communities.Where(x => x.ID == ID).Single().CommunityMemberships;
            }

            return View(viewModel);
        }

        // GET: Communities/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Community community;
            community = await _context.Communities
                .FirstOrDefaultAsync(m => m.ID == id);

            if (community == null)
            {
                return NotFound();
            }
            
            return View(community);
        }

        // GET: Communities/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Communities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Title,Budget")] Community community)
        {
            if (ModelState.IsValid)
            {
                _context.Add(community);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(community);
        }

        // GET: Communities/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var community = await _context.Communities.FindAsync(id);
            if (community == null)
            {
                return NotFound();
            }
            return View(community);
        }

        // POST: Communities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ID,Title,Budget")] Community community)
        {
            if (id != community.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(community);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommunityExists(community.ID))
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
            return View(community);
        }

        // GET: Communities/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            AdvertisementViewModel viewModel = new AdvertisementViewModel();
            if (id != null)
            {
                viewModel.Community = await _context.Communities.FindAsync(id);
                if (viewModel.Community == null)
                {
                    return NotFound();
                }
                viewModel.Advertisements = _context.Advertisements.Where(x => x.CommunityID == id).ToList();
            }
            else
            {
                return NotFound();
            }

            return View(viewModel);
        }

        // POST: Communities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var community = await _context.Communities.FindAsync(id);
            _context.Communities.Remove(community);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CommunityExists(string id)
        {
            return _context.Communities.Any(e => e.ID == id);
        }
    }
}

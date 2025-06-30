using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Gym.Models;

namespace Gym.Controllers
{
    /// <summary>
    /// Controller responsible for managing gym members, including viewing, creating, editing, and deleting members.
    /// </summary>
    public class MemberController : Controller
    {
        private readonly GymContext _context;

        /// <summary>
        /// Initializes the MemberController with a database context.
        /// </summary>
        /// <param name="context">GymContext instance for accessing the database.</param>
        public MemberController(GymContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Displays a list of all members with optional search by name.
        /// </summary>
        /// <param name="searchString">Optional search string for filtering by full name.</param>
        /// <returns>Index view with filtered or complete list of members.</returns>
        public async Task<IActionResult> Index(string searchString)
        {
            var members = _context.Members
                .Include(m => m.Trainer)
                .Include(m => m.MemberClasses)
                    .ThenInclude(mc => mc.GymClass)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                members = members.Where(m => m.FullName.Contains(searchString));
            }

            return View(await members.ToListAsync());
        }

        /// <summary>
        /// Displays the details of a specific member.
        /// </summary>
        /// <param name="id">The ID of the member.</param>
        /// <returns>Details view of the member or NotFound if not found.</returns>
        public async Task<IActionResult> Details(int id)
        {
            var member = await _context.Members
                .Include(m => m.Trainer)
                .Include(m => m.MemberClasses)
                    .ThenInclude(mc => mc.GymClass)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (member == null) return NotFound();
            return View(member);
        }

        /// <summary>
        /// Returns the Create view with trainer and class selection options.
        /// </summary>
        public async Task<IActionResult> Create()
        {
            ViewBag.Trainers = await _context.Trainers.ToListAsync();
            ViewBag.GymClasses = await _context.GymClasses.ToListAsync();
            return View();
        }

        /// <summary>
        /// Handles creation of a new member with selected trainer and classes.
        /// </summary>
        /// <param name="member">The member object.</param>
        /// <param name="trainerId">Trainer ID assigned to the member.</param>
        /// <param name="selectedClasses">List of selected class IDs.</param>
        /// <returns>Redirects to Index or returns the Create view on failure.</returns>
        [HttpPost]
        public async Task<IActionResult> Create(Member member, int? trainerId, List<int> selectedClasses)
        {
            if (ModelState.IsValid)
            {
                member.TrainerId = trainerId;

                if (selectedClasses != null && selectedClasses.Any())
                {
                    member.MemberClasses = selectedClasses.Select(cid => new MemberClass
                    {
                        GymClassId = cid
                    }).ToList();
                }

                _context.Members.Add(member);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Trainers = await _context.Trainers.ToListAsync();
            ViewBag.GymClasses = await _context.GymClasses.ToListAsync();
            return View(member);
        }

        /// <summary>
        /// Loads the Edit view for a specific member.
        /// </summary>
        /// <param name="id">The ID of the member to edit.</param>
        /// <returns>Edit view or NotFound if member not found.</returns>
        public async Task<IActionResult> Edit(int id)
        {
            var member = await _context.Members
                .Include(m => m.MemberClasses)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (member == null) return NotFound();

            ViewBag.Trainers = await _context.Trainers.ToListAsync();
            ViewBag.GymClasses = await _context.GymClasses.ToListAsync();
            ViewBag.SelectedClasses = member.MemberClasses.Select(mc => mc.GymClassId).ToList();

            return View(member);
        }

        /// <summary>
        /// Updates the details of a member.
        /// </summary>
        /// <param name="id">The ID of the member to update.</param>
        /// <param name="member">The updated member object.</param>
        /// <param name="trainerId">The ID of the assigned trainer.</param>
        /// <param name="selectedClasses">List of selected gym class IDs.</param>
        /// <returns>Redirects to Index or reloads Edit view on error.</returns>
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Member member, int? trainerId, List<int> selectedClasses)
        {
            var existingMember = await _context.Members
                .Include(m => m.MemberClasses)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (existingMember == null) return NotFound();

            if (ModelState.IsValid)
            {
                existingMember.FullName = member.FullName;
                existingMember.Email = member.Email;
                existingMember.PhoneNumber = member.PhoneNumber;
                existingMember.TrainerId = trainerId;

                existingMember.MemberClasses.Clear();
                if (selectedClasses != null)
                {
                    existingMember.MemberClasses = selectedClasses.Select(cid => new MemberClass
                    {
                        MemberId = id,
                        GymClassId = cid
                    }).ToList();
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Trainers = await _context.Trainers.ToListAsync();
            ViewBag.GymClasses = await _context.GymClasses.ToListAsync();
            ViewBag.SelectedClasses = selectedClasses;
            return View(member);
        }

        /// <summary>
        /// Deletes a member by ID.
        /// </summary>
        /// <param name="id">The ID of the member to delete.</param>
        /// <returns>Redirects to Index or NotFound if member doesn't exist.</returns>
        public async Task<IActionResult> Delete(int id)
        {
            var member = await _context.Members.FindAsync(id);
            if (member == null) return NotFound();

            _context.Members.Remove(member);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}

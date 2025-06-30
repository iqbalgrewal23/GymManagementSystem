using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Gym.Models;

namespace Gym.Controllers
{
    /// <summary>
    /// Controller for managing gym classes in the system.
    /// </summary>
    public class GymClassController : Controller
    {
        private readonly GymContext _context;

        public GymClassController(GymContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Displays a list of all gym classes.
        /// </summary>
        /// <returns>A view with a list of <see cref="GymClass"/> items.</returns>
        public async Task<IActionResult> Index()
        {
            return View(await _context.GymClasses.ToListAsync());
        }

        /// <summary>
        /// Displays details of a specific gym class, including enrolled members.
        /// </summary>
        /// <param name="id">The ID of the gym class to display.</param>
        /// <returns>A view with the gym class details or NotFound if not found.</returns>
        public async Task<IActionResult> Details(int id)
        {
            var gymClass = await _context.GymClasses
                .Include(gc => gc.MemberClasses)
                    .ThenInclude(mc => mc.Member)
                .FirstOrDefaultAsync(gc => gc.Id == id);

            if (gymClass == null) return NotFound();
            return View(gymClass);
        }

        /// <summary>
        /// Displays the form to create a new gym class.
        /// </summary>
        /// <returns>A view for creating a gym class.</returns>
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Handles POST request to create a new gym class.
        /// </summary>
        /// <param name="gymClass">The gym class object submitted from the form.</param>
        /// <returns>Redirects to Index on success or returns to form on failure.</returns>
        [HttpPost]
        public async Task<IActionResult> Create(GymClass gymClass)
        {
            if (ModelState.IsValid)
            {
                _context.GymClasses.Add(gymClass);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(gymClass);
        }

        /// <summary>
        /// Displays the edit form for a gym class.
        /// </summary>
        /// <param name="id">The ID of the gym class to edit.</param>
        /// <returns>A view for editing the gym class or NotFound if not found.</returns>
        public async Task<IActionResult> Edit(int id)
        {
            var gymClass = await _context.GymClasses.FindAsync(id);
            if (gymClass == null) return NotFound();
            return View(gymClass);
        }

        /// <summary>
        /// Handles POST request to update a gym class.
        /// </summary>
        /// <param name="id">The ID of the gym class to update.</param>
        /// <param name="gymClass">The updated gym class object.</param>
        /// <returns>Redirects to Index on success or returns to form on failure.</returns>
        [HttpPost]
        public async Task<IActionResult> Edit(int id, GymClass gymClass)
        {
            if (id != gymClass.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Entry(gymClass).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(gymClass);
        }

        /// <summary>
        /// Deletes a specific gym class.
        /// </summary>
        /// <param name="id">The ID of the gym class to delete.</param>
        /// <returns>Redirects to Index after deletion or NotFound if not found.</returns>
        public async Task<IActionResult> Delete(int id)
        {
            var gymClass = await _context.GymClasses.FindAsync(id);
            if (gymClass == null) return NotFound();

            _context.GymClasses.Remove(gymClass);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}

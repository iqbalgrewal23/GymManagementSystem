using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Gym.Models;

namespace Gym.Controllers
{
    /// <summary>
    /// Controller for managing Trainer-related operations such as listing, creating, editing, and deleting trainers.
    /// </summary>
    public class TrainerController : Controller
    {
        private readonly GymContext _context;

        /// <summary>
        /// Initializes the TrainerController with the database context.
        /// </summary>
        /// <param name="context">GymContext instance for database access.</param>
        public TrainerController(GymContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Displays a list of all trainers. Supports optional search by name.
        /// </summary>
        /// <param name="searchString">Optional search string for filtering trainer names.</param>
        /// <returns>Index view with list of trainers.</returns>
        public async Task<IActionResult> Index(string searchString)
        {
            var trainers = _context.Trainers.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                trainers = trainers.Where(t => t.Name.Contains(searchString));
            }

            return View(await trainers.ToListAsync());
        }

        /// <summary>
        /// Shows detailed information about a specific trainer, including their assigned members.
        /// </summary>
        /// <param name="id">Trainer ID.</param>
        /// <returns>Details view of the trainer or NotFound if not found.</returns>
        public async Task<IActionResult> Details(int id)
        {
            var trainer = await _context.Trainers
                .Include(t => t.Members)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (trainer == null) return NotFound();
            return View(trainer);
        }

        /// <summary>
        /// Loads the Create view for adding a new trainer.
        /// </summary>
        /// <returns>Create view.</returns>
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Handles submission of new trainer data to create a trainer record.
        /// </summary>
        /// <param name="trainer">Trainer object submitted via form.</param>
        /// <returns>Redirects to Index on success or reloads Create view on failure.</returns>
        [HttpPost]
        public async Task<IActionResult> Create(Trainer trainer)
        {
            if (ModelState.IsValid)
            {
                _context.Trainers.Add(trainer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(trainer);
        }

        /// <summary>
        /// Loads the Edit view for a specific trainer.
        /// </summary>
        /// <param name="id">Trainer ID.</param>
        /// <returns>Edit view or NotFound if trainer does not exist.</returns>
        public async Task<IActionResult> Edit(int id)
        {
            var trainer = await _context.Trainers.FindAsync(id);
            if (trainer == null) return NotFound();
            return View(trainer);
        }

        /// <summary>
        /// Handles submission of edited trainer data.
        /// </summary>
        /// <param name="id">Trainer ID.</param>
        /// <param name="trainer">Trainer object with updated values.</param>
        /// <returns>Redirects to Index on success or reloads Edit view on failure.</returns>
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Trainer trainer)
        {
            if (id != trainer.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Entry(trainer).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(trainer);
        }

        /// <summary>
        /// Deletes a trainer by ID.
        /// </summary>
        /// <param name="id">Trainer ID.</param>
        /// <returns>Redirects to Index or returns NotFound if trainer does not exist.</returns>
        public async Task<IActionResult> Delete(int id)
        {
            var trainer = await _context.Trainers.FindAsync(id);
            if (trainer == null) return NotFound();

            _context.Trainers.Remove(trainer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}

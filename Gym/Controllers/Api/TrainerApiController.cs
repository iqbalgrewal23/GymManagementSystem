using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Gym.Models;
using Gym.DTOs;

namespace Gym.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class TrainerApiController : ControllerBase
    {
        private readonly GymContext _context;

        public TrainerApiController(GymContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves a list of all trainers.
        /// </summary>
        /// <returns>
        /// A list of <see cref="TrainerDTO"/> containing trainer details.
        /// </returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TrainerDTO>>> GetTrainers()
        {
            return await _context.Trainers
                .Select(t => new TrainerDTO
                {
                    Id = t.Id,
                    Name = t.Name,
                    PhoneNumber = t.PhoneNumber
                })
                .ToListAsync();
        }

        /// <summary>
        /// Adds a new trainer to the database.
        /// </summary>
        /// <param name="dto">The trainer data to add.</param>
        /// <returns>
        /// The created <see cref="Trainer"/> object.
        /// </returns>
        [HttpPost]
        public async Task<ActionResult<Trainer>> AddTrainer(TrainerDTO dto)
        {
            var trainer = new Trainer
            {
                Name = dto.Name,
                PhoneNumber = dto.PhoneNumber,
                Email = dto.Email  
            };

            _context.Trainers.Add(trainer);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetTrainers), new { id = trainer.Id }, trainer);
        }

        /// <summary>
        /// Updates an existing trainer's details.
        /// </summary>
        /// <param name="id">The ID of the trainer to update.</param>
        /// <param name="dto">The updated trainer information.</param>
        /// <returns>No content on success.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTrainer(int id, TrainerDTO dto)
        {
            var trainer = await _context.Trainers.FindAsync(id);
            if (trainer == null) return NotFound();

            trainer.Name = dto.Name;
            trainer.PhoneNumber = dto.PhoneNumber;
            trainer.Email = dto.Email;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Deletes a trainer by ID.
        /// </summary>
        /// <param name="id">The ID of the trainer to delete.</param>
        /// <returns>No content if deletion succeeds.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTrainer(int id)
        {
            var trainer = await _context.Trainers.FindAsync(id);
            if (trainer == null) return NotFound();

            _context.Trainers.Remove(trainer);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}

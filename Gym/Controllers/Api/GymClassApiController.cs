using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Gym.Models;
using Gym.DTOs;

namespace Gym.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class GymClassApiController : ControllerBase
    {
        private readonly GymContext _context;

        public GymClassApiController(GymContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves a list of all gym classes.
        /// </summary>
        /// <example>
        /// GET: api/GymClassApi -> Returns a list of all gym class records.
        /// </example>
        /// <returns>
        /// A list of <see cref="GymClassDTO"/> objects containing class ID and name.
        /// </returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GymClassDTO>>> GetGymClasses()
        {
            return await _context.GymClasses
                .Select(gc => new GymClassDTO
                {
                    Id = gc.Id,
                    ClassName = gc.ClassName
                })
                .ToListAsync();
        }

        /// <summary>
        /// Adds a new gym class to the system.
        /// </summary>
        /// <param name="dto">The data transfer object containing the new class details.</param>
        /// <example>
        /// POST: api/GymClassApi with body { "ClassName": "Yoga" }
        /// </example>
        /// <returns>
        /// The created <see cref="GymClass"/> object with generated ID.
        /// </returns>
        [HttpPost]
        public async Task<ActionResult<GymClass>> AddGymClass(GymClassDTO dto)
        {
            var gymClass = new GymClass { ClassName = dto.ClassName };
            _context.GymClasses.Add(gymClass);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetGymClasses), new { id = gymClass.Id }, gymClass);
        }

        /// <summary>
        /// Updates an existing gym class by ID.
        /// </summary>
        /// <param name="id">The ID of the class to update.</param>
        /// <param name="dto">The updated class data.</param>
        /// <example>
        /// PUT: api/GymClassApi/1 with body { "ClassName": "Pilates" }
        /// </example>
        /// <returns>
        /// No content if successful; NotFound if the class doesn't exist.
        /// </returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGymClass(int id, GymClassDTO dto)
        {
            var gymClass = await _context.GymClasses.FindAsync(id);
            if (gymClass == null) return NotFound();
            gymClass.ClassName = dto.ClassName;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Deletes a gym class by ID.
        /// </summary>
        /// <param name="id">The ID of the class to delete.</param>
        /// <example>
        /// DELETE: api/GymClassApi/3
        /// </example>
        /// <returns>
        /// No content if successful; NotFound if the class doesn't exist.
        /// </returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGymClass(int id)
        {
            var gymClass = await _context.GymClasses.FindAsync(id);
            if (gymClass == null) return NotFound();
            _context.GymClasses.Remove(gymClass);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}

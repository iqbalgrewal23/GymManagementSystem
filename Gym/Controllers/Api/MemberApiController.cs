using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Gym.Models;
using Gym.DTOs;

namespace Gym.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class MemberApiController : ControllerBase
    {
        private readonly GymContext _context;

        public MemberApiController(GymContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves a list of all gym members along with their assigned trainer and enrolled class IDs.
        /// </summary>
        /// <returns>
        /// A list of <see cref="MemberDTO"/> objects containing member details.
        /// </returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDTO>>> GetMembers()
        {
            return await _context.Members
                .Include(m => m.MemberClasses)
                .Select(m => new MemberDTO
                {
                    Id = m.Id,
                    FullName = m.FullName,
                    TrainerId = m.TrainerId,
                    GymClassIds = m.MemberClasses.Select(mc => mc.GymClassId).ToList()
                })
                .ToListAsync();
        }

        /// <summary>
        /// Adds a new gym member with optional assigned trainer and class enrollments.
        /// </summary>
        /// <param name="dto">The data transfer object containing member information.</param>
        /// <returns>
        /// The created <see cref="Member"/> object.
        /// </returns>
        [HttpPost]
        public async Task<ActionResult<Member>> AddMember(MemberDTO dto)
        {
            var member = new Member
            {
                FullName = dto.FullName,
                TrainerId = dto.TrainerId
            };

            if (dto.GymClassIds != null)
            {
                member.MemberClasses = dto.GymClassIds
                    .Select(id => new MemberClass { GymClassId = id, Member = member })
                    .ToList();
            }

            _context.Members.Add(member);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetMembers), new { id = member.Id }, member);
        }

        /// <summary>
        /// Updates an existing member's information including assigned trainer and class enrollments.
        /// </summary>
        /// <param name="id">The ID of the member to update.</param>
        /// <param name="dto">The updated member data.</param>
        /// <returns>No content on successful update.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMember(int id, MemberDTO dto)
        {
            var member = await _context.Members
                .Include(m => m.MemberClasses)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (member == null) return NotFound();

            member.FullName = dto.FullName;
            member.TrainerId = dto.TrainerId;

            // update classes
            member.MemberClasses.Clear();
            if (dto.GymClassIds != null)
            {
                member.MemberClasses = dto.GymClassIds
                    .Select(cid => new MemberClass { MemberId = id, GymClassId = cid })
                    .ToList();
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Deletes a member by ID from the gym system.
        /// </summary>
        /// <param name="id">The ID of the member to delete.</param>
        /// <returns>No content if deletion is successful.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMember(int id)
        {
            var member = await _context.Members.FindAsync(id);
            if (member == null) return NotFound();
            _context.Members.Remove(member);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}

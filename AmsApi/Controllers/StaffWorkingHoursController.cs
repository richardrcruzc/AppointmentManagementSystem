using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AmsApi.Domain;
using AmsApi.Infraestructure;

namespace AmsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StaffWorkingHoursController : ControllerBase
    {
        private readonly AmsApiDbContext _context;

        public StaffWorkingHoursController(AmsApiDbContext context)
        {
            _context = context;
        }

        // GET: api/StaffWorkingHours
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StaffWorkingHour>>> GetStaffWorkingHours()
        {
            return await _context.StaffWorkingHours.ToListAsync();
        }

        // GET: api/StaffWorkingHours/5
        [HttpGet("{id}")]
        public async Task<ActionResult<StaffWorkingHour>> GetStaffWorkingHour(int id)
        {
            var staffWorkingHour = await _context.StaffWorkingHours.FindAsync(id);

            if (staffWorkingHour == null)
            {
                return NotFound();
            }

            return staffWorkingHour;
        }

        // PUT: api/StaffWorkingHours/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStaffWorkingHour(int id, StaffWorkingHour staffWorkingHour)
        {
            if (id != staffWorkingHour.Id)
            {
                return BadRequest();
            }

            _context.Entry(staffWorkingHour).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StaffWorkingHourExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/StaffWorkingHours
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<StaffWorkingHour>> PostStaffWorkingHour(StaffWorkingHour staffWorkingHour)
        {
            _context.StaffWorkingHours.Add(staffWorkingHour);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStaffWorkingHour", new { id = staffWorkingHour.Id }, staffWorkingHour);
        }

        // DELETE: api/StaffWorkingHours/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<StaffWorkingHour>> DeleteStaffWorkingHour(int id)
        {
            var staffWorkingHour = await _context.StaffWorkingHours.FindAsync(id);
            if (staffWorkingHour == null)
            {
                return NotFound();
            }

            _context.StaffWorkingHours.Remove(staffWorkingHour);
            await _context.SaveChangesAsync();

            return staffWorkingHour;
        }

        private bool StaffWorkingHourExists(int id)
        {
            return _context.StaffWorkingHours.Any(e => e.Id == id);
        }
    }
}

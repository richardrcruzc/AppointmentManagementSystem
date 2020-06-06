using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AmsApi.Domain;
using AmsApi.Infraestructure;
using Microsoft.AspNetCore.Authorization;
using AmsApi.Data;

namespace AmsApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ClosedDatesController : ControllerBase
    {
        private readonly AmsApiDbContext _context;

        public ClosedDatesController(AmsApiDbContext context)
        {
            _context = context;
        }

        // GET: api/ClosedDates
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<ClosedDate>>> GetClosedDates()
        //{
        //    return await _context.ClosedDates.ToListAsync();
        //}
        [HttpGet]
        public async Task<ActionResult<ApiResult<ClosedDate>>> GetClosedDates(
             int pageIndex = 0,
             int pageSize = 10,
             string sortColumn = null,
             string sortOrder = null,
             string filterColumn = null,
             string filterQuery = null)
        {
            return await ApiResult<ClosedDate>.CreateAsync(
                   _context.ClosedDates.AsNoTracking(),
                    pageIndex,
                    pageSize,
                    sortColumn,
                    sortOrder,
                    filterColumn,
                    filterQuery);
        }

        // GET: api/ClosedDates/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ClosedDate>> GetClosedDate(int id)
        {
            var closedDate = await _context.ClosedDates.FindAsync(id);

            if (closedDate == null)
            {
                return NotFound();
            }

            return closedDate;
        }

        // PUT: api/ClosedDates/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClosedDate(int id, ClosedDate closedDate)
        {
            if (id != closedDate.Id)
            {
                return BadRequest();
            }

            _context.Entry(closedDate).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClosedDateExists(id))
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

        // POST: api/ClosedDates
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<ClosedDate>> PostClosedDate(ClosedDate closedDate)
        {
            _context.ClosedDates.Add(closedDate);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetClosedDate", new { id = closedDate.Id }, closedDate);
        }

        // DELETE: api/ClosedDates/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ClosedDate>> DeleteClosedDate(int id)
        {
            var closedDate = await _context.ClosedDates.FindAsync(id);
            if (closedDate == null)
            {
                return NotFound();
            }

            _context.ClosedDates.Remove(closedDate);
            await _context.SaveChangesAsync();

            return closedDate;
        }

        private bool ClosedDateExists(int id)
        {
            return _context.ClosedDates.Any(e => e.Id == id);
        }
    }
}

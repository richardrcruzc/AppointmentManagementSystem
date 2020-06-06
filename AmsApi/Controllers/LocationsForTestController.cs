using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AmsApi.Data;
using AmsApi.Domain;
using AmsApi.Infraestructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace AmsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationsForTestController:ControllerBase
    {
        private readonly AmsApiDbContext _context;

        public LocationsForTestController(AmsApiDbContext context)
        {
            _context = context;
        }
        // GET: api/Cities/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Location>> GetLocation(int id)
        {
            var city = await _context.Locations.FindAsync(id);

            if (city == null)
            {
                return NotFound();
            }

            return city;
        }

    }
}

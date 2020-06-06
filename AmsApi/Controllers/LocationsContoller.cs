using AmsApi.Data;
using AmsApi.Domain;
using AmsApi.Models;
using AmsApi.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using AmsApi.Repository;

namespace AmsApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class LocationsController : ControllerBase
    {

        private readonly ILogger<LocationsController> _logger;

        private readonly IMapper _mapper;
        private readonly IRepository<Location> _s;
        private readonly IUserService _service;

        public LocationsController(IUserService service, IRepository<Location> s, IMapper mapper, ILogger<LocationsController> logger)
        {
            _logger = logger;
            _mapper = mapper;
            _service = service;
            _s = s;
        }
        [HttpPost]
        public async Task<ActionResult<Location>> PostLocation([FromBody]Location location)
        {        

            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                 

                await _s.CreateAsync(location);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error creating location", ex);
                return BadRequest($"Unable to create Location");
            }

            return CreatedAtAction("GetLocations", new { id = location.Id }, location);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutLocation(int id, [FromBody]Location location)
        {
            if (id != location.Id)
            {
                return BadRequest();
            }

            try
            {
                await _s.UpdateAsync(id, location);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError("Error creating location", ex);

                if (!LocationExists(id))
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

        [HttpGet]
        public async Task<ActionResult<ApiResult<LocationListModel>>> GetLocations(
                int pageIndex = 0,
                int pageSize = 10,
                string sortColumn = null,
                string sortOrder = null,
                string filterColumn = null,
                string filterQuery = null)
        {
            return await ApiResult<LocationListModel>.CreateAsync(
                    _s.GetAll().Select(l=> 
                    new LocationListModel 
                    { 
                        Id=l.Id,
                     City=l.City,
                      ContactName=l.ContactName,
                       Description=l.Description,
                        Phone=l.Phone,
                         TotalAppt=l.Appointments.Count(),
                          TotalClosed = l.ClosedDates.Count(),
                           TotalMsgs = l.NotificationMessages.Count()
                    }),
                    pageIndex,
                    pageSize,
                    sortColumn,
                    sortOrder,
                    filterColumn,
                    filterQuery);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LocationModel>> GetByIdAsync(int id)
        {
            var location = await _s.FindBy(i => i.Id == id)
                .Include(x => x.Appointments)
                .Include(x => x.NotificationMessages)
                .Include(x => x.ClosedDates).FirstOrDefaultAsync();

         

            


            var model = _mapper.Map<LocationModel>(location);
            return Ok(model);
          
        }
        //[HttpPost]
        //public async Task<IActionResult> PostAsync([FromBody] LocationModel s)
        //{
        //    try
        //    {
        //        if (!ModelState.IsValid)
        //        {
        //            return BadRequest(ModelState);
        //        }

        //        var l = _mapper.Map<Location>(s);

        //        await _s.CreateAsync(l);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest($"Unable to create Location");
        //    }

        //    return Ok(s);
        //}
        private bool LocationExists(int id)
        {
            return _s.GetAll().Any(e => e.Id == id);
        }

        [HttpPost]
        [Route("IsDupeField")]
        public bool IsDupeField(
            int locationId,
            string fieldName,
            string fieldValue)
        {
            // Standard approach(using strongly-typed LAMBA expressions)
            //switch (fieldName)
            //{
            //    case "name":
            //        return _context.Countries.Any(
            //            c => c.Name == fieldValue && c.Id != countryId);
            //    case "iso2":
            //        return _context.Countries.Any(
            //            c => c.ISO2 == fieldValue && c.Id != countryId);
            //    case "iso3":
            //        return _context.Countries.Any(
            //            c => c.ISO3 == fieldValue && c.Id != countryId);
            //    default:
            //        return false;
            //}

            // Dynamic approach (using System.Linq.Dynamic.Core)
            return (ApiResult<Location>.IsValidProperty(fieldName, true))
                ? _s.GetAll().Any(
                    String.Format("{0} == @0 && Id != @1", fieldName),
                    fieldValue,
                    locationId)
                : false;
        }
    }
}

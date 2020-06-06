using AmsApi.Data;
using AmsApi.Domain;
using AmsApi.Repository;
using AmsApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AmsApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ServicesController : ControllerBase
    {
        private IRepository<Service> _s;
        private readonly IUserService _service;

        public ServicesController(IUserService service, IRepository<Service> s)
        {
            _service = service;
            _s = s;
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<Service>> DeleteSC(int id)
        {
            var sc = await _s.GetById(id);
            if (sc == null)
            {
                return NotFound();
            }

            await _s.DeleteAsync(id);

            return sc;

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(int id, [FromBody] Service sc)
        {

            if (id != sc.Id)
            {
                return BadRequest();
            }
        
            try
            {
                await _s.UpdateAsync(id, sc);
            }
            catch (DbUpdateConcurrencyException)
            {
                var exist = await _s.GetById(id);
                if (exist == null)
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

      
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] Service s)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var newS = new Service { ActiveStatus=s.ActiveStatus, 
                    DurationHour = s.DurationHour, 
                    DurationMinute=s.DurationMinute, 
                    Photo=s.Photo,
                    Price=s.Price, 
                    ServiceCategory =  s.ServiceCategory,
                    ServiceDescription=s.ServiceDescription,
                    ServiceName =s.ServiceName
                };
                await _s.CreateAsync(newS);
            }
            catch (Exception ex)
            {
                return BadRequest($"Unable to create catagory");
            }

            return Ok(s);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Service>> GetByIdAsync(int id)
        {
            return await _s.GetById(id);
        }
     
        [HttpGet]
        public async Task<ActionResult<ApiResult<Service>>> GetAllServicesAsync(
         int pageIndex = 0,
         int pageSize = 10,
         string sortColumn = null,
         string sortOrder = null,
         string filterColumn = null,
         string filterQuery = null)
        {
            return await ApiResult<Service>.CreateAsync(
                   _s.GetAll().Include("ServiceCategory"),
                    pageIndex,
                    pageSize,
                    sortColumn,
                    sortOrder,
                    filterColumn,
                    filterQuery);
        }
        [HttpPost]
        [Route("IsDupeField")]
        public bool IsDupeField(
           int Id,
           string fieldName,
           string fieldValue)
        {

            // Dynamic approach (using System.Linq.Dynamic.Core)
            return (ApiResult<Service>.IsValidProperty(fieldName, true))
                ? _s.GetAll().Any(
                    String.Format("{0} == @0 && Id != @1", fieldName),
                    fieldValue,
                    Id)
                : false;
        }
    }
}

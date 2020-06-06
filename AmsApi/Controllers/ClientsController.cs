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
    public class ClientsController : ControllerBase
    {
        private IRepository<Client> _c; 
        public ClientsController(  IRepository<Client> s)
        {
            
           _c= s;
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<Client>> DeleteSC(int id)
        {
            var sc = await _c.GetById(id);
            if (sc == null)
            {
                return NotFound();
            }

            await _c.DeleteAsync(id);

            return sc;

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(int id, [FromBody] Client sc)
        {

            if (id != sc.Id)
            {
                return BadRequest();
            }

            try
            {
                await _c.UpdateAsync(id, sc);
            }
            catch (DbUpdateConcurrencyException)
            {
                var exist = await _c.GetById(id);
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
        public async Task<IActionResult> PostAsync([FromBody] Client s)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                } 
                await _c.CreateAsync(s);
            }
            catch (Exception ex)
            {
                return BadRequest($"Unable to create client");
            }

            return Ok(s);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Client>> GetByIdAsync(int id)
        {
            return await _c.GetById(id);
        }

        [HttpGet]
        public async Task<ActionResult<ApiResult<Client>>> GetAllServicesAsync(
         int pageIndex = 0,
         int pageSize = 10,
         string sortColumn = null,
         string sortOrder = null,
         string filterColumn = null,
         string filterQuery = null)
        {
            return await ApiResult<Client>.CreateAsync(
                   _c.GetAll(),
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
            return (ApiResult<Client>.IsValidProperty(fieldName, true))
                ? _c.GetAll().Any(
                    String.Format("{0} == @0 && Id != @1", fieldName),
                    fieldValue,
                    Id)
                : false;
        }
    }
}

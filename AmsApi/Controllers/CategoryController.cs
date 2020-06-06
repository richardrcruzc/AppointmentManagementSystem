using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Security.Claims;
using System.Threading.Tasks;
using AmsApi.Data;
using AmsApi.Domain;
using AmsApi.Infraestructure;
using AmsApi.Repository;
using AmsApi.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AmsApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly IMapper _mapper;
        private IRepository<ServiceCategory> _sc;
        private readonly IUserService _service;
        private readonly AmsApiDbContext _cntx;
        public CategoriesController(IUserService service, IRepository<ServiceCategory> sc, IMapper mapper, AmsApiDbContext cntx)
        {
            _cntx = cntx;
            _mapper = mapper;
            _service = service;
            _sc = sc;
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<ServiceCategory>> DeleteSC(int id)
        {            
                var sc = await _sc.GetById(id);
            if (sc == null)
            {
                return NotFound("Unable to delete Category");            
            }

            await _sc.DeleteAsync(id);

            return Ok(sc);
        
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(int id, [FromBody] ServiceCategory sc)
        {

            if (id != sc.Id)
            {
                return NotFound("Unable to update category");
            }
            await _sc.UpdateAsync(id, sc);
            try
            {
                await _sc.UpdateAsync(id, sc);
            }
            catch (DbUpdateConcurrencyException)
            {
                var exist = await _sc.GetById(id);
                if (exist==null)
                {
                    return NotFound("Unable to update category");
                }
                else
                {
                    throw;
                }
            }

            return Ok(sc);
        }

        [HttpPost] 
        public async Task<IActionResult> PostAsync([FromBody] ServiceCategory sc)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var newSc = new ServiceCategory { Description=sc.Description };
               await  _sc.CreateAsync(sc);
            }
            catch (Exception ex)
            {
                return BadRequest($"Unable to create catagory");
            }
            
            return  Ok(sc);
        }
       
        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceCategory>> GetByIdAsync(int id)
        {
            var model= await _sc.FindBy(i=>i.Id ==id).Include(x=>x.Services).FirstOrDefaultAsync();
             
            return Ok(model);
        }
        [HttpGet] 
        public async Task<ActionResult<ApiResult<ServiceCategory>>> GetAllServiceCategoriesAsync( 
                int pageIndex = 0,
                int pageSize = 10,
                string sortColumn = null,
                string sortOrder = null,
                string filterColumn = null,
                string filterQuery = null)
        {
            return await ApiResult<ServiceCategory>.CreateAsync(
                    _sc.GetAll(),
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
            return (ApiResult<ServiceCategory>.IsValidProperty(fieldName, true))
                ? _sc.GetAll().Any(
                    String.Format("{0} == @0 && Id != @1", fieldName),
                    fieldValue,
                    Id)
                : false;
        }
    }
}
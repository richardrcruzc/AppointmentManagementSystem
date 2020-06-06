using AmsApi.Data;
using AmsApi.Domain;
using AmsApi.Infraestructure;
using AmsApi.Models;
using AmsApi.Models.AccountViewModels;
using AmsApi.Models.ApplicationUsersModels;
using AmsApi.Repository;
using AmsApi.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Security.Claims;
using System.Threading.Tasks; 
using Microsoft.Extensions.Configuration;


namespace AmsApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api")]
    public class UsersController : ControllerBase
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<DbInitializer> _logger;
        private readonly IMapper _mapper;
        private IUserService _userService;
        private readonly AmsApiDbContext _context;
        private IRepository<Staff> _s;
        private IRepository<Client> _c;
        public UsersController(IUserService userService, AmsApiDbContext context, IRepository<Staff> s, IRepository<Client> c,
            IMapper mapper, SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ILogger<DbInitializer> logger)
        {
            _c = c;
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
            _mapper = mapper;
            _s = s;
            _context = context;
            _userService = userService;
        }

        [HttpPost("ForgotPassword")]
        [AllowAnonymous]        
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return BadRequest("Cannot verify your email");
                }

                // For more information on how to enable account confirmation and password reset please
                // visit https://go.microsoft.com/fwlink/?LinkID=532713
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                //var callbackUrl = Url.ResetPasswordCallbackLink(user.Id, code, Request.Scheme);
                //await _emailSender.SendEmailAsync(model.Email, "Reset Password",
                //   $"Please reset your password by clicking here: <a href='{callbackUrl}'>link</a>");
                return Ok(code); 
            }

            // If execution got this far, something failed, redisplay the form.
            return BadRequest("Cannot verify your email");
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPasswordAsync(ResetPasswordViewModel model)
        {
            
            if (!ModelState.IsValid)
            {
                return BadRequest("Unable to process action.");
            }
            if (model.Code == null)
            {
                return BadRequest("A code must be supplied for password reset.");
            }
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return BadRequest("Unable to process action.");
            }
            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
            {
                return Ok("Password changed.");
            }

            return BadRequest("Unable to process action.");
        }

        [HttpPost]
        [AllowAnonymous] 
        public async Task<IActionResult> LoginWithRecoveryCode(LoginWithRecoveryCodeViewModel model, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Unable to process action.");
            }

            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                return BadRequest($"Unable to load two-factor authentication user.");
            }

            var recoveryCode = model.RecoveryCode.Replace(" ", string.Empty);

            var result = await _signInManager.TwoFactorRecoveryCodeSignInAsync(recoveryCode);

            if (result.Succeeded)
            {
                _logger.LogInformation("User with ID {UserId} logged in with a recovery code.", user.Id);
                return Ok("User with ID { user.Id} logged in with a recovery code.");
            }
            if (result.IsLockedOut)
            {
                _logger.LogWarning("User with ID {UserId} account locked out.", user.Id);
                return BadRequest($"User with ID {user.Id} account locked out.");
            }
            else
            {
                _logger.LogWarning("Invalid recovery code entered for user with ID {UserId}", user.Id);
                ModelState.AddModelError(string.Empty, "Invalid recovery code entered.");
                return BadRequest($"Invalid recovery code entered for user with ID {user.Id}");
            }
        }

        [HttpPost("customergister")]
        public async Task<IActionResult> CustomerRegisterAsync([FromBody]ClientModel model)
        {

            var userExist = await _userService.GetUser(model.Email);
            if (userExist != null)
            {
                _logger.LogInformation($"User exist in staff {model.Email}.");
                return BadRequest(new { message = "Email is incorrect" });
            }
            var appUserExist = await _userManager.FindByEmailAsync(model.Email);
            if (appUserExist != null)
            {
                _logger.LogInformation($"User exist in userapp {model.Email}.");
                return BadRequest(new { message = "Email is incorrect" });
            }

            var aUser = new ApplicationUser { UserName = model.Email, Email = model.Email, EmailConfirmed = false};
            var result = _userManager.CreateAsync(aUser, model.Password).Result;
            if (result.Succeeded)
            {                
                await _userManager.AddToRolesAsync(aUser, new List<string> { "customer" });

                _logger.LogInformation($"User created a new account {model.Email} with password.");

                var staffNew = _mapper.Map<Staff>(model);

                await _s.CreateAsync(staffNew);
                model.Id = staffNew.Id;

                _logger.LogInformation($"Customer created a new account {model.Email} with password.");

            }

            return Ok(model);
        }



        [HttpPost("Userregister")]
        public async Task<IActionResult> UserRegisterAsync([FromBody]StaffModel model)
        { 

            var userExist = await _userService.GetUser(model.Email);
            if (userExist != null)
            {
                _logger.LogInformation($"User exist in staff {model.Email}.");
                return BadRequest(new { message = "Email is incorrect" });
            }
            var appUserExist = await _userManager.FindByEmailAsync(model.Email);
            if (appUserExist != null)
            {
                _logger.LogInformation($"User exist in userapp {model.Email}.");
                return BadRequest(new { message = "Email is incorrect" });
            }

            var aUser = new ApplicationUser { UserName = model.Email, Email = model.Email, EmailConfirmed = false};
            var result = _userManager.CreateAsync(aUser, model.Password).Result;
            if (result.Succeeded)
            {
                await _userManager.AddToRolesAsync(aUser, new List<string> { "staff" });

                _logger.LogInformation($"User created a new account {model.Email} with password.");

                var staffNew = _mapper.Map<Staff>(model);

                await _s.CreateAsync(staffNew);
                model.Id = staffNew.Id;

                _logger.LogInformation($"Staff created a new account {model.Email} with password.");

            }
            
            return Ok(model);
        }


        //[AllowAnonymous]
        [HttpPut("usereupdate/{id}")]
        public async Task<IActionResult> UserProfileAsync(int id,[FromBody]StaffModel model)
        {

            if(id!= model.Id)
                return BadRequest(new { message = "User information is incorrect" });

            var userExist =await _userService.GetById(id);
            if (userExist==null)
            {
                _logger.LogInformation($"User does not exist in staff {model.Email}.");
                return BadRequest(new { message = "Email is incorrect" });
            }
            
            userExist = _mapper.Map<Staff>(model);

            await _s.UpdateAsync(id, userExist);

            _logger.LogInformation($"Staff update {userExist.Email}.");

            return Ok(model);
        }
        [HttpPost("currentuser")]
        public async Task<IActionResult> GetCurrentUserAsync()
        {
            ClaimsPrincipal currentUser = this.User;
            var currentUserName = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            var user = await  _userManager.FindByEmailAsync(currentUserName);
            if (user != null)
            {
                var isInRoleAsync = await _userManager.IsInRoleAsync(user, "customer");

            if (!isInRoleAsync)
                {
                    //get staff user
                    var staff = _userService.GetAll().Where(x => x.Email == currentUserName).FirstOrDefault();
                    return Ok(staff);
                }
                
                //get customer user
                var custorm = _c.FindBy(x => x.Email == currentUserName).FirstOrDefault();
                return Ok(custorm);
            }
            return Ok(user);
        }

        [HttpGet("user-profile/{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var users = await _userService.GetById(id);
            return Ok(users);
        }
        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]UserModel model)
        {
            var user = _userService.Authenticate(model.Email, model.Password);

            if (user == null)
                return BadRequest(new { message = "Email or password is incorrect" });

            return Ok(user);
        }

        [HttpGet("getallusers")]
        public async Task<ActionResult<ApiResult<StaffModel>>> GetAllAsync(int pageIndex = 0,
                int pageSize = 10,
                string sortColumn = null,
                string sortOrder = null,
                string filterColumn = null,
                string filterQuery = null)
        {


            return await ApiResult<StaffModel>.CreateAsync(
                     _s.GetAll().Select(l =>
                     new StaffModel
                     {
                         Id = l.Id,
                         LastName = l.LastName,
                         FirstName = l.FirstName,
                         Mobile = l.Mobile,
                         Email = l.Email,
                         Active = l.Active,
                         Title = l.Title,
                         CanBook = l.CanBook,

                     }),
                     pageIndex,
                     pageSize,
                     sortColumn,
                     sortOrder,
                     filterColumn,
                     filterQuery); ; ;
        }
        
        
        [AllowAnonymous]
        [HttpPost]
        [Route("isValidEmail/{email}")]
        public async Task<bool> DoesEmailExist(string email)
        {
            var exist = await  _userManager.FindByEmailAsync(email);
            if (exist == null)
            {
                exist = await _userManager.FindByNameAsync(email);
                if (exist == null)
                    return false;
            }
            return true;



        }


       [HttpPost]
        [Route("IsDupeField")]
        public bool IsDupeField(
           int userId,
           string fieldName,
           string fieldValue)
        {
            // Dynamic approach (using System.Linq.Dynamic.Core)
            return (ApiResult<Staff>.IsValidProperty(fieldName, true))
                ? _s.GetAll().Any(
                    String.Format("{0} == @0 && Id != @1", fieldName),
                    fieldValue,
                    userId)
                : false;
        }
    }
}

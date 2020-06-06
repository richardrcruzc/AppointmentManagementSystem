using AmsApi.Domain;
using AmsApi.Models;
using AmsApi.Models.ApplicationUsersModels;
using AmsApi.Repository;
using AmsApi.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace AmsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IRepository<Token> _tk;

        private readonly ILogger<TokenController> _logger;

        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserService _service;
        private readonly AuthOptions _authOptions;
        public TokenController(IUserService service, IOptions<AuthOptions> authOptionsAccessor, IRepository<Token> tk,
        UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ILogger<TokenController> logger)
        {
            _tk = tk;
            _logger = logger;
            _signInManager = signInManager;
            _userManager = userManager;
            _service = service;
            _authOptions = authOptionsAccessor.Value;
        }

        [HttpPost]
        public async System.Threading.Tasks.Task<IActionResult> GetAsync([FromBody] GetTokenModel user)
        {
            var currentUser = await _userManager.FindByEmailAsync(user.Email);
            if (currentUser == null)
                currentUser = await _userManager.FindByNameAsync(user.Email);


            var result = await _signInManager.PasswordSignInAsync(user.Email, user.Password, true, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                _logger.LogInformation($"User {user.Email} logged in.");
            }
            else
            {
                _logger.LogError($"User {user.Email} error logged in.");
                return Ok(new
                {
                    error = "Unauthorized"
                });
            }
            var currentUserRole =await _userManager.GetRolesAsync(currentUser);
            var role = currentUserRole[0];


            var authClaims = new[]
               {
                    new Claim(JwtRegisteredClaimNames.Sub, currentUser.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, currentUser.Email.ToString()),
                    new Claim(ClaimTypes.Role, role)
                };

                var token = new JwtSecurityToken(
                    issuer: _authOptions.Issuer,
                    audience: _authOptions.Audience,
                    expires: DateTime.Now.AddHours(_authOptions.ExpiresInMinutes),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authOptions.SecureKey)),
                        SecurityAlgorithms.HmacSha256Signature)
                    );

                    var rt = CreateRefreshToken(currentUser.Id, currentUser.Id);
                    await _tk.CreateAsync(rt);

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo,
                    id = currentUser.Id,
                    userRole = role,
                    userName= currentUser.Email,
                }) ;
           
        }
        private Token CreateRefreshToken(string clientId, string userId)
        {
            return new Token()
            {
                ClientId = clientId,
                UserId = userId,
                Type = 0,
                Value = Guid.NewGuid().ToString("N"),
                CreatedDate = DateTime.UtcNow
            };
        }
        
    }
}

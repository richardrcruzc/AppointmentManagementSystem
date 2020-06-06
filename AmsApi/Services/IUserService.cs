using AmsApi.Domain;
using AmsApi.Infraestructure;
using AmsApi.Models;
using AmsApi.Models.ApplicationUsersModels;
using AmsApi.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AmsApi.Services
{
    public interface IUserService
    {
        Task<bool> IsValidUser(string email);
        Task<bool> IsValidUser(UserModel user);
        Task<Staff> GetById(int id);
        Task<Staff> GetUser(string userName);
        Task<UserModel> Authenticate(string username, string password);
        IEnumerable<Staff> GetAll();
    }
    public class UserService : IUserService
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger _logger;

        private IRepository<Staff> _s;
        private readonly IMapper _mapper;
        private readonly AmsApiDbContext _context;
        private readonly IConfiguration Configuration;
        public UserService(AmsApiDbContext context, IConfiguration configuration, IMapper mapper, IRepository<Staff> s,
              UserManager<ApplicationUser> userManager,
          SignInManager<ApplicationUser> signInManager,
            ILogger<UserService> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            
            _logger = logger;
            _s = s;
            _mapper = mapper;
            Configuration = configuration;
            _context = context;
        }
        public async Task<bool> IsValidUser(UserModel model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password,true, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                _logger.LogInformation($"User {model.Email} logged in.");

                return true;
            }
            if (result.RequiresTwoFactor)
            {
                _logger.LogInformation($"User {model.Email} RequiresTwoFactor.");
            }
            else if (result.IsLockedOut)
            {
                _logger.LogInformation($"User {model.Email} IsLockedOut");
            }
            else {
                _logger.LogInformation($"User {model.Email} Invalid login attempt");
            }
                return false ;            
        }

        public async Task<Staff> GetUser(string userName)
        {
            var user = await _userManager.FindByEmailAsync(userName);
            if(user==null)
                user = await _userManager.FindByNameAsync(userName);
            if (user != null)
            {
                var isConfirm = await _userManager.IsEmailConfirmedAsync(user);
                if (isConfirm)
                {
                    return await _s.FindBy(x => x.Email == user.Email).FirstOrDefaultAsync();
                }
            }
            return null;                
        }

        public async Task<UserModel> Authenticate(string username, string password)
        {
            var result = await _signInManager.PasswordSignInAsync(username, password, true, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                _logger.LogInformation("User logged in.");
            }

            var user = await _userManager.FindByEmailAsync(username);
            if (user == null)
                user = await _userManager.FindByNameAsync(username);
            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
            {

                // authentication successful so generate jwt token
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(Configuration["JWT:ServerSecret"]);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                    }),
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);

                var model = _mapper.Map<UserModel>(user);

                model.Token = tokenHandler.WriteToken(token);

                //return user.WithoutPassword();
                return model;
            }
            else
            {
                return new UserModel();
            }
        }

        public IEnumerable<Staff> GetAll()
        {
           return _s.GetAll();

          //  var model = quey.Select(x => _mapper.Map<UserModel>(x));
            //return query;
        }

        public async Task<Staff> GetById(int id)
        {
            return  await _s.GetById(id);
        }

        public async Task<bool> IsValidUser(string userName)
        {
            var user = await _userManager.FindByEmailAsync(userName);
            if (user == null)
                user = await _userManager.FindByNameAsync(userName);
            if (user != null)
            {
                return true;
            }
            else
            {
                return false;
            }

            }
    }
}

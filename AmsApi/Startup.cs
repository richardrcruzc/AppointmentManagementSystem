using AmsApi.Data;
using AmsApi.Domain;
using AmsApi.Infraestructure;
using AmsApi.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using AmsApi.Services;
using AmsApi.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using AutoMapper;
using AmsApi.Services.HealthCheck;
using AmsApi.Models.ApplicationUsersModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;

namespace AmsApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddCors();
            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins,
                builder =>
                {
                    builder.AllowAnyOrigin()                    
                                .AllowAnyMethod()
                                .AllowAnyHeader();
                });
            });
            services.AddControllers()
     .AddNewtonsoftJson(options =>
     options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
 );



            // Auto Mapper Configurations
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);


            services.AddDbContext<AmsApiDbContext>(options =>
           options.UseSqlServer(Configuration.GetConnectionString("AmsApiDbContext")));
            services.AddScoped<AmsApiDbContext>();

            // Add ASP.NET Core Identity support
            services.AddDefaultIdentity<ApplicationUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredLength = 8;
                options.User.RequireUniqueEmail = true;
                
            })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<AmsApiDbContext>();
            services.AddIdentityServer()
                   .AddApiAuthorization<ApplicationUser, AmsApiDbContext>();
            //services.AddAuthentication()
            //       .AddIdentityServerJwt();
             
            var authOptions = Configuration.GetSection("AuthOptions").Get<AuthOptions>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    //security switch
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    RequireExpirationTime = true,

                    //standard configuration
                    ValidIssuer = authOptions.Issuer,
                    ValidAudience = authOptions.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authOptions.SecureKey)),
                    
                };
            });



            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            services.AddTransient<IEmailSender, EmailSender>();
            services.AddScoped<IUserService, UserService>();
            services.Configure<AuthOptions>(Configuration.GetSection("AuthOptions"));

           

            services.AddHealthChecks()
                  .AddCheck("ICMP_01", new ICMPHealthCheck("www.ryadel.com", 100))
                .AddCheck("ICMP_02", new ICMPHealthCheck("www.google.com", 100))
                .AddCheck("ICMP_03", new ICMPHealthCheck("www.does-not-exist.com", 100));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(MyAllowSpecificOrigins);

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseHealthChecks("/hc", new CustomHealthCheckOptions());
            
            app.UseCors(x => x
               .AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader());

            app.UseAuthentication();
           // app.UseIdentityServer();
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapControllers();
                endpoints.MapControllerRoute(
                    name:"default",
                    pattern:"{controller}/{action=index}/{id?}");
            });


        }
    }
}

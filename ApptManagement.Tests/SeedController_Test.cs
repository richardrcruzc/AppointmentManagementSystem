using IdentityServer4.EntityFramework.Options;
using AmsApi.Controllers;
using AmsApi.Domain;
using AmsApi.Infraestructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Hosting;
using Moq;
using AmsApi.Models.ApplicationUsersModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ApptManagement.Tests
{
    public class SeedController_Test
    {
        public SeedController_Test()
        {
        }
        /// <summary>
        /// Test the CreateDefaultUsers() method
        /// </summary>
        [Fact]
        public async void CreateDefaultUsers()
        {
            #region Arrange
            // create the option instances required by the
            // AmsApiDbContext
            var options = new DbContextOptionsBuilder<AmsApiDbContext>()
                .UseInMemoryDatabase(databaseName: "AmsContext")
                .Options;
            var storeOptions = Options.Create(new
             OperationalStoreOptions());
            // create a IWebHost environment mock instance
            var mockEnv = new Mock<IWebHostEnvironment>().Object;
            // define the variables for the users we want to test
            ApplicationUser user_Admin = null;
            ApplicationUser user_User = null;
            ApplicationUser user_Employee = null;
            ApplicationUser user_NotExisting = null;
            #endregion
            #region Act
            // create a AmsApiDbContext instance using the
            // in-memory DB
            using (var context = new AmsApiDbContext(options,
             storeOptions))
            {
                // create a RoleManager instance
                var roleManager = IdentityHelper.GetRoleManager(
                    new RoleStore<IdentityRole>(context));
                // create a UserManager instance
                var userManager = IdentityHelper.GetUserManager(
                    new UserStore<ApplicationUser>(context));
                
                // create a SeedController instance
                var controller = new SeedController(
                    context, 
                    roleManager,
                    userManager,
                    mockEnv
                    );
                // execute the SeedController's CreateDefaultUsers()
                // method to create the default users (and roles)
                await controller.CreateDefaultUsers();
                // retrieve the users
                user_Admin = await userManager.FindByEmailAsync("admin@email.com");
                user_User = await userManager.FindByEmailAsync("user@email.com");
                user_Employee = await userManager.FindByEmailAsync("employee@email.com");
                user_NotExisting = await userManager.FindByEmailAsync("notexisting@email.com");
            }
            #endregion
            #region Assert
            Assert.True(
                user_Admin != null
                && user_User != null
                    && user_Employee != null
                && user_NotExisting == null
                );
            #endregion
        }
    }
}


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

namespace ApptManagement.Tests
{
    public class LocationController_Test
    {
        /// <summary>
        /// Test the GetLocation() method
        /// </summary>
        [Fact]
        public async void GetLocation()
        {
            #region Arrange
            // todo: define the required assets
            var storeOptions = Options.Create(new OperationalStoreOptions());
            var options = new DbContextOptionsBuilder<AmsApiDbContext>()
                .UseInMemoryDatabase(databaseName: "AmsContext")
                .Options;
            using(var context = new AmsApiDbContext(options, storeOptions))
            {
                context.Add(new Location(){
                    Id=1,
                    Active=true,
                    Address="Ab",
                    Address1="Ab",
                    Cancelation=true,
                    City="Test city",
                    Confirmation=true,
                    ContactEmail="contact@email.com",
                    ContactName="Contact NAme",
                    Country="CountryName",
                    Description="Locatin Description",
                    NoShowUp=true,
                    Phone="12345678901",
                    Reminder=true,
                    Rescheduling=true,
                    State="State testing",
                    ThankYou=true,
                    ZipCode="12345"
                });

                await context.SaveChangesAsync();
            }
            Location location_existing = null;
            Location location_notExisting = null;

            #endregion


            #region Act
            // todo: invoke the test   
            
            using(var context = new AmsApiDbContext(options,storeOptions))
            {
                var controller = new LocationsForTestController(context);
                location_existing = (await controller.GetLocation(1)).Value;
                location_notExisting = (await controller.GetLocation(2)).Value;

            }

            #endregion
            #region Assert
            // todo: verify that conditions are met.

            Assert.True(location_existing!=null  && location_notExisting==null);

            #endregion
        }
    }
}

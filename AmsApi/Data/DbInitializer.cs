using AmsApi.Domain;
using AmsApi.Infraestructure;
using AmsApi.Models.ApplicationUsersModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AmsApi.Data
{
    public class DbInitializer
    {
        public static void Initialize(AmsApiDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ILogger<DbInitializer>  logger)
        {
            context.Database.EnsureCreated();




            var locations = new Location[] {
             new Location{   Active=true, Address="7696 NW Greensprint St", Address1="Suit 123", ContactEmail="Ri@gmail.con", ContactName="Contact PersonNAme", Description="Este es un salo de belleza", State="FL", ZipCode="34987", Country="US",Phone="1234567890",City="Port Saint Lucie" },
             new Location{ Active=true, Address="123 NW Greensprint St", Address1="Suit 434", ContactEmail="aai@gmail.con", ContactName="PersonNAme", Description="Este es una barberia", State="GA", ZipCode="3093", Country="US" ,Phone="1234567811",City="Port Saint Lucie"},
            };
            if (!context.Locations.Any())
            {
                //return;   // DB has been seeded

                    context.Locations.AddRange(locations);

                context.SaveChanges();
            }
            var cds = new ClosedDate[] {
             new ClosedDate { Location =  locations.FirstOrDefault(), Description="Holyday 2", From=DateTime.Now.AddDays(55), To=DateTime.Now.AddDays(55) },
             new ClosedDate { Location =  locations.FirstOrDefault(), Description="Holyday 1", From=DateTime.Now.AddDays(5), To=DateTime.Now.AddDays(5) },
            };
            if (!context.ClosedDates.Any())
            {
                //return;   // DB has been seeded

                
                    context.ClosedDates.AddRange(cds);
                
                context.SaveChanges();
            }




            var scs = new ServiceCategory[] { 
                 new ServiceCategory { Description="Haircuts and Hairdressing" },
                 new ServiceCategory { Description="Color" }

                };
            if (!context.ServiceCategories.Any())
            {
                
                    context.ServiceCategories.AddRange(scs);
                
                context.SaveChanges();
            }
           
                var ss = new Service[] { 
                 new Service { ServiceCategory = scs.Single(s=>s.Description=="Haircuts and Hairdressing"), ActiveStatus=true, DurationHour=1,DurationMinute=30,  Price=50.00m, ServiceDescription="Modern Hair cuts", ServiceName="Hair Cuts" },
                 new Service {  ServiceCategory = scs.Single(s=>s.Description=="Haircuts and Hairdressing"), ActiveStatus=true, DurationHour=1,DurationMinute=15,  Price=45.00m, ServiceDescription="Modern Hair cuts", ServiceName="Wash" },
                 new Service { ServiceCategory = scs.Single(s=>s.Description=="Haircuts and Hairdressing"), ActiveStatus=true, DurationHour=1,DurationMinute=30,  Price=50.00m, ServiceDescription="Hoof cuts", ServiceName="Manicure and Pedicure" },
                };
            if (!context.Services.Any())
            {
                context.Services.AddRange(ss);
                context.SaveChanges();
            }


             var clients = new Client[] {
             new Client{AcceptsMarketingNotifications=true, Active=true, Email="cl1@com.com", FirstName="Client1", LastName="last 1", Mobile="1234567890", SendNotificationBy="phone",  Photo="234234"},
             new Client{ AcceptsMarketingNotifications=true, Active=true, Email="cl2@com.com", FirstName="Client 2", LastName="last 2", Mobile="1234567890", SendNotificationBy="email" , Photo="234234"},
             new Client{ AcceptsMarketingNotifications=true, Active=true, Email="cl3@com.com", FirstName="Client 3", LastName="last 3", Mobile="1234567890", SendNotificationBy="email" , Photo="234234"},
            };
            if (!context.Clients.Any())
            {

                context.Clients.AddRange(clients);
              
                context.SaveChanges();
            }

            var uRoles = new IdentityRole { Name="admin", ConcurrencyStamp= "adminrole" };
            IdentityResult resultR = roleManager.CreateAsync(uRoles).Result;
            uRoles = new IdentityRole { Name = "staff", ConcurrencyStamp = "staffrole" };
            resultR = roleManager.CreateAsync(uRoles).Result;
            uRoles = new IdentityRole { Name = "customer", ConcurrencyStamp = "customerrole" };
            resultR = roleManager.CreateAsync(uRoles).Result;
            

            var aUser = new ApplicationUser { UserName = "cl1@com.com", Email = "cl1@com.com", EmailConfirmed=true };
            var result =  userManager.CreateAsync(aUser,"Password1$").Result;

         
            var resultRole= userManager.AddToRoleAsync(aUser, "customer").Result;


            if (result.Succeeded)
            {
                logger.LogInformation("User created a new account with password.");

                var token = userManager.GenerateEmailConfirmationTokenAsync(aUser).Result;
             var results=   userManager.ConfirmEmailAsync(aUser, token).Result;

                aUser = new ApplicationUser { UserName = "cl2@com.com", Email = "cl2@com.com", EmailConfirmed = true };
                results = userManager.CreateAsync(aUser, "Password1$").Result;
                resultRole = userManager.AddToRoleAsync(aUser, "customer").Result;

                token = userManager.GenerateEmailConfirmationTokenAsync(aUser).Result;
                results = userManager.ConfirmEmailAsync(aUser, token).Result;


                aUser = new ApplicationUser { UserName = "cl3@com.com", Email = "cl3@com.com", EmailConfirmed = true };
                userManager.CreateAsync(aUser, "Password1$").Wait();
                resultRole = userManager.AddToRoleAsync(aUser, "customer").Result;

            }
            var sts = new Staff[] {
            new Staff { Active=true, BriefCv="Nice cuts", CanBook=true, Email="urs1@usr.com", FirstName="Staly 1", LastName="st", Mobile ="1234567890", Title="Cutter" },
            new Staff { Active=true, BriefCv="Nice cuts same", CanBook=true, Email="urs2@usr.com", FirstName="St3", LastName="tr", Mobile ="1234563333", Title="Very good"},
            new Staff { Active=true, BriefCv="D' Penco Nice cuts same", CanBook=true, Email="urs3@usr.com", FirstName="the", LastName="penco", Mobile ="1234562233", Title="The Penco"},
            };
            if (!context.Staffs.Any())
            {   
                    context.Staffs.AddRange(sts);                
                context.SaveChanges();
            }
            
            aUser = new ApplicationUser { UserName = "urs1@usr.com", Email = "urs1@usr.com", EmailConfirmed = true};
            result= userManager.CreateAsync(aUser, "Password1$").Result;
            resultRole = userManager.AddToRoleAsync(aUser, "staff").Result;

            if (result.Succeeded)
            {
                var token = userManager.GenerateEmailConfirmationTokenAsync(aUser).Result;
                var results = userManager.ConfirmEmailAsync(aUser, token).Result;

                aUser = new ApplicationUser { UserName = "urs2@usr.com", Email = "urs2@usr.com", EmailConfirmed = true};
                userManager.CreateAsync(aUser, "Password1$").Wait();                
                resultRole = userManager.AddToRoleAsync(aUser, "admin").Result;

                token = userManager.GenerateEmailConfirmationTokenAsync(aUser).Result;
                 results =userManager.ConfirmEmailAsync(aUser, token).Result;

                aUser = new ApplicationUser { UserName = "urs3@usr.com", Email = "urs3@usr.com", EmailConfirmed = true};
                userManager.CreateAsync(aUser, "Password1$").Wait();
                resultRole = userManager.AddToRoleAsync(aUser, "staff").Result;
            }

            var whs = new StaffWorkingHour[] {
             new StaffWorkingHour { EndRepeat= EndRepeat.Ongoing, From=DateTime.Now, To =DateTime.Now.AddHours(5), Location=locations.FirstOrDefault(), Staff = sts.FirstOrDefault() },
             new StaffWorkingHour { EndRepeat= EndRepeat.Ongoing, From=DateTime.Now, To =DateTime.Now.AddHours(3), Location=locations.LastOrDefault(), Staff = sts.LastOrDefault() },

            };
            if (!context.StaffWorkingHours.Any())
            {
                
                    context.StaffWorkingHours.AddRange(whs);
                
                context.SaveChanges();

            }

        }

    }
}

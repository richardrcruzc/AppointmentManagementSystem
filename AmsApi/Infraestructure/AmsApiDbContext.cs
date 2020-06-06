using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using AmsApi.Domain;
using AmsApi.Models.ApplicationUsersModels;
using System.Linq;
using System;
using System.Threading.Tasks;
using System.Threading;

namespace AmsApi.Infraestructure
{
     
    public class AmsApiDbContext :ApiAuthorizationDbContext<ApplicationUser>
    {
        public AmsApiDbContext(DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions)
            : base(options, operationalStoreOptions)
        {
        
        }
        public string CurrentUserId { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<ClosedDate> ClosedDates { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<NotificationMessage> NotificationMessages { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<ServiceCategory> ServiceCategories { get; set; }
        public DbSet<Staff> Staffs { get; set; }
        public DbSet<StaffWorkingHour> StaffWorkingHours { get; set; }
        public DbSet<Token> Tokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Course>().ToTable("Course");
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ApplicationUser>().HasMany(u => u.Tokens).WithOne(i => i.User);

            modelBuilder.Entity<Token>().ToTable("Tokens");
            modelBuilder.Entity<Token>().Property(i => i.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Token>().HasOne(i => i.User).WithMany(u => u.Tokens);

            
        }

        public override int SaveChanges()
        {
            UpdateAuditEntities();
            return base.SaveChanges();
        }


        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            UpdateAuditEntities();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }


        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            UpdateAuditEntities();
            return base.SaveChangesAsync(cancellationToken);
        }


        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            UpdateAuditEntities();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void UpdateAuditEntities()
        {
            var modifiedEntries = ChangeTracker.Entries()
                .Where(x => x.Entity is IEntity && (x.State == EntityState.Added || x.State == EntityState.Modified));


            foreach (var entry in modifiedEntries)
            {
                var entity = (IEntity)entry.Entity;
                DateTime now = DateTime.UtcNow;

                if (entry.State == EntityState.Added)
                {
                    entity.CreatedDate = now;
                    entity.CreatedBy = CurrentUserId;
                }
                else
                {
                    base.Entry(entity).Property(x => x.CreatedBy).IsModified = false;
                    base.Entry(entity).Property(x => x.CreatedDate).IsModified = false;
                }

                entity.UpdatedDate = now;
                entity.UpdatedBy = CurrentUserId;
            }
        }
    }
}

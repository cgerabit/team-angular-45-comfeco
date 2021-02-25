using BackendComfeco.Models;
using BackendComfeco.Settings;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace BackendComfeco
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        
        {
            
        }

       
        protected override void OnModelCreating(ModelBuilder builder)
        { 
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>()
              .HasMany(x => x.Roles)
              .WithOne()
              .HasForeignKey(x => x.UserId)
              .IsRequired()
              .OnDelete(DeleteBehavior.Cascade);

         

            builder.Entity<ApplicationUserSocialNetwork>().HasKey(x => new { x.UserId, x.SocialNetworkId });

            builder.Entity<ApplicationUserTechnology>().HasKey(x => new { x.UserId, x.TechnologyId });


            builder.Entity<UserAuthenticationCode>().HasKey(x => x.Token);

            //-----------------------ONLY DEBUG------------------------------

            builder.Entity<ExternalLoginValidRedirectUrl>()
                .HasData(new ExternalLoginValidRedirectUrl { Id = 1, Url = "http://localhost:4200/auth/external-signin-callback" });
            builder.Entity<ExternalLoginValidRedirectUrl>()
                .HasData(new ExternalLoginValidRedirectUrl { Id = 2, Url = "https://localhost:4200/auth/external-signin-callback" });


            builder.Entity<PersistentLoginValidRedirectUrl>()
                .HasData(new PersistentLoginValidRedirectUrl { Id = 1, Url = "https://localhost:4200/auth/persistent-signin-callback" });
            builder.Entity<PersistentLoginValidRedirectUrl>()
              .HasData(new PersistentLoginValidRedirectUrl { Id = 2, Url = "http://localhost:4200/auth/persistent-signin-callback" });


            //----------------------------------------------------------

            // Add content creators: 

            builder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Id = ApplicationConstants.Roles.ContentCreatorRoleId,
                Name = ApplicationConstants.Roles.ContentCreatorRoleName,
                NormalizedName = ApplicationConstants.Roles.ContentCreatorRoleName
            });

        }
        public DbSet<ExternalLoginValidRedirectUrl> ExternalLoginValidRedirectUrls { get; set; }

        public DbSet<UserAuthenticationCode> UserAuthenticationCodes { get; set; }

        public DbSet<PersistentLoginValidRedirectUrl> PersistentLoginValidRedirectUrls { get; set; }

        public DbSet<Area> Areas { get; set; }

        public DbSet<Technology> Technologies { get; set; }

        public DbSet<SocialNetwork> SocialNetworks { get; set; }

        public DbSet<Workshop> Workshops { get; set; }

        public DbSet<Comunity> Comunities { get; set; }

        public DbSet<ApplicationUserSocialNetwork> ApplicationUserSocialNetworks { get; set; }

        public DbSet<ApplicationUserTechnology> ApplicationUserTechnologies { get; set; }

        public DbSet<Sponsor> Sponsors { get; set; }

        public DbSet<Event> Events { get; set; }

    }
}

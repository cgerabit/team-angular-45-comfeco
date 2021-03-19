using BackendComfeco.Models;
using BackendComfeco.Settings;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using Org.BouncyCastle.Math.EC.Rfc7748;

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

            builder.Entity<ApplicationUserEvents>().HasKey(x => new
            {
                x.UserId,
                x.EventId
            });

            builder.Entity<UserAuthenticationCode>().HasKey(x => x.Token);
            
            builder.Entity<Event>().HasQueryFilter(e => e.IsActive);

            builder.Entity<ApplicationUser>()
                .HasMany(x =>x.ApplicationUserEvents)
                .WithOne(x=>x.User)
                .HasForeignKey(x=>x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ApplicationUser>()
                .HasOne(x => x.Specialty)
                .WithMany(x => x.Specialists)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<ApplicationUserBadges>().HasKey(x => new
            {
                x.UserId,x.BadgeId

            });

            
           

            builder.Entity<Badge>().HasData(new Badge
            {
                Name="Sociable",
                Id=1,
                Description="Esta persona es muy sociable",
                Instructions="Para obtener esta insignia actualiza los datos de tu perfil"
            });
            builder.Entity<Badge>().HasData(new Badge
            {
                Name="Concursante",
                Id=2,
                Description="Esta persona es muy competitiva",
                Instructions="Inscribete en tu primer evento"

            });
            builder.Entity<Badge>().HasData(new Badge
            {
                Name = "Grupero",
                Id = 3,
                Description = "Esta persona le encanta compartir en comunidad",
                Instructions = "Unete a tu primer grupo"

            });

            //-----------------------ONLY DEBUG------------------------------

            builder.Entity<ExternalLoginValidRedirectUrl>()
                .HasData(new ExternalLoginValidRedirectUrl { Id = 1, Url = "http://localhost:4200/auth/external-signin-callback" });
            builder.Entity<ExternalLoginValidRedirectUrl>()
                .HasData(new ExternalLoginValidRedirectUrl { Id = 2, Url = "https://localhost:4200/auth/external-signin-callback" });

            builder.Entity<ExternalLoginValidRedirectUrl>()
                .HasData(new ExternalLoginValidRedirectUrl{ Id =3 ,Url= "http://team45.comfeco.cristiangerani.com/auth/external-signin-callback" });

            builder.Entity<ExternalLoginValidRedirectUrl>()
               .HasData(new ExternalLoginValidRedirectUrl { Id = 4, Url = "https://team45.comfeco.cristiangerani.com/auth/external-signin-callback" });


            builder.Entity<PersistentLoginValidRedirectUrl>()
                .HasData(new PersistentLoginValidRedirectUrl { Id = 1, Url = "https://localhost:4200/auth/persistent-signin-callback" });
            builder.Entity<PersistentLoginValidRedirectUrl>()
              .HasData(new PersistentLoginValidRedirectUrl { Id = 2, Url = "http://localhost:4200/auth/persistent-signin-callback" });

            builder.Entity<PersistentLoginValidRedirectUrl>()
               .HasData(new PersistentLoginValidRedirectUrl { Id = 3, Url = "https://team45.comfeco.cristiangerani.com/auth/external-signin-callback" });
            builder.Entity<PersistentLoginValidRedirectUrl>()
              .HasData(new PersistentLoginValidRedirectUrl { Id = 4, Url = "http://team45.comfeco.cristiangerani.com/auth/external-signin-callback" });

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

        public DbSet<ApplicationUserEvents> ApplicationUserEvents { get; set; }

        public DbSet<Country> Countries { get; set; }

        public DbSet<Gender> Genders { get; set; }

        public DbSet<Group> Groups { get; set; }
        public DbSet<ApplicationUserBadges> ApplicationUserBadges { get; set; }
        public DbSet<Badge> Badges { get; set; }
        public DbSet<UserActivity> UserActivities { get; set; }
    }
}

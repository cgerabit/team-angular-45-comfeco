using BackendComfeco.Models;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BackendComfeco
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        
        {
            
        }

       
        protected override void OnModelCreating(ModelBuilder builder)
        {
            //-----------------------ONLY DEBUG------------------------------

            builder.Entity<ExternalLoginValidRedirectUrl>()
                .HasData(new ExternalLoginValidRedirectUrl {Id=1 ,Url="http://localhost:4200/auth/external-signin-callback" });
            builder.Entity<ExternalLoginValidRedirectUrl>()
                .HasData(new ExternalLoginValidRedirectUrl { Id=2,Url="https://localhost:4200/auth/external-signin-callback" });


            builder.Entity<PersistentLoginValidRedirectUrl>()
                .HasData( new PersistentLoginValidRedirectUrl {Id=1,Url= "https://localhost:4200/auth/persistent-signin-callback" });
              builder.Entity<PersistentLoginValidRedirectUrl>()
                .HasData( new PersistentLoginValidRedirectUrl {Id=2,Url= "http://localhost:4200/auth/persistent-signin-callback" });


            //----------------------------------------------------------

            builder.Entity<UserAuthenticationCode>().HasKey(x => x.Token);

            base.OnModelCreating(builder);
        }
        public DbSet<ExternalLoginValidRedirectUrl> ExternalLoginValidRedirectUrls { get; set; }

        public DbSet<UserAuthenticationCode> UserAuthenticationCodes { get; set; }

        public DbSet<PersistentLoginValidRedirectUrl> PersistentLoginValidRedirectUrls { get; set; }

    }
}

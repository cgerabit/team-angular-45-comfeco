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

            builder.Entity<ExternalLoginValidRedirectURL>()
                .HasData(new ExternalLoginValidRedirectURL {Id=1 ,Url="http://localhost:4200/auth/external-signin-callback" });
            builder.Entity<ExternalLoginValidRedirectURL>()
                .HasData(new ExternalLoginValidRedirectURL { Id=2,Url="https://localhost:4200/auth/external-signin-callback" });


            //----------------------------------------------------------
            

            base.OnModelCreating(builder);
        }
        public DbSet<ExternalLoginValidRedirectURL> ExternalLoginValidsRedirectUrl { get; set; }

        public DbSet<UserAuthenticationCode> UserAuthenticationCodes { get; set; }
    }
}

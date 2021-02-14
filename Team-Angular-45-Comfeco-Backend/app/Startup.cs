using BackendComfeco.Helpers;
using BackendComfeco.Models;
using BackendComfeco.Settings;
using BackendComfeco.Shared.Settings;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

using System;
using System.Text;

namespace BackendComfeco
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ComfecoBackend", Version = "v1" });
            });
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("ComfecoProjectDb")));
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                  Encoding.UTF8.GetBytes(Configuration["auth:key"])),
                    ClockSkew = TimeSpan.Zero,
                    SaveSigninToken = true

                };
            }).AddFacebook(config => {
                    config.AppId = Configuration["facebookauth:appid"];
                    config.AppSecret = Configuration["facebookauth:appsecret"];
                }).AddGoogle(config =>
                {
                    config.ClientId = Configuration["googleauth:clientid"];
                    config.ClientSecret = Configuration["googleauth:clientsecret"];
                });

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedEmail = false;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
                options.User.RequireUniqueEmail = true;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<DataProtectionTokenProviderOptions>(options =>
            {
                options.TokenLifespan = TimeSpan.FromHours(1);

            });
            services.Configure<MailSettings>(Configuration.GetSection("MailSettings"));
            services.AddCors(
                options =>
            {
                options.AddPolicy(ApplicationConstants.DEVELOPMENTCORSPOLICYNAME,
                    new CorsPolicyBuilder()
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .Build());



                options.DefaultPolicyName = ApplicationConstants.DEVELOPMENTCORSPOLICYNAME;
            });
            services.AddSingleton<IEmailService, EmailService>();
            services.AddTransient<ThreadSafeRandom>();
            services.AddAutoMapper(typeof(Startup));

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
              
            }
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ComfecoBackend v1"));

            app.UseHttpsRedirection();
            
            app.UseRouting();
            app.UseCors(ApplicationConstants.DEVELOPMENTCORSPOLICYNAME);

            app.UseStaticFiles();
            app.UseAuthentication();
            
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

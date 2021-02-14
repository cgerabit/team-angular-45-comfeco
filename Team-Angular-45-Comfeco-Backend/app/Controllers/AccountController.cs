using AutoMapper;

using TeamAngular45Backend.DTOs.Auth;
using TeamAngular45Backend.DTOs.Email;
using TeamAngular45Backend.Helpers;
using TeamAngular45Backend.Models;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace TeamAngular45Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {

        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IMapper mapper;
        private readonly IEmailService mailService;
        private readonly IWebHostEnvironment env;
        private readonly IConfiguration configuration;
        private readonly ThreadSafeRandom threadSafeRandom;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IMapper mapper,
            IEmailService mailService,
            IWebHostEnvironment env,
            IConfiguration configuration,
            ThreadSafeRandom threadSafeRandom
            )
        {

            this.userManager = userManager;
            this.signInManager = signInManager;
            this.mapper = mapper;
            this.mailService = mailService;
            this.env = env;
            this.configuration = configuration;
            this.threadSafeRandom = threadSafeRandom;
        }


        [HttpPost("register")]
        public async Task<ActionResult> Register(ApplicationUserCreationDTO aplicationUserCreationDTO)
        {

            var user = mapper.Map<ApplicationUser>(aplicationUserCreationDTO);


            var result = await userManager.CreateAsync(user, aplicationUserCreationDTO.Password);

            if (result.Succeeded)
            {
                //TODO :
                //ADD SOME DEFAULT Claims, roles etc


                await SendEmailConfirmation(user);

                return Ok();
            }


            return BadRequest(result.Errors.Select(x => x.Description));

        }

        [HttpPost("login")]
        public async Task<ActionResult<TokenResponse>> Login(LoginDTO loginDTO)
        {
            ApplicationUser user = await (loginDTO.IsEmail ? userManager.FindByEmailAsync(loginDTO.UserName) : userManager.FindByNameAsync(loginDTO.UserName));

            if (user == null)
            {

                return BadRequest("Invalid credentials");
            }


            var result = await signInManager.PasswordSignInAsync(user, loginDTO.Password, false, true);
            if (result.IsLockedOut)
            {
                return BadRequest("Your account is locked");
            }
            else if (result.RequiresTwoFactor)
            {
                //TODO:
                //Build two factor fluid
            }
            else if (result.Succeeded)
            {
                return await BuildLoginToken(user.Email, loginDTO.PersistLogin);
            }

            if (!user.EmailConfirmed)
            {
                return BadRequest("Please confirm your email");

            }

            return BadRequest("Invalid credentials");

        }



        [HttpGet("confirmaccount")]
        public async Task<ActionResult> ConfirmAccount([FromQuery] ConfirmEmailDTO confirmEmailDTO)
        {
            
            //confirmEmailDTO.Token = HttpUtility.UrlDecode(confirmEmailDTO.Token);
            //confirmEmailDTO.UserId = HttpUtility.UrlDecode(confirmEmailDTO.UserId);

            var user = await userManager.FindByIdAsync(confirmEmailDTO.UserId);
            if (user == null)
            {
                return BadRequest("El enlace es incorrecto");
            }

            var result = await userManager.ConfirmEmailAsync(user, confirmEmailDTO.Token);

            if (result.Succeeded)
            {
                return Ok("Account confirmed");
            }
            else
            {
                return BadRequest();
            }

        }

        [HttpPost("resendconfirmation")]
        public async Task<ActionResult> ResendConfirmationEmail(ResendConfirmationDTO resendConfirmationEmailDTO)
        {
            var user = await userManager.FindByEmailAsync(resendConfirmationEmailDTO.Email);

            await SendEmailConfirmation(user);

            return Ok();

        }

        private async Task<TokenResponse> BuildLoginToken(string email, bool persist)
        {

            var user = await userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return null;
            }

            var roles = await userManager.GetRolesAsync(user);

            var userInfo = mapper.Map<UserInfo>(user);

            return BuildToken(userInfo, roles, persist);

        }
        private async Task<bool> SendEmailConfirmation(ApplicationUser user)
        {

            if (user != null)
            {
                if (!user.EmailConfirmed)
                {
                    var token = await userManager.GenerateEmailConfirmationTokenAsync(user);

                    var currentUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";

                    // En un futuro esto apuntara al front y sera el front el encargado de enviar el request
                    string url = $"{currentUrl}/api/account/confirmaccount?UserId={HttpUtility.UrlEncode(user.Id)}&Token={HttpUtility.UrlEncode(token)}";


                    string body = System.IO.File.ReadAllText(Path.Combine(env.WebRootPath, "templates", "index.htm"));




                    var logoSrc = $"{currentUrl}/LogoEmail756x244.png";

                    await mailService.SendEmailAsync(new MailRequest
                    {

                        Body = $"{body.Replace("TEXTOCONFIRMAR", $"Confirmar Email").Replace("_URL_", url).Replace("_SALUDO_", $"Hola, {user.UserName} por favor has click en el enlace para confirmar que eres el propietario de este email.").Replace("SRCLOGO", $"{logoSrc}")}",
                        Subject = $"Hola {user.UserName} Por favor confirma tu cuenta",
                        ToEmail = user.Email,
                    });

                    return true;
                }
            }



            return false;
        }
        [HttpPost("sendrecoverpwd")]
        public async Task<ActionResult> SendPasswordRecovery(SendPasswordRecoveryDTO recoveryDTO)
        {

            await SendRecoverPasswordEmail(recoveryDTO.Email);


            return Ok();
        }
        [HttpPost("confirmrecoverpwd")]
        public async Task<ActionResult> ConfirmPasswordRecovery(ConfirmPasswordRecoveryDTO confirmPasswordRecoveryDTO)
        {
            confirmPasswordRecoveryDTO.Token = HttpUtility.UrlDecode(confirmPasswordRecoveryDTO.Token);
            var user = await userManager.FindByIdAsync(confirmPasswordRecoveryDTO.UserId);
            if (user == null)
            {
                return BadRequest();
            }

            var result = await userManager.ResetPasswordAsync(user, confirmPasswordRecoveryDTO.Token, confirmPasswordRecoveryDTO.Password);

            if (result.Succeeded)
            {


                return Ok();
            }

            return BadRequest();

        }

        [HttpGet("externalproviders")] 
        public async Task<ActionResult<List<ExternalProvidersDTO>>> GetExternalProviders()
        {
            var externalProviders = await signInManager.GetExternalAuthenticationSchemesAsync();

            return mapper.Map<List<ExternalProvidersDTO>>(externalProviders);

        }
        [HttpGet("externallogin")]
        public ActionResult ExternalLogin([FromQuery] ExternalLoginDTO externalLoginDTO)
        {
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Account", new { externalLoginDTO.returnUrl });

            var properties = signInManager.ConfigureExternalAuthenticationProperties(externalLoginDTO.Provider, redirectUrl);

            return Challenge(properties,externalLoginDTO.Provider);
        }

        [HttpGet("externallogincallback")]
        public async Task<ActionResult<TokenResponse>> ExternalLoginCallback([FromQuery]string returnUrl)
        {
            var info = await signInManager.GetExternalLoginInfoAsync();

            if (info == null)
            {
                return BadRequest();
            }

            var result = await signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false);

            if (result.Succeeded)
            {
                var userIdentifier = info.Principal.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdentifier == null)
                {
                    return BadRequest("An unexpected error has been ocurred");
                }

                var user = await userManager.FindByLoginAsync(info.LoginProvider, userIdentifier.Value);

                return await BuildLoginToken(user.Email, false);
            }
            else
            {
                var email = info.Principal.FindFirst(ClaimTypes.Email);
                if (email == null)
                {
                    return BadRequest("An unexpected error has been ocurred");

                }
                var user = await userManager.FindByEmailAsync(email.Value);
                if (user != null)
                {
                    return BadRequest("Un usuario con ese email ya esta registrado por favor inicia session y enlaza este metodo de autenticacion");
                }
                string name;
                var username = info.Principal.FindFirst(ClaimTypes.Name);
                if (username == null)
                {
                    return BadRequest("An unexpected error has been ocurred");
                }

                name = username.Value.Replace(" ", "_");

                user = await userManager.FindByNameAsync(name);
                if (user != null)
                {
                    //Si el nombre de usuario ya existe le agregamos un numero aleatorio al final probablemente sea conveniente hacer un endpoint intermedio para preguntarle al usuario que username quiere
                    name += threadSafeRandom.Next(1, 20000).ToString();
                }


                var nUser = new ApplicationUser
                {
                    UserName = name,
                    Email = email.Value
                };

                var creationResult = await userManager.CreateAsync(nUser);


                if (creationResult.Succeeded)
                {

                    var extLoginAdditionResult= await userManager.AddLoginAsync(nUser, info);
                    if (!extLoginAdditionResult.Succeeded)
                    {
                        return BadRequest();
                    }

                   await  SendEmailConfirmation(nUser);

                    return new TokenResponse
                    {
                        ResponseType = "Register"
                    };

                }
               

            }

            return BadRequest();
        }

        private async Task SendRecoverPasswordEmail(string Email)
        {
            var user = await userManager.FindByEmailAsync(Email);
            if (user != null)
            {
                var token = await userManager.GeneratePasswordResetTokenAsync(user);
                // TODO : ENLACE DE LA APP DE ANGULAR PARA RECUPERAR PASSWORD
                var currentUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";

                // TODO:
                // En un futuro esto debe apuntar al front
                string url = $"ourcurrentfronturl/api/account/confirmrecoverypwd?UserId={HttpUtility.UrlEncode(user.Id)}&Token={HttpUtility.UrlEncode(token)}";



                string body = System.IO.File.ReadAllText(Path.Combine(env.WebRootPath, "templates", "index.htm"));


                var logoSrc = $"{currentUrl}/LogoEmail756x244.png";

                await mailService.SendEmailAsync(new MailRequest
                {

                    Body = $"{body.Replace("TEXTOCONFIRMAR", $"Cambiar mi Contraseña").Replace("_URL_", url).Replace("_SALUDO_", $"Hola, {user.UserName} has solicitado un cambio de contraseña, por favor has click en el enlace para confirmar el cambio").Replace("SRCLOGO", $"{logoSrc}").Replace("BIENVENIDO", "REESTABLECER CONTRASEÑA")}",
                    Subject = $"Hola {user.UserName} reestablece tu contraseña aqui",
                    ToEmail = Email,
                });

            }

        }


        private TokenResponse BuildToken(UserInfo userInfo, IList<string> roles, bool persistLogin)
        {
            var claims = new List<Claim>()
            {
               new Claim(JwtRegisteredClaimNames.UniqueName, userInfo.Email),
               new Claim(ClaimTypes.Name, userInfo.Email),
               new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
               new Claim(JwtRegisteredClaimNames.Sub,userInfo.UserId)
            };

            foreach (var rol in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, rol));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["auth:key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Si necesitamos mantener al usuario logueado generamos un token con 3 dias de antiguedad, 
            var expiration = persistLogin ? DateTime.UtcNow.AddDays(3) : DateTime.UtcNow.AddMinutes(60);

            JwtSecurityToken token = new JwtSecurityToken(
               issuer: null,
               audience: null,
               claims: claims,
               expires: expiration,
               signingCredentials: creds);

            return new TokenResponse()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration
            };
        }



    }
}

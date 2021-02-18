﻿using AutoMapper;

using BackendComfeco.DTOs.Auth;
using BackendComfeco.DTOs.Email;
using BackendComfeco.Helpers;
using BackendComfeco.Models;
using BackendComfeco.Settings;

<<<<<<< Updated upstream
=======
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
>>>>>>> Stashed changes
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

namespace BackendComfeco.Controllers
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
        private readonly ApplicationDbContext applicationDbContext;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IMapper mapper,
            IEmailService mailService,
            IWebHostEnvironment env,
            IConfiguration configuration,
            ThreadSafeRandom threadSafeRandom,
            ApplicationDbContext applicationDbContext
            )
        {

            this.userManager = userManager;
            this.signInManager = signInManager;
            this.mapper = mapper;
            this.mailService = mailService;
            this.env = env;
            this.configuration = configuration;
            this.threadSafeRandom = threadSafeRandom;
            this.applicationDbContext = applicationDbContext;
        }


<<<<<<< Updated upstream
=======
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("refreshToken")]
        public async Task<ActionResult<TokenResponse>> RefreshToken()
        {
            var user = await GetUserFromContext();

            if (user == null) { return Unauthorized(); }

            return await BuildLoginToken(user.Email, false);
        }


        [Authorize(AuthenticationSchemes = ApplicationConstants.PersistLoginSchemeName)]
        [HttpGet("havepersistlogin")]
        public ActionResult IsPersistent()
        {
            return Ok();
        }

        [HttpGet("logout")]
        public async Task Logout()
        {
            await HttpContext.SignOutAsync(ApplicationConstants.PersistLoginSchemeName);
        }

        [Authorize(AuthenticationSchemes = ApplicationConstants.PersistLoginSchemeName)]
        [HttpGet("persistLogin")]
        public async Task<ActionResult> PersistLogin([FromQuery] LoginWithPersistDTO loginWithPersistDTO)
        {
            var user = await GetUserFromContext();
            if (user == null) { return RedirectToAction(nameof(RedirectToLogin)); }

            loginWithPersistDTO.SecurityKeyHash = CryptographyUtils.Base64Decode(HttpUtility.UrlDecode(loginWithPersistDTO.SecurityKeyHash));

            if (!(await PersistentRedirectUrlExist(loginWithPersistDTO.returnUrl)))
            {
                return BadRequest("returnUrl no es un valor valido");
            }
            var token = await userManager.GenerateUserTokenAsync(user, ApplicationConstants.AuthCodeTokenProviderName, ApplicationConstants.PersistentLoginTokenPurposeName);

            var authenticationCode = new UserAuthenticationCode
            {
                Expiration = DateTime.UtcNow.AddMinutes(5),
                SecurityKey = loginWithPersistDTO.SecurityKeyHash,
                Token = token,
                UserId = user.Id
            };

            applicationDbContext.Add(authenticationCode);

            await applicationDbContext.SaveChangesAsync();


            string url = $"{loginWithPersistDTO.returnUrl}?authcode={HttpUtility.UrlEncode(token)}";

            if (!string.IsNullOrEmpty(loginWithPersistDTO.InternalUrl))
            {
                url += $"&internalUrl={HttpUtility.UrlEncode(loginWithPersistDTO.InternalUrl)}";
            }

            return Redirect(url);


        }

        [HttpGet("redirectToLogin")]
        public ActionResult RedirectToLogin()
        {
            return Redirect(ApplicationConstants.LoginFrontendDefaultEndpoint);
        }

>>>>>>> Stashed changes
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


            var result = await signInManager.PasswordSignInAsync(user, loginDTO.Password, loginDTO.PersistLogin, true);
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
                if (loginDTO.PersistLogin)
                {
                    var principal = await signInManager.CreateUserPrincipalAsync(user);


                    var properties = new AuthenticationProperties
                    {
                        IsPersistent = true
                    };
                    await HttpContext.SignInAsync(ApplicationConstants.PersistLoginSchemeName, principal, properties);

                }

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

                    string url = $"{ApplicationConstants.ConfirmEmailFrontendDefaultEndpoint}?UserId={HttpUtility.UrlEncode(user.Id)}&Token={HttpUtility.UrlEncode(token)}";


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
        public async Task<ActionResult> ExternalLogin([FromQuery] ExternalLoginDTO externalLoginDTO)
        {

            externalLoginDTO.SecurityKeyHash = CryptographyUtils.Base64Decode(HttpUtility.UrlDecode(externalLoginDTO.SecurityKeyHash));
            if (!(await ExternalRedirectUrlExist(externalLoginDTO.returnUrl)))
            {
                return BadRequest("returnUrl no es un valor valido");
            }
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Account", new { externalLoginDTO.returnUrl });

            HttpContext.Response.Cookies.Append(ApplicationConstants.KeyHashCookieName, externalLoginDTO.SecurityKeyHash,

                     new Microsoft.AspNetCore.Http.CookieOptions()
                     {
                         HttpOnly = true,
                         MaxAge = TimeSpan.FromMinutes(10),
                         Secure = true,
                         SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Lax
                     });



            var properties = signInManager.ConfigureExternalAuthenticationProperties(externalLoginDTO.Provider, redirectUrl, externalLoginDTO.SecurityKeyHash);


            return Challenge(properties, externalLoginDTO.Provider);
        }

        private async Task<bool> ExternalRedirectUrlExist(string returnUrl) =>
            await applicationDbContext.ExternalLoginValidRedirectUrls.AnyAsync(url => url.Url == returnUrl);

        private async Task<bool> PersistentRedirectUrlExist(string returnUrl) =>
            await applicationDbContext.PersistentLoginValidRedirectUrls.AnyAsync(url => url.Url == returnUrl);





        [HttpPost("claimauthcode")]
        public async Task<ActionResult<TokenResponse>> ClaimAuthCode(AuthCodeClaimDTO authCodeClaimDTO)
        {
            authCodeClaimDTO.SecurityKey = CryptographyUtils.Base64Decode(authCodeClaimDTO.SecurityKey);

            var dbAuthCode = await applicationDbContext.UserAuthenticationCodes.FirstOrDefaultAsync(code => code.Token == authCodeClaimDTO.Token);

            if (dbAuthCode == null)
            {
                return BadRequest();
            }

            string md5Key = CryptographyUtils.ComputeSHA256Hash(authCodeClaimDTO.SecurityKey);

            if (dbAuthCode.SecurityKey.ToLower() != md5Key.ToLower())
            {
                return BadRequest();
            }


            var user = await applicationDbContext.Users.Where(x => x.Id == dbAuthCode.UserId).FirstOrDefaultAsync();

            string purpose = authCodeClaimDTO.Purpose == ApplicationConstants.ExternalLoginTokenPurposeName ?
                ApplicationConstants.ExternalLoginTokenPurposeName : authCodeClaimDTO.Purpose == ApplicationConstants.PersistentLoginTokenPurposeName ?
                ApplicationConstants.PersistentLoginTokenPurposeName : string.Empty;

            if (string.IsNullOrEmpty(purpose))
            {
                return BadRequest("Proposito incorrecto");
            }



            bool result = await userManager.VerifyUserTokenAsync(user, ApplicationConstants.AuthCodeTokenProviderName, purpose,
                authCodeClaimDTO.Token);

            if (!result)
            {
                return BadRequest();
            }


            return await BuildLoginToken(user.Email, authCodeClaimDTO.Purpose == ApplicationConstants.PersistentLoginTokenPurposeName);

        }

        [HttpGet("externallogincallback")]
        public async Task<ActionResult> ExternalLoginCallback([FromQuery] string returnUrl)
        {
            var info = await signInManager.GetExternalLoginInfoAsync();


            if (info == null || !HttpContext.Request.Cookies.ContainsKey(ApplicationConstants.KeyHashCookieName))
            {
                return Redirect($"{ApplicationConstants.LoginFrontendDefaultEndpoint}?msg=La session ha expirado por favor intentalo de nuevo");
            }


            string keyHash = HttpContext.Request.Cookies[ApplicationConstants.KeyHashCookieName];

            var result = await signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false);

            if (result.Succeeded)
            {

                var userIdentifier = info.Principal.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdentifier == null)
                {
                    return Redirect($"{ApplicationConstants.LoginFrontendDefaultEndpoint}?msg=Ha ocurrido un error inesperado");
                }


                var user = await userManager.FindByLoginAsync(info.LoginProvider, userIdentifier.Value);


                var token = await userManager.GenerateUserTokenAsync(user, ApplicationConstants.AuthCodeTokenProviderName, ApplicationConstants.ExternalLoginTokenPurposeName);


                var authenticationCode = new UserAuthenticationCode
                {
                    Expiration = DateTime.UtcNow.AddMinutes(5),
                    SecurityKey = keyHash,
                    Token = token,
                    UserId = user.Id
                };

                applicationDbContext.Add(authenticationCode);
                await applicationDbContext.SaveChangesAsync();

                string url = $"{returnUrl}?authcode={HttpUtility.UrlEncode(token)}";

                return Redirect(url);


            }
            else if (result.IsNotAllowed)
            {
                return Redirect($"{ApplicationConstants.LoginFrontendDefaultEndpoint}?msg=Tu cuenta no se encuentra activa aun, por favor verifica tu email");
            }
            else if (result.IsLockedOut)
            {
                return Redirect($"{ApplicationConstants.LoginFrontendDefaultEndpoint}?msg=Tu cuenta se encuentra bloqueada temporalmente intentalo de nuevo mas tarde");
            }
            else
            {

                //Register
                var email = info.Principal.FindFirst(ClaimTypes.Email);
                if (email == null)
                {
                    return Redirect($"{ApplicationConstants.LoginFrontendDefaultEndpoint}?msg=El proveedor de identidad no envio la informacion necesaria intentalo de nuevo");

                }
                var user = await userManager.FindByEmailAsync(email.Value);
                if (user != null)
                {
                    return Redirect($"{ApplicationConstants.LoginFrontendDefaultEndpoint}?msg=Un usuario con ese email ya esta registrado por favor inicia session y enlaza este metodo de autenticacion");
                }
                string name;
                var username = info.Principal.FindFirst(ClaimTypes.Name);
                if (username == null)
                {
                    return Redirect($"{ApplicationConstants.LoginFrontendDefaultEndpoint}?msg=El proveedor de identidad no envio la informacion necesaria intentalo de nuevo");
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

                    var extLoginAdditionResult = await userManager.AddLoginAsync(nUser, info);
                    if (!extLoginAdditionResult.Succeeded)
                    {
                        return Redirect($"{ApplicationConstants.LoginFrontendDefaultEndpoint}?msg=Ha ocurrido un error inesperado");
                    }

                    await SendEmailConfirmation(nUser);

                    return await ExternalLoginCallback(returnUrl);

                }
            }

            return Redirect($"{ApplicationConstants.LoginFrontendDefaultEndpoint}?msg=Ha ocurrido un error inesperado");
        }

        private async Task SendRecoverPasswordEmail(string Email)
        {
            var user = await userManager.FindByEmailAsync(Email);
            if (user != null)
            {
                var token = await userManager.GeneratePasswordResetTokenAsync(user);

                var currentUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";


                string url = $"{ApplicationConstants.PasswordRecoveryFrontendDefaultEndpoint}?UserId={HttpUtility.UrlEncode(user.Id)}&Token={HttpUtility.UrlEncode(token)}";



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
            //Basic claims
            var claims = new List<Claim>()
            {
               new Claim(JwtRegisteredClaimNames.UniqueName, userInfo.UserName),
               new Claim(ClaimTypes.Name, userInfo.UserName),
               new Claim(ClaimTypes.Email,userInfo.Email),
               new Claim(ClaimTypes.NameIdentifier,userInfo.UserId)
            };

            foreach (var rol in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, rol));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["auth:key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Si necesitamos mantener al usuario logueado generamos un token con 3 dias de antiguedad, 
            var expiration = DateTime.UtcNow.AddMinutes(60);

            JwtSecurityToken token = new JwtSecurityToken(
               issuer: null,
               audience: null,
               claims: claims,
               expires: expiration,
               signingCredentials: creds);

            return new TokenResponse()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration,
                IsPersistent = persistLogin
            };
        }

<<<<<<< Updated upstream
=======
        private async Task<ApplicationUser> GetUserFromContext()
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return null;

            }

            string email = HttpContext.User.Identity.Name;

            if (string.IsNullOrEmpty(email))
            {
                return null;
            }

            var user = await userManager.FindByNameAsync(email);

            return user;

        }

>>>>>>> Stashed changes


    }
}

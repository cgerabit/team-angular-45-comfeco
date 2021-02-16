using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using System.Threading.Tasks;

namespace BackendComfeco.Security
{
    public class AuthCodeTokenProvider<TUser>
        : DataProtectorTokenProvider<TUser> where TUser : class
    {
        public AuthCodeTokenProvider(IDataProtectionProvider dataProtectionProvider, IOptions<AuthCodeTokenProviderOptions> options, ILogger<DataProtectorTokenProvider<TUser>> logger) : base(dataProtectionProvider, options, logger)
        {


        }

        public override Task<bool> CanGenerateTwoFactorTokenAsync(UserManager<TUser> manager, TUser user)
        {
            return Task.FromResult(false);
        }


    }
}

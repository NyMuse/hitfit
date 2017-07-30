using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace hitfit.app.Models
{
    public class UserClaimsPrincipalFactory : IUserClaimsPrincipalFactory<User>
    {
        public IdentityOptions Options { get; }

        public UserClaimsPrincipalFactory(IOptions<IdentityOptions> optionsAccessor)
        {
            Options = optionsAccessor.Value;
        }

        public virtual Task<ClaimsPrincipal> CreateAsync(User user)
        {
            var identity = new ClaimsIdentity(Options.Cookies.ApplicationCookieAuthenticationScheme,
            Options.ClaimsIdentity.UserNameClaimType,
            Options.ClaimsIdentity.RoleClaimType);
            identity.AddClaims(new[]
            {
                new Claim(Options.ClaimsIdentity.UserIdClaimType, user.Id.ToString()),
                new Claim(Options.ClaimsIdentity.UserNameClaimType, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.UserName),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.IsAdministrator ? "admin" : "user"), 
                new Claim(ClaimTypes.GivenName, user.FirstName),
                new Claim(ClaimTypes.Surname, user.LastName),
                new Claim(ClaimTypes.Email, user.Email)
            });

            return Task.FromResult(new ClaimsPrincipal(identity));
        }
    }
}

using IdentityModel;
using IdentityServer.Models;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServer.Service
{
    public class ProfileServices : IProfileService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ProfileServices(
            UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<List<Claim>> GetClaimsFromUserAsync(ApplicationUser user)
        {
            var claims = new List<Claim> {
                new Claim(JwtClaimTypes.PreferredUserName, user.UserName)
            };

            var role = await _userManager.GetRolesAsync(user);
            role.ToList().ForEach(f =>
            {
                claims.Add(new Claim(JwtClaimTypes.Role, f));
            });

            return claims;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var subjectId = context.Subject.Claims.FirstOrDefault(c => c.Type == "sub").Value;
            var user = await _userManager.FindByIdAsync(subjectId);
            context.IssuedClaims = await GetClaimsFromUserAsync(user);
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var subjectId = context.Subject.Claims.FirstOrDefault(c => c.Type == "sub").Value;
            var user = await _userManager.FindByIdAsync(subjectId);
            context.IsActive = user != null;
        }
    }
}

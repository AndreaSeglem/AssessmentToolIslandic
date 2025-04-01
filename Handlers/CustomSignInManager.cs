using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace LetterKnowledgeAssessment.Handlers
{
    public class CustomSignInManager<TUser> : SignInManager<TUser> where TUser : class
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CustomSignInManager(
            UserManager<TUser> userManager,
            IHttpContextAccessor httpContextAccessor,
            IHttpContextAccessor contextAccessor,
            IUserClaimsPrincipalFactory<TUser> claimsFactory,
            IOptions<IdentityOptions> optionsAccessor,
            ILogger<SignInManager<TUser>> logger,
            IAuthenticationSchemeProvider schemes,
            IUserConfirmation<TUser> confirmation)
            : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes, confirmation)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public override async Task SignInAsync(TUser user, bool isPersistent, string authenticationMethod = null)
        {
            await base.SignInAsync(user, isPersistent, authenticationMethod);

            var context = _httpContextAccessor.HttpContext;
            if (context != null)
            {
                // Get culture from URL or cookie
                var userCulture = "no"; // Default
                if (context.Request.Query.ContainsKey("culture"))
                {
                    userCulture = context.Request.Query["culture"];
                }
                else if (context.Request.Cookies.ContainsKey(CookieRequestCultureProvider.DefaultCookieName))
                {
                    userCulture = CookieRequestCultureProvider.ParseCookieValue(context.Request.Cookies[CookieRequestCultureProvider.DefaultCookieName]).Cultures[0].Value;
                }

                // Set cultur-cookie again, so it is not overwritten
                context.Response.Cookies.Append(
                    CookieRequestCultureProvider.DefaultCookieName,
                    CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(userCulture)),
                    new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1), IsEssential = true }
                );
            }
        }
    }
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Net;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.WebUtilities;
using LetterKnowledgeAssessment.Models;
using LetterKnowledgeAssessment.Handlers;
using Microsoft.AspNetCore.Localization;


namespace LetterKnowledgeAssessment.Areas.Identity.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<Teacher> _signInManager;
        private readonly ILogger<LoginModel> _logger;

        public LoginModel(SignInManager<Teacher> signInManager, ILogger<LoginModel> logger)
        {
            _signInManager = signInManager;
            _logger = logger;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

   
        public string ErrorMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required(ErrorMessage = "Mandatory")]
            //[EmailAddress( ErrorMessage = "E-postadressen er ugyldig.")]
            public string Email { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required(ErrorMessage = "Mandatory")]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Display(Name = "RememberMe")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);


            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
{
    returnUrl ??= Url.Content("~/");

    var culture = Request.Query["culture"].FirstOrDefault()
      ?? Request.Cookies[CookieRequestCultureProvider.DefaultCookieName]?.Split('|')[0].Split('=')[1]
      ?? CultureInfo.CurrentCulture.TwoLetterISOLanguageName;

    // Apply the culture
    CultureInfo.CurrentCulture = new CultureInfo(culture);
    CultureInfo.CurrentUICulture = new CultureInfo(culture);

    Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName, $"c={culture}|uic={culture}", new CookieOptions
    {
        Expires = DateTimeOffset.UtcNow.AddYears(1),
        IsEssential = true
    });

    // Clean and update the ReturnUrl
    if (!string.IsNullOrEmpty(returnUrl))
    {
        returnUrl = System.Web.HttpUtility.UrlDecode(returnUrl); // Decode URL to avoid double encoding

        if (!returnUrl.Contains("culture="))
        {
            returnUrl = QueryHelpers.AddQueryString(returnUrl, "culture", culture);
        }
        else
        {
            returnUrl = UrlHelper.UpdateCultureInReturnUrl(returnUrl, culture, Request);
        }
    }

    ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

    if (ModelState.IsValid)
    {
        var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);
        
        if (result.Succeeded)
{
    // Clear the session to prevent old data from affecting culture
    HttpContext.Session.Clear();

    // Redirect explicitly to the homepage with the correct culture
    var redirectUrl = QueryHelpers.AddQueryString("/", "culture", culture);

    _logger.LogInformation($"User logged in successfully. Redirecting to: {redirectUrl}");
    return LocalRedirect(redirectUrl);
}

        if (result.RequiresTwoFactor)
        {
            return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
        }
        if (result.IsLockedOut)
        {
            _logger.LogWarning("User account locked out.");
            return RedirectToPage("./Lockout");
        }

        _logger.LogWarning($"Login failed for user: {Input.Email}");
        ErrorMessage = "Invalid login attempt.";
    }
    else
    {
        _logger.LogWarning("Login attempt failed due to invalid model state.");
        ErrorMessage = "Invalid login attempt.";
    }

    return Page();
}

    }
}

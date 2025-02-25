// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Net;
using System.Threading.Tasks;
using LetterKnowledgeAssessment.Models;
using LetterKnowledgeAssessment.Handlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.WebUtilities;
using System.Globalization;
using Microsoft.AspNetCore.Localization;


namespace LetterKnowledgeAssessment.Areas.Identity.Pages.Account
{
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<Teacher> _signInManager;

        public LogoutModel(SignInManager<Teacher> signInManager, ILogger<LogoutModel> logger)
        {
            _signInManager = signInManager;
        }

public async Task<IActionResult> OnPost(string returnUrl = null)
{
    await _signInManager.SignOutAsync();

    // Get the culture from query, cookie, or current culture
    var culture = Request.Query["culture"].FirstOrDefault() ??
                  Request.Cookies[CookieRequestCultureProvider.DefaultCookieName]?.Split('|')[0].Split('=')[1] ??
                  CultureInfo.CurrentCulture.TwoLetterISOLanguageName;

    // Fallback to Norwegian if no culture exists
    if (string.IsNullOrEmpty(culture))
    {
        culture = "no";
    }

    // Update the culture cookie to persist the selection
    Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName, $"c={culture}|uic={culture}", new CookieOptions
    {
        Expires = DateTimeOffset.UtcNow.AddYears(1),
        IsEssential = true
    });

    // Redirect handling
    if (!string.IsNullOrEmpty(returnUrl))
    {
        // Update ReturnUrl with the correct culture
        returnUrl = UrlHelper.UpdateCultureInReturnUrl(returnUrl, culture, Request);

        // Ensure culture is present in the returnUrl if missing
        if (!returnUrl.Contains("culture="))
        {
            returnUrl = QueryHelpers.AddQueryString(returnUrl, "culture", culture);
        }

        return LocalRedirect(returnUrl);
    }
    else
    {
        // Default redirect to the login page with culture preserved
        var defaultRedirect = QueryHelpers.AddQueryString("/Identity/Account/Login", "culture", culture);
        return LocalRedirect(defaultRedirect);
    }
}

    }
}

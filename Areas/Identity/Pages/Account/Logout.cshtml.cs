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

    var culture = Request.Query["culture"].FirstOrDefault() ??
                  Request.Cookies[CookieRequestCultureProvider.DefaultCookieName]?.Split('|')[0].Split('=')[1] ??
                  CultureInfo.CurrentCulture.TwoLetterISOLanguageName;

    if (string.IsNullOrEmpty(culture))
    {
        culture = "no"; // Standard norwegian if no culture exists
    }

    Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName, $"c={culture}|uic={culture}", new CookieOptions
    {
        Expires = DateTimeOffset.UtcNow.AddYears(1),
        IsEssential = true
    });

    if (!string.IsNullOrEmpty(returnUrl))
    {
        // Update ReturnUrl with new culture
        returnUrl = UrlHelper.UpdateCultureInReturnUrl(returnUrl, culture, Request);
        return LocalRedirect(returnUrl);
    }
    else
    {
        // Default URL with culture
        var defaultRedirect = QueryHelpers.AddQueryString("/Index", "culture", culture);
        return LocalRedirect(defaultRedirect);
    }
}
    }
}

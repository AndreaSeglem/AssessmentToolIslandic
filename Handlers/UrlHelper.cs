using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace LetterKnowledgeAssessment.Handlers
{
    public static class UrlHelper
    {
        public static string UpdateCultureInReturnUrl(string returnUrl, string newCulture, HttpRequest request)
        {
            returnUrl = WebUtility.UrlDecode(returnUrl);
            // Parse existing parameters
            var uriBuilder = new UriBuilder(new Uri(request.Scheme + "://" + request.Host + returnUrl));
            var query = QueryHelpers.ParseQuery(uriBuilder.Query);

            // Remove existing "culture" parameters
            var queryParams = query
                .Where(kvp => !string.Equals(kvp.Key, "culture", StringComparison.OrdinalIgnoreCase))
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value.ToString());

            // Add new culture
            queryParams["culture"] = newCulture;

            // Reconstruct URL
            uriBuilder.Query = QueryHelpers.AddQueryString("", queryParams);
            var updatedUrl = uriBuilder.Uri.PathAndQuery;

            return updatedUrl;
        }




    }
}

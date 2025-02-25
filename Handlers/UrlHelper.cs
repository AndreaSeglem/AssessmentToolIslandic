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

    if (string.IsNullOrWhiteSpace(returnUrl) || returnUrl == "/")
    {
        return QueryHelpers.AddQueryString("/", "culture", newCulture);
    }

    var uriBuilder = new UriBuilder(new Uri(request.Scheme + "://" + request.Host + returnUrl));
    var query = QueryHelpers.ParseQuery(uriBuilder.Query);

    var queryParams = query
        .Where(kvp => !string.Equals(kvp.Key, "culture", StringComparison.OrdinalIgnoreCase))
        .ToDictionary(kvp => kvp.Key, kvp => kvp.Value.ToString());

    queryParams["culture"] = newCulture;

    uriBuilder.Query = QueryHelpers.AddQueryString("", queryParams);
    return uriBuilder.Path + uriBuilder.Query;
}





    }
}

using System.Collections.Generic;
using System.Linq;

namespace Toast.Services;

public class UrlGenerator
{
    public string GenerateBaseUrl(string serviceUrl, string entityName)
    {
        return $"{serviceUrl}/{entityName}";
    }

    public string ApplyQueryOptions(string baseUrl, IDictionary<string, string> queryOptions)
    {
        var queryString = string.Join("&", queryOptions.Select(q => $"{q.Key}={q.Value}"));
        return $"{baseUrl}?{queryString}";
    }

    public string GenerateNestedFilterUrl(string baseUrl, string nestedFilter)
    {
        return $"{baseUrl}?$filter={nestedFilter}";
    }
}

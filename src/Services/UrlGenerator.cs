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

    public string GenerateFilterByIdUrl(string baseUrl, string id)
    {
        return $"{baseUrl}?$filter=id eq '{id}'";
    }

    public string GenerateFilterByNameUrl(string baseUrl, string name)
    {
        return $"{baseUrl}?$filter=name eq '{name}'";
    }

    public string GenerateFilterByDateOfBirthUrl(string baseUrl, string dob)
    {
        return $"{baseUrl}?$filter=dob eq {dob}";
    }

    public string GenerateFilterByDateRangeUrl(string baseUrl, string startDate, string endDate)
    {
        return $"{baseUrl}?$filter=dob ge {startDate} and dob le {endDate}";
    }

    public string GenerateSelectSpecificPropertiesUrl(string baseUrl, string properties)
    {
        return $"{baseUrl}?$select={properties}";
    }

    public string GenerateOrderByUrl(string baseUrl, string property)
    {
        return $"{baseUrl}?$orderby={property}";
    }

    public string GenerateOrderByDescendingUrl(string baseUrl, string property)
    {
        return $"{baseUrl}?$orderby={property} desc";
    }

    public string GenerateTopUrl(string baseUrl, int top)
    {
        return $"{baseUrl}?$top={top}";
    }

    public string GenerateSkipUrl(string baseUrl, int skip)
    {
        return $"{baseUrl}?$skip={skip}";
    }

    public string GenerateTopAfterSkipUrl(string baseUrl, int skip, int top)
    {
        return $"{baseUrl}?$skip={skip}&$top={top}";
    }

    public string GenerateFilterAndSelectUrl(string baseUrl, string filter, string select)
    {
        return $"{baseUrl}?$filter={filter}&$select={select}";
    }

    public string GenerateFilterOrderByAndTopUrl(string baseUrl, string filter, string orderBy, int top)
    {
        return $"{baseUrl}?$filter={filter}&$orderby={orderBy}&$top={top}";
    }

    public string GenerateSelectAndOrderByUrl(string baseUrl, string select, string orderBy)
    {
        return $"{baseUrl}?$select={select}&$orderby={orderBy}";
    }

    public string GenerateApplyAggregationsUrl(string baseUrl, string apply)
    {
        return $"{baseUrl}?$apply={apply}";
    }

    public string GenerateExpandUrl(string baseUrl, string expand)
    {
        return $"{baseUrl}?$expand={expand}";
    }

    public string GenerateExpandWithSelectUrl(string baseUrl, string expand, string select)
    {
        return $"{baseUrl}?$expand={expand}($select={select})";
    }

    public string GenerateExpandWithFilterUrl(string baseUrl, string expand, string filter)
    {
        return $"{baseUrl}?$expand={expand}($filter={filter})";
    }

    public string GenerateRecursiveExpandUrl(string baseUrl, string expand)
    {
        return $"{baseUrl}?$expand={expand}";
    }

    public string GenerateRecursiveExpandWithSelectUrl(string baseUrl, string expand, string select)
    {
        return $"{baseUrl}?$expand={expand}($select={select})";
    }

    public string GenerateNestedFilterWithCollectionUrl(string baseUrl, string collection, string filter)
    {
        return $"{baseUrl}?$filter={collection}/any(a: a/{filter})";
    }

    public string GenerateNestedFilterWithMultipleConditionsUrl(string baseUrl, string collection, string conditions)
    {
        return $"{baseUrl}?$filter={collection}/any(a: {conditions})";
    }
}

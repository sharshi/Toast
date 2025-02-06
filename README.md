# Toast

## OData URL Testing Suite

The OData URL Testing Suite is a C# library designed to help you test OData services by generating and executing OData URLs based on metadata and query options. The suite includes the following components:

- `MetadataService`: A service to fetch and parse the OData metadata document.
- `UrlGenerator`: A service to generate OData URLs based on the metadata and query options.
- `TestRunner`: A service to execute the generated URLs and validate the responses.

### Usage

#### MetadataService

```csharp
public class MetadataService
{
    public async Task<string> FetchMetadataAsync(string serviceUrl)
    {
        using (HttpClient client = new HttpClient())
        {
            string metadataUrl = $"{serviceUrl}/$metadata";
            return await client.GetStringAsync(metadataUrl);
        }
    }

    public Metadata ParseMetadata(string metadataXml)
    {
        // Parse the metadata XML and return a Metadata object
    }
}
```

#### UrlGenerator

```csharp
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
```

#### TestRunner

```csharp
public class TestRunner
{
    public async Task<HttpResponseMessage> ExecuteUrlAsync(string url)
    {
        using (HttpClient client = new HttpClient())
        {
            return await client.GetAsync(url);
        }
    }

    public bool ValidateResponse(HttpResponseMessage response)
    {
        // Validate the response based on expected criteria
        return response.IsSuccessStatusCode;
    }
}
```

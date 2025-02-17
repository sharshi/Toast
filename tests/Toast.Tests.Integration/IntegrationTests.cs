using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Toast.Services;

namespace Toast.Tests.Integration;

public class IntegrationTests
{
    private readonly string serviceUrl = "https://services.odata.org/TripPinRESTierService";

    [Fact]
    public async Task EndToEndTest()
    {
        // Arrange
        var metadataService = new MetadataService();
        var urlGenerator = new UrlGenerator();
        var testRunner = new TestRunner();

        // Act
        var metadataXml = await metadataService.FetchMetadataAsync(serviceUrl);
        var metadata = metadataService.ParseMetadata(metadataXml);

        var baseUrl = urlGenerator.GenerateBaseUrl(serviceUrl, "People");
        var queryOptions = new Dictionary<string, string>
        {
            { "$filter", "Name eq 'John Doe'" },
            { "$select", "ID,Name" }
        };
        var url = urlGenerator.ApplyQueryOptions(baseUrl, queryOptions);

        var response = await testRunner.ExecuteUrlAsync(url);
        var isValid = testRunner.ValidateResponse(response);

        // Assert
        Assert.True(isValid);
    }

    [Fact]
    public async Task ComprehensiveIntegrationTest()
    {
        // Arrange
        var metadataService = new MetadataService();
        var urlGenerator = new UrlGenerator();
        var testRunner = new TestRunner();

        // Act
        var metadataXml = await metadataService.FetchMetadataAsync(serviceUrl);
        var metadata = metadataService.ParseMetadata(metadataXml);

        var baseUrl = urlGenerator.GenerateBaseUrl(serviceUrl, "People");

        // Test various query options
        var queryOptionsList = new List<Dictionary<string, string>>
        {
            new Dictionary<string, string> { { "$filter", "Name eq 'John Doe'" } },
            new Dictionary<string, string> { { "$select", "ID,Name" } },
            new Dictionary<string, string> { { "$orderby", "Name" } },
            new Dictionary<string, string> { { "$top", "10" } },
            new Dictionary<string, string> { { "$skip", "5" } },
            new Dictionary<string, string> { { "$filter", "Name eq 'John Doe'" }, { "$select", "ID,Name" } },
            new Dictionary<string, string> { { "$filter", "Name eq 'John Doe'" }, { "$orderby", "Name" }, { "$top", "5" } },
            new Dictionary<string, string> { { "$select", "Name,DateOfBirth" }, { "$orderby", "DateOfBirth desc" } },
            new Dictionary<string, string> { { "$expand", "Address" } },
            new Dictionary<string, string> { { "$expand", "Address($select=City,State)" } },
            new Dictionary<string, string> { { "$expand", "Address($filter=City eq 'New York')" } },
            new Dictionary<string, string> { { "$expand", "Address($expand=Country)" } },
            new Dictionary<string, string> { { "$expand", "Address($select=City;$expand=Country($select=Name))" } },
            new Dictionary<string, string> { { "$filter", "Address/City eq 'New York' and DateOfBirth ge 1990-01-01T00:00:00Z" } },
            new Dictionary<string, string> { { "$filter", "Addresses/any(a: a/City eq 'New York')" } },
            new Dictionary<string, string> { { "$filter", "Addresses/any(a: a/City eq 'New York' and a/State eq 'NY')" } }
        };

        foreach (var queryOptions in queryOptionsList)
        {
            var url = urlGenerator.ApplyQueryOptions(baseUrl, queryOptions);
            var response = await testRunner.ExecuteUrlAsync(url);
            var isValid = testRunner.ValidateResponse(response);

            // Assert
            Assert.True(isValid);
        }
    }
}

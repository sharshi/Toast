using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Toast.Services;
using Toast.Models;

namespace Toast.Tests
{
    public class IntegrationTests
    {
        private readonly string serviceUrl = "http://example.com/odata";

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

            var baseUrl = urlGenerator.GenerateBaseUrl(serviceUrl, metadata.Entities[0].Name);
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
    }
}

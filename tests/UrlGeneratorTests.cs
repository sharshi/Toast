using System.Collections.Generic;
using Xunit;
using Toast.Services;

namespace Toast.Tests
{
    public class UrlGeneratorTests
    {
        [Fact]
        public void GenerateBaseUrl_ReturnsCorrectUrl()
        {
            // Arrange
            var serviceUrl = "http://example.com/odata";
            var entityName = "Persons";
            var urlGenerator = new UrlGenerator();

            // Act
            var baseUrl = urlGenerator.GenerateBaseUrl(serviceUrl, entityName);

            // Assert
            Assert.Equal("http://example.com/odata/Persons", baseUrl);
        }

        [Fact]
        public void ApplyQueryOptions_ReturnsCorrectUrl()
        {
            // Arrange
            var baseUrl = "http://example.com/odata/Persons";
            var queryOptions = new Dictionary<string, string>
            {
                { "$filter", "name eq 'John Doe'" },
                { "$select", "id,name" }
            };
            var urlGenerator = new UrlGenerator();

            // Act
            var urlWithQueryOptions = urlGenerator.ApplyQueryOptions(baseUrl, queryOptions);

            // Assert
            Assert.Equal("http://example.com/odata/Persons?$filter=name eq 'John Doe'&$select=id,name", urlWithQueryOptions);
        }

        [Fact]
        public void GenerateNestedFilterUrl_ReturnsCorrectUrl()
        {
            // Arrange
            var baseUrl = "http://example.com/odata/Persons";
            var nestedFilter = "Address/City eq 'New York'";
            var urlGenerator = new UrlGenerator();

            // Act
            var urlWithNestedFilter = urlGenerator.GenerateNestedFilterUrl(baseUrl, nestedFilter);

            // Assert
            Assert.Equal("http://example.com/odata/Persons?$filter=Address/City eq 'New York'", urlWithNestedFilter);
        }
    }
}

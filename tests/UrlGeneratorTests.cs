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

        [Fact]
        public void GenerateFilterByIdUrl_ReturnsCorrectUrl()
        {
            // Arrange
            var baseUrl = "http://example.com/odata/Persons";
            var id = "123";
            var urlGenerator = new UrlGenerator();

            // Act
            var url = urlGenerator.GenerateFilterByIdUrl(baseUrl, id);

            // Assert
            Assert.Equal("http://example.com/odata/Persons?$filter=id eq '123'", url);
        }

        [Fact]
        public void GenerateFilterByNameUrl_ReturnsCorrectUrl()
        {
            // Arrange
            var baseUrl = "http://example.com/odata/Persons";
            var name = "John Doe";
            var urlGenerator = new UrlGenerator();

            // Act
            var url = urlGenerator.GenerateFilterByNameUrl(baseUrl, name);

            // Assert
            Assert.Equal("http://example.com/odata/Persons?$filter=name eq 'John Doe'", url);
        }

        [Fact]
        public void GenerateFilterByDateOfBirthUrl_ReturnsCorrectUrl()
        {
            // Arrange
            var baseUrl = "http://example.com/odata/Persons";
            var dob = "1990-01-01T00:00:00Z";
            var urlGenerator = new UrlGenerator();

            // Act
            var url = urlGenerator.GenerateFilterByDateOfBirthUrl(baseUrl, dob);

            // Assert
            Assert.Equal("http://example.com/odata/Persons?$filter=dob eq 1990-01-01T00:00:00Z", url);
        }

        [Fact]
        public void GenerateFilterByDateRangeUrl_ReturnsCorrectUrl()
        {
            // Arrange
            var baseUrl = "http://example.com/odata/Persons";
            var startDate = "1990-01-01T00:00:00Z";
            var endDate = "1999-12-31T23:59:59Z";
            var urlGenerator = new UrlGenerator();

            // Act
            var url = urlGenerator.GenerateFilterByDateRangeUrl(baseUrl, startDate, endDate);

            // Assert
            Assert.Equal("http://example.com/odata/Persons?$filter=dob ge 1990-01-01T00:00:00Z and dob le 1999-12-31T23:59:59Z", url);
        }

        [Fact]
        public void GenerateSelectSpecificPropertiesUrl_ReturnsCorrectUrl()
        {
            // Arrange
            var baseUrl = "http://example.com/odata/Persons";
            var properties = "id,name";
            var urlGenerator = new UrlGenerator();

            // Act
            var url = urlGenerator.GenerateSelectSpecificPropertiesUrl(baseUrl, properties);

            // Assert
            Assert.Equal("http://example.com/odata/Persons?$select=id,name", url);
        }

        [Fact]
        public void GenerateOrderByUrl_ReturnsCorrectUrl()
        {
            // Arrange
            var baseUrl = "http://example.com/odata/Persons";
            var property = "name";
            var urlGenerator = new UrlGenerator();

            // Act
            var url = urlGenerator.GenerateOrderByUrl(baseUrl, property);

            // Assert
            Assert.Equal("http://example.com/odata/Persons?$orderby=name", url);
        }

        [Fact]
        public void GenerateOrderByDescendingUrl_ReturnsCorrectUrl()
        {
            // Arrange
            var baseUrl = "http://example.com/odata/Persons";
            var property = "name";
            var urlGenerator = new UrlGenerator();

            // Act
            var url = urlGenerator.GenerateOrderByDescendingUrl(baseUrl, property);

            // Assert
            Assert.Equal("http://example.com/odata/Persons?$orderby=name desc", url);
        }

        [Fact]
        public void GenerateTopUrl_ReturnsCorrectUrl()
        {
            // Arrange
            var baseUrl = "http://example.com/odata/Persons";
            var top = 10;
            var urlGenerator = new UrlGenerator();

            // Act
            var url = urlGenerator.GenerateTopUrl(baseUrl, top);

            // Assert
            Assert.Equal("http://example.com/odata/Persons?$top=10", url);
        }

        [Fact]
        public void GenerateSkipUrl_ReturnsCorrectUrl()
        {
            // Arrange
            var baseUrl = "http://example.com/odata/Persons";
            var skip = 5;
            var urlGenerator = new UrlGenerator();

            // Act
            var url = urlGenerator.GenerateSkipUrl(baseUrl, skip);

            // Assert
            Assert.Equal("http://example.com/odata/Persons?$skip=5", url);
        }

        [Fact]
        public void GenerateTopAfterSkipUrl_ReturnsCorrectUrl()
        {
            // Arrange
            var baseUrl = "http://example.com/odata/Persons";
            var skip = 5;
            var top = 10;
            var urlGenerator = new UrlGenerator();

            // Act
            var url = urlGenerator.GenerateTopAfterSkipUrl(baseUrl, skip, top);

            // Assert
            Assert.Equal("http://example.com/odata/Persons?$skip=5&$top=10", url);
        }

        [Fact]
        public void GenerateFilterAndSelectUrl_ReturnsCorrectUrl()
        {
            // Arrange
            var baseUrl = "http://example.com/odata/Persons";
            var filter = "name eq 'John Doe'";
            var select = "id,dob";
            var urlGenerator = new UrlGenerator();

            // Act
            var url = urlGenerator.GenerateFilterAndSelectUrl(baseUrl, filter, select);

            // Assert
            Assert.Equal("http://example.com/odata/Persons?$filter=name eq 'John Doe'&$select=id,dob", url);
        }

        [Fact]
        public void GenerateFilterOrderByAndTopUrl_ReturnsCorrectUrl()
        {
            // Arrange
            var baseUrl = "http://example.com/odata/Persons";
            var filter = "dob ge 1990-01-01T00:00:00Z";
            var orderBy = "name";
            var top = 5;
            var urlGenerator = new UrlGenerator();

            // Act
            var url = urlGenerator.GenerateFilterOrderByAndTopUrl(baseUrl, filter, orderBy, top);

            // Assert
            Assert.Equal("http://example.com/odata/Persons?$filter=dob ge 1990-01-01T00:00:00Z&$orderby=name&$top=5", url);
        }

        [Fact]
        public void GenerateSelectAndOrderByUrl_ReturnsCorrectUrl()
        {
            // Arrange
            var baseUrl = "http://example.com/odata/Persons";
            var select = "name,dob";
            var orderBy = "dob desc";
            var urlGenerator = new UrlGenerator();

            // Act
            var url = urlGenerator.GenerateSelectAndOrderByUrl(baseUrl, select, orderBy);

            // Assert
            Assert.Equal("http://example.com/odata/Persons?$select=name,dob&$orderby=dob desc", url);
        }

        [Fact]
        public void GenerateApplyAggregationsUrl_ReturnsCorrectUrl()
        {
            // Arrange
            var baseUrl = "http://example.com/odata/Persons";
            var apply = "groupby((name),aggregate(dob with min as MinDob))";
            var urlGenerator = new UrlGenerator();

            // Act
            var url = urlGenerator.GenerateApplyAggregationsUrl(baseUrl, apply);

            // Assert
            Assert.Equal("http://example.com/odata/Persons?$apply=groupby((name),aggregate(dob with min as MinDob))", url);
        }

        [Fact]
        public void GenerateExpandUrl_ReturnsCorrectUrl()
        {
            // Arrange
            var baseUrl = "http://example.com/odata/Persons";
            var expand = "Address";
            var urlGenerator = new UrlGenerator();

            // Act
            var url = urlGenerator.GenerateExpandUrl(baseUrl, expand);

            // Assert
            Assert.Equal("http://example.com/odata/Persons?$expand=Address", url);
        }

        [Fact]
        public void GenerateExpandWithSelectUrl_ReturnsCorrectUrl()
        {
            // Arrange
            var baseUrl = "http://example.com/odata/Persons";
            var expand = "Address";
            var select = "city,state";
            var urlGenerator = new UrlGenerator();

            // Act
            var url = urlGenerator.GenerateExpandWithSelectUrl(baseUrl, expand, select);

            // Assert
            Assert.Equal("http://example.com/odata/Persons?$expand=Address($select=city,state)", url);
        }

        [Fact]
        public void GenerateExpandWithFilterUrl_ReturnsCorrectUrl()
        {
            // Arrange
            var baseUrl = "http://example.com/odata/Persons";
            var expand = "Address";
            var filter = "city eq 'New York'";
            var urlGenerator = new UrlGenerator();

            // Act
            var url = urlGenerator.GenerateExpandWithFilterUrl(baseUrl, expand, filter);

            // Assert
            Assert.Equal("http://example.com/odata/Persons?$expand=Address($filter=city eq 'New York')", url);
        }

        [Fact]
        public void GenerateRecursiveExpandUrl_ReturnsCorrectUrl()
        {
            // Arrange
            var baseUrl = "http://example.com/odata/Persons";
            var expand = "Address($expand=Country)";
            var urlGenerator = new UrlGenerator();

            // Act
            var url = urlGenerator.GenerateRecursiveExpandUrl(baseUrl, expand);

            // Assert
            Assert.Equal("http://example.com/odata/Persons?$expand=Address($expand=Country)", url);
        }

        [Fact]
        public void GenerateRecursiveExpandWithSelectUrl_ReturnsCorrectUrl()
        {
            // Arrange
            var baseUrl = "http://example.com/odata/Persons";
            var expand = "Address($select=city;$expand=Country($select=name))";
            var urlGenerator = new UrlGenerator();

            // Act
            var url = urlGenerator.GenerateRecursiveExpandWithSelectUrl(baseUrl, expand);

            // Assert
            Assert.Equal("http://example.com/odata/Persons?$expand=Address($select=city;$expand=Country($select=name))", url);
        }

        [Fact]
        public void GenerateNestedFilterWithCollectionUrl_ReturnsCorrectUrl()
        {
            // Arrange
            var baseUrl = "http://example.com/odata/Persons";
            var collection = "Addresses";
            var filter = "City eq 'New York'";
            var urlGenerator = new UrlGenerator();

            // Act
            var url = urlGenerator.GenerateNestedFilterWithCollectionUrl(baseUrl, collection, filter);

            // Assert
            Assert.Equal("http://example.com/odata/Persons?$filter=Addresses/any(a: a/City eq 'New York')", url);
        }

        [Fact]
        public void GenerateNestedFilterWithMultipleConditionsUrl_ReturnsCorrectUrl()
        {
            // Arrange
            var baseUrl = "http://example.com/odata/Persons";
            var collection = "Addresses";
            var conditions = "a/City eq 'New York' and a/State eq 'NY'";
            var urlGenerator = new UrlGenerator();

            // Act
            var url = urlGenerator.GenerateNestedFilterWithMultipleConditionsUrl(baseUrl, collection, conditions);

            // Assert
            Assert.Equal("http://example.com/odata/Persons?$filter=Addresses/any(a: a/City eq 'New York' and a/State eq 'NY')", url);
        }
    }
}

using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Toast.Services;
using Toast.Models;
using System.Collections.Generic;

namespace Toast.Tests
{
    public class MetadataServiceTests
    {
        [Fact]
        public async Task FetchMetadataAsync_ReturnsMetadata()
        {
            // Arrange
            var serviceUrl = "http://example.com/odata";
            var metadataService = new MetadataService();

            // Act
            var metadataXml = await metadataService.FetchMetadataAsync(serviceUrl);

            // Assert
            Assert.NotNull(metadataXml);
            Assert.Contains("<edmx:Edmx", metadataXml);
        }

        [Fact]
        public void ParseMetadata_ReturnsMetadataObject()
        {
            // Arrange
            var metadataXml = @"
                <edmx:Edmx xmlns:edmx='http://docs.oasis-open.org/odata/ns/edmx'>
                    <edmx:DataServices>
                        <Schema xmlns='http://docs.oasis-open.org/odata/ns/edm'>
                            <EntityType Name='Person'>
                                <Key>
                                    <PropertyRef Name='ID' />
                                </Key>
                                <Property Name='ID' Type='Edm.String' Nullable='false' />
                                <Property Name='Name' Type='Edm.String' />
                                <NavigationProperty Name='Address' Type='Namespace.Address' />
                            </EntityType>
                        </Schema>
                    </edmx:DataServices>
                </edmx:Edmx>";
            var metadataService = new MetadataService();

            // Act
            var metadata = metadataService.ParseMetadata(metadataXml);

            // Assert
            Assert.NotNull(metadata);
            Assert.Single(metadata.Entities);
            Assert.Equal("Person", metadata.Entities[0].Name);
            Assert.Equal(2, metadata.Entities[0].Properties.Count);
            Assert.Single(metadata.Entities[0].NavigationProperties);
        }

        [Fact]
        public void ExtractQueryOptions_ReturnsQueryOptions()
        {
            // Arrange
            var metadataXml = @"
                <edmx:Edmx xmlns:edmx='http://docs.oasis-open.org/odata/ns/edmx'>
                    <edmx:DataServices>
                        <Schema xmlns='http://docs.oasis-open.org/odata/ns/edm'>
                            <EntityType Name='Person'>
                                <Key>
                                    <PropertyRef Name='ID' />
                                </Key>
                                <Property Name='ID' Type='Edm.String' Nullable='false' />
                                <Property Name='Name' Type='Edm.String' />
                                <NavigationProperty Name='Address' Type='Namespace.Address' />
                            </EntityType>
                        </Schema>
                    </edmx:DataServices>
                </edmx:Edmx>";
            var metadataService = new MetadataService();

            // Act
            var queryOptions = metadataService.ExtractQueryOptions(metadataXml);

            // Assert
            Assert.NotNull(queryOptions);
            Assert.Contains("$filter=ID eq 'value'", queryOptions);
            Assert.Contains("$select=ID", queryOptions);
            Assert.Contains("$orderby=ID", queryOptions);
            Assert.Contains("$filter=Name eq 'value'", queryOptions);
            Assert.Contains("$select=Name", queryOptions);
            Assert.Contains("$orderby=Name", queryOptions);
        }

        [Fact]
        public void ValidateMetadata_ThrowsExceptionForInvalidMetadata()
        {
            // Arrange
            var invalidMetadataXml = "<invalid></invalid>";
            var metadataService = new MetadataService();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => metadataService.ParseMetadata(invalidMetadataXml));
        }

        [Fact]
        public async Task FetchMetadataAsync_ThrowsExceptionForInvalidMetadata()
        {
            // Arrange
            var serviceUrl = "http://example.com/invalid-odata";
            var metadataService = new MetadataService();

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await metadataService.FetchMetadataAsync(serviceUrl));
        }

        [Fact]
        public void AutogenerateUrlsFromMetadata_ReturnsGeneratedUrls()
        {
            // Arrange
            var metadataXml = @"
                <edmx:Edmx xmlns:edmx='http://docs.oasis-open.org/odata/ns/edmx'>
                    <edmx:DataServices>
                        <Schema xmlns='http://docs.oasis-open.org/odata/ns/edm'>
                            <EntityType Name='Person'>
                                <Key>
                                    <PropertyRef Name='ID' />
                                </Key>
                                <Property Name='ID' Type='Edm.String' Nullable='false' />
                                <Property Name='Name' Type='Edm.String' />
                                <NavigationProperty Name='Address' Type='Namespace.Address' />
                            </EntityType>
                        </Schema>
                    </edmx:DataServices>
                </edmx:Edmx>";
            var metadataService = new MetadataService();

            // Act
            var urls = metadataService.AutogenerateUrlsFromMetadata(metadataXml);

            // Assert
            Assert.NotNull(urls);
            Assert.Contains("http://example.com/odata/Person?$filter=ID eq 'value'", urls);
            Assert.Contains("http://example.com/odata/Person?$select=ID", urls);
            Assert.Contains("http://example.com/odata/Person?$orderby=ID", urls);
            Assert.Contains("http://example.com/odata/Person?$filter=Name eq 'value'", urls);
            Assert.Contains("http://example.com/odata/Person?$select=Name", urls);
            Assert.Contains("http://example.com/odata/Person?$orderby=Name", urls);
        }
    }
}

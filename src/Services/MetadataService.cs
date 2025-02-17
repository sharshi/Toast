using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using Toast.Models;
using System.Collections.Generic;
using System.Linq;

namespace Toast.Services;

public class MetadataService
{
    public async Task<string> FetchMetadataAsync(string serviceUrl)
    {
        using (HttpClient client = new HttpClient())
        {
            string metadataUrl = $"{serviceUrl}/$metadata";
            var metadataXml = await client.GetStringAsync(metadataUrl);
            ValidateMetadata(metadataXml);
            return metadataXml;
        }
    }

    public Metadata ParseMetadata(string metadataXml)
    {
        ValidateMetadata(metadataXml);

        var metadata = new Metadata();
        metadata.Entities = new List<Entity>();
        var xDocument = XDocument.Parse(metadataXml);

        // Parse entities
        foreach (var entityElement in xDocument.Descendants("{http://docs.oasis-open.org/odata/ns/edm}EntityType"))
        {
            var entity = new Entity
            {
                Name = entityElement.Attribute("Name").Value,
                Properties = new List<Property>(),
                NavigationProperties = new List<NavigationProperty>()
            };

            // Parse properties
            foreach (var propertyElement in entityElement.Elements("{http://docs.oasis-open.org/odata/ns/edm}Property"))
            {
                var property = new Property
                {
                    Name = propertyElement.Attribute("Name").Value,
                    Type = propertyElement.Attribute("Type").Value
                };
                entity.Properties.Add(property);
            }

            // Parse navigation properties
            foreach (var navPropertyElement in entityElement.Elements("{http://docs.oasis-open.org/odata/ns/edm}NavigationProperty"))
            {
                var navProperty = new NavigationProperty
                {
                    Name = navPropertyElement.Attribute("Name").Value,
                    Type = navPropertyElement.Attribute("Type").Value
                };
                entity.NavigationProperties.Add(navProperty);
            }

            metadata.Entities.Add(entity);
        }

        // Parse entities
        foreach (var entityElement in xDocument.Descendants("{http://docs.oasis-open.org/odata/ns/edm}ComplexType"))
        {
            var entity = new Entity
            {
                Name = entityElement.Attribute("Name").Value,
                Properties = new List<Property>(),
                NavigationProperties = new List<NavigationProperty>()
            };

            // Parse properties
            foreach (var propertyElement in entityElement.Elements("{http://docs.oasis-open.org/odata/ns/edm}Property"))
            {
                var property = new Property
                {
                    Name = propertyElement.Attribute("Name").Value,
                    Type = propertyElement.Attribute("Type").Value
                };
                entity.Properties.Add(property);
            }

            // Parse navigation properties
            foreach (var navPropertyElement in entityElement.Elements("{http://docs.oasis-open.org/odata/ns/edm}NavigationProperty"))
            {
                var navProperty = new NavigationProperty
                {
                    Name = navPropertyElement.Attribute("Name").Value,
                    Type = navPropertyElement.Attribute("Type").Value
                };
                entity.NavigationProperties.Add(navProperty);
            }

            metadata.Entities.Add(entity);
        }

        // Parse entityContainser.entityset
        foreach (var entityContainerElement in xDocument.Descendants("{http://docs.oasis-open.org/odata/ns/edm}EntityContainer"))
        {
            foreach (var entitySetElement in entityContainerElement.Elements("{http://docs.oasis-open.org/odata/ns/edm}EntitySet"))
            {
                var entitySet = new Entity
                {
                    Name = entitySetElement.Attribute("Name").Value,
                    Properties = entitySetElement.Attributes().Select(a => new Property { Name = a.Name.LocalName, Type = a.Value }).ToList(),
                };

                // Parse navigation property bindings
                foreach (var navPropertyBindingElement in entitySetElement.Elements("{http://docs.oasis-open.org/odata/ns/edm}NavigationPropertyBinding"))
                {
                    var navPropertyBinding = new NavigationPropertyBinding
                    {
                        Path = navPropertyBindingElement.Attribute("Path").Value,
                        Target = navPropertyBindingElement.Attribute("Target").Value
                    };
                    entitySet.NavigationPropertyBindings.Add(navPropertyBinding);
                }

                metadata.Relationships.Add(entitySet);
            }
        }

        return metadata;
    }

    public List<string> ExtractQueryOptions(string metadataXml)
    {
        var queryOptions = new List<string>();
        var xDocument = XDocument.Parse(metadataXml);

        // Extract query options from the EDM XML
        foreach (var entityElement in xDocument.Descendants("{http://docs.oasis-open.org/odata/ns/edm}EntityType"))
        {
            foreach (var propertyElement in entityElement.Elements("{http://docs.oasis-open.org/odata/ns/edm}Property"))
            {
                var propertyName = propertyElement.Attribute("Name").Value;
                queryOptions.Add($"$filter={propertyName} eq 'value'");
                queryOptions.Add($"$select={propertyName}");
                queryOptions.Add($"$orderby={propertyName}");
            }
        }

        return queryOptions;
    }

    private void ValidateMetadata(string metadataXml)
    {
        if (string.IsNullOrEmpty(metadataXml))
        {
            throw new ArgumentException("Metadata XML cannot be null or empty.");
        }

        var xDocument = XDocument.Parse(metadataXml);
        if (!xDocument.Descendants("{http://docs.oasis-open.org/odata/ns/edm}EntityType").Any())
        {
            throw new ArgumentException("Metadata XML does not contain any EntityType elements.");
        }
    }

    public List<string> AutogenerateUrlsFromMetadata(string metadataXml)
    {
        var urls = new List<string>();
        var xDocument = XDocument.Parse(metadataXml);

        foreach (var entityElement in xDocument.Descendants("{http://docs.oasis-open.org/odata/ns/edm}EntityType"))
        {
            var entityName = entityElement.Attribute("Name").Value;
            var baseUrl = $"http://example.com/odata/{entityName}";

            // Generate URLs for each query option
            foreach (var propertyElement in entityElement.Elements("{http://docs.oasis-open.org/odata/ns/edm}Property"))
            {
                var propertyName = propertyElement.Attribute("Name").Value;
                urls.Add($"{baseUrl}?$filter={propertyName} eq 'value'");
                urls.Add($"{baseUrl}?$select={propertyName}");
                urls.Add($"{baseUrl}?$orderby={propertyName}");
            }
        }

        return urls;
    }
}

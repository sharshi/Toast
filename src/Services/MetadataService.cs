using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using Toast.Models;
using System.Collections.Generic;
using System.Linq;

namespace Toast.Services
{
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
            var metadata = new Metadata();
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

            // Parse relationships
            foreach (var associationElement in xDocument.Descendants("{http://docs.oasis-open.org/odata/ns/edm}Association"))
            {
                var relationship = new Relationship
                {
                    Name = associationElement.Attribute("Name").Value,
                    FromEntity = associationElement.Element("{http://docs.oasis-open.org/odata/ns/edm}End").Attribute("Type").Value,
                    ToEntity = associationElement.Elements("{http://docs.oasis-open.org/odata/ns/edm}End").Last().Attribute("Type").Value
                };
                metadata.Relationships.Add(relationship);
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
    }
}

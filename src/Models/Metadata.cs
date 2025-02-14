using System.Collections.Generic;

namespace Toast.Models;

public class Metadata
{
    public List<Entity> Entities { get; set; }
    public List<Property> Properties { get; set; }
    public List<Relationship> Relationships { get; set; }
}

public class Entity
{
    public string Name { get; set; }
    public List<Property> Properties { get; set; }
    public List<NavigationProperty> NavigationProperties { get; set; }
}

public class Property
{
    public string Name { get; set; }
    public string Type { get; set; }
}

public class NavigationProperty
{
    public string Name { get; set; }
    public string Type { get; set; }
}

public class Relationship
{
    public string Name { get; set; }
    public string FromEntity { get; set; }
    public string ToEntity { get; set; }
}

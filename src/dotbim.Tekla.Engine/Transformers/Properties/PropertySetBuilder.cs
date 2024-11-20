using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace dotbimTekla.Engine.Transformers.Properties;
public class PropertySetBuilder
{
    private readonly XmlFileSerializer _xmlFileSerializer;

    public PropertySetBuilder()
    {
        _xmlFileSerializer = new();
    }

    public IfcPropertiesDictionary? GetNeededProperties(string filePath)
    {
        var propertySetConfiguration = _xmlFileSerializer.ReadFile(filePath);
        if (propertySetConfiguration is null)
            return null;

        var dictionary = new Dictionary<IncludeEntityType, List<IfcProperties>>();
        foreach (var propertySet in propertySetConfiguration.PropertySetDefinitions.Where(d => !d.isIgnored))
        {
            var bindings = propertySetConfiguration.PropertySetBindings.FirstOrDefault(b => b.referenceId == propertySet.referenceId); ;
            if (bindings is null)
                continue;

            var entityTypes = bindings.Rules.Select(r => r.entityType).ToList();
            var properties = propertySet.Properties.Property.OfType<PropertySingleValueType>()
                .Where(p => !p.isIgnored).ToList();

            var ifcProperties = new IfcProperties(propertySet.Name, properties);
            foreach (var entityType in entityTypes)
            {
                if (dictionary.ContainsKey(entityType))
                    dictionary[entityType].Add(ifcProperties);
                else
                    dictionary[entityType] = new List<IfcProperties>() { ifcProperties };
            }
        }

        return new IfcPropertiesDictionary(dictionary);
    }
}

public class IfcPropertiesDictionary
{
    private readonly Dictionary<IncludeEntityType, List<IfcProperties>> _dictionary;

    public IfcPropertiesDictionary(Dictionary<IncludeEntityType, List<IfcProperties>> dictionary)
    {
        _dictionary = dictionary;
    }
}



public record IfcProperties(string PSetName, IReadOnlyList<PropertySingleValueType> Properties);
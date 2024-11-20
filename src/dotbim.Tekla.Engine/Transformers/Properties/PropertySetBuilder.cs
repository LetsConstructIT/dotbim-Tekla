using dotbimTekla.Engine.Transformers.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace dotbimTekla.Engine.Exporters.Properties;
public class PropertySetBuilder
{
    private readonly XmlFileSerializer _xmlFileSerializer;
    private readonly PropertySingleFactory _propertySingleFactory;
    public PropertySetBuilder()
    {
        _xmlFileSerializer = new();
        _propertySingleFactory = new();
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
                .Where(p => !p.isIgnored)
                .Select(_propertySingleFactory.Construct)
                .ToList();

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
    private readonly Dictionary<IncludeEntityType, EntityQueryScope> _dictionary;

    public IfcPropertiesDictionary(Dictionary<IncludeEntityType, List<IfcProperties>> dictionary)
    {
        _dictionary = TransformRawDictionary(dictionary);
    }

    private Dictionary<IncludeEntityType, EntityQueryScope> TransformRawDictionary(Dictionary<IncludeEntityType, List<IfcProperties>> dictionary)
    {
        var result = new Dictionary<IncludeEntityType, EntityQueryScope>();

        foreach (var keyValue in dictionary)
        {
            result[keyValue.Key] = Transform(keyValue.Value);
        }
        return result;
    }

    private EntityQueryScope Transform(List<IfcProperties> properties)
    {
        var udas = properties.SelectMany(p => p.Properties.Where(r => r.ParameterType == ParameterType.Uda));
        var templates = properties.SelectMany(p => p.Properties.Where(r => r.ParameterType == ParameterType.Template));

        return new EntityQueryScope(properties, GetParameters(templates), GetParameters(udas));
    }

    private QueryParameters GetParameters(IEnumerable<PropertySingle> properties)
    {
        var stringNames = new ArrayList();
        var doubleNames = new ArrayList();
        var integerNames = new ArrayList();
        foreach (var item in properties.GroupBy(p => p.ParameterValueType))
        {
            switch (item.Key)
            {
                case ParameterValueType.String:
                    AddRange(stringNames, item.Select(i => i.TeklaName));
                    break;
                case ParameterValueType.Double:
                    AddRange(doubleNames, item.Select(i => i.TeklaName));
                    break;
                case ParameterValueType.Integer:
                    AddRange(integerNames, item.Select(i => i.TeklaName));
                    break;
                default:
                    break;
            }
        }

        return new QueryParameters(stringNames, doubleNames, integerNames);

        void AddRange(ArrayList arrayList, IEnumerable<string> toAdd)
        {
            foreach (var item in toAdd)
                arrayList.Add(item);
        }
    }
}

public class PropertySingleFactory
{
    public PropertySingle Construct(PropertySingleValueType propertySingleValueType)
    {
        var outputName = propertySingleValueType.Name;
        return propertySingleValueType.PropertyValue switch
        {
            StringValueType stringValueType => new(outputName, GetTeklaName(stringValueType.GetValue), GetParameterType(stringValueType.GetValue), ParameterValueType.String),
            IntegerValueType integerValueType => new(outputName, GetTeklaName(integerValueType.GetValue), GetParameterType(integerValueType.GetValue), ParameterValueType.Integer),
            MeasureValueType measureValueType => new(outputName, GetTeklaName(measureValueType.GetValue), GetParameterType(measureValueType.GetValue), ParameterValueType.Double),
            _ => new(outputName, string.Empty, ParameterType.Uda, ParameterValueType.String)
        };
    }

    private string GetTeklaName(VariableType variableType)
    {
        return variableType switch
        {
            UdaVariableType uda => uda.UdaName,
            TemplateVariableType template => template.TemplateName,
            _ => throw new ArgumentOutOfRangeException(nameof(variableType)),
        };
    }

    private ParameterType GetParameterType(VariableType variableType)
    {
        return variableType switch
        {
            UdaVariableType => ParameterType.Uda,
            TemplateVariableType => ParameterType.Template,
            _ => throw new ArgumentOutOfRangeException(nameof(variableType)),
        };
    }
}

public record PropertySingle(string OutputName, string TeklaName, ParameterType ParameterType, ParameterValueType ParameterValueType);

public record IfcProperties(string PSetName, IReadOnlyList<PropertySingle> Properties);

public record QueryParameters(ArrayList StringNames, ArrayList DoubleNames, ArrayList IntegerNames);

public record EntityQueryScope(List<IfcProperties> Properties, QueryParameters Templates, QueryParameters Udas);

public enum ParameterType
{
    Uda,
    Template
}
public enum ParameterValueType
{
    String,
    Double,
    Integer
}
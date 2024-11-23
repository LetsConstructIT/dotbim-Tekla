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

        var dictionary = new Dictionary<IncludeEntityType, List<PropertySingle>>();
        foreach (var propertySet in propertySetConfiguration.PropertySetDefinitions.Where(d => !d.isIgnored))
        {
            var bindings = propertySetConfiguration.PropertySetBindings.FirstOrDefault(b => b.referenceId == propertySet.referenceId); ;
            if (bindings is null)
                continue;

            var entityTypes = bindings.Rules.Select(r => r.entityType).ToList();
            var properties = propertySet.Properties.Property.OfType<PropertySingleValueType>()
                .Where(p => !p.isIgnored)
                .Select(p => _propertySingleFactory.Construct(p, new PSetName(propertySet.Name)))
                .ToList();

            foreach (var entityType in entityTypes)
            {
                if (dictionary.ContainsKey(entityType))
                    dictionary[entityType].AddRange(properties);
                else
                    dictionary[entityType] = properties;
            }
        }

        return new IfcPropertiesDictionary(dictionary);
    }
}

public class IfcPropertiesDictionary
{
    private readonly Dictionary<IncludeEntityType, EntityQueryScope> _dictionary;

    public IfcPropertiesDictionary(Dictionary<IncludeEntityType, List<PropertySingle>> dictionary)
    {
        _dictionary = TransformRawDictionary(dictionary);
    }

    public EntityQueryScope? QueryScope(IncludeEntityType entityType)
    {
        if (_dictionary.ContainsKey(entityType))
            return _dictionary[entityType];
        else
            return null;
    }

    private Dictionary<IncludeEntityType, EntityQueryScope> TransformRawDictionary(Dictionary<IncludeEntityType, List<PropertySingle>> dictionary)
    {
        var result = new Dictionary<IncludeEntityType, EntityQueryScope>();

        foreach (var keyValue in dictionary)
        {
            result[keyValue.Key] = Transform(keyValue.Value);
        }
        return result;
    }

    private EntityQueryScope Transform(List<PropertySingle> properties)
    {
        var udas = properties.Where(p => p.ParameterType == ParameterType.Uda);
        var templates = properties.Where(p => p.ParameterType == ParameterType.Template);

        return new EntityQueryScope(GetParameters(templates), GetParameters(udas));
    }

    private QueryParameters GetParameters(IEnumerable<PropertySingle> properties)
    {
        var stringProperties = new List<PropertySingle>();
        var doubleProperties = new List<PropertySingle>();
        var integerNames = new List<PropertySingle>();
        foreach (var item in properties.GroupBy(p => p.ParameterValueType))
        {
            switch (item.Key)
            {
                case ParameterValueType.String:
                    stringProperties.AddRange(item);
                    break;
                case ParameterValueType.Double:
                    doubleProperties.AddRange(item);
                    break;
                case ParameterValueType.Integer:
                    integerNames.AddRange(item);
                    break;
                default:
                    break;
            }
        }

        return new QueryParameters(ToQueryType(stringProperties, ParameterValueType.String),
                                   ToQueryType(doubleProperties, ParameterValueType.Double),
                                   ToQueryType(integerNames, ParameterValueType.Integer));

        SingleTypeQuery ToQueryType(List<PropertySingle> properties, ParameterValueType parameterValueType)
        {
            var names = new ArrayList(properties.Select(p => p.TeklaName).ToArray());

            return new SingleTypeQuery(parameterValueType, names, properties);
        }
    }
}

public class PropertySingleFactory
{
    public PropertySingle Construct(PropertySingleValueType propertySingleValueType, PSetName propertySetName)
    {
        var outputName = propertySingleValueType.Name;
        return propertySingleValueType.PropertyValue switch
        {
            StringValueType stringValueType => new(propertySetName, outputName, GetTeklaName(stringValueType.GetValue), GetParameterType(stringValueType.GetValue), ParameterValueType.String),
            IntegerValueType integerValueType => new(propertySetName, outputName, GetTeklaName(integerValueType.GetValue), GetParameterType(integerValueType.GetValue), ParameterValueType.Integer),
            MeasureValueType measureValueType => new(propertySetName, outputName, GetTeklaName(measureValueType.GetValue), GetParameterType(measureValueType.GetValue), ParameterValueType.Double),
            _ => new(propertySetName, outputName, string.Empty, ParameterType.Uda, ParameterValueType.String)
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

public record PSetName(string Name);
public record PropertySingle(PSetName PSet, string OutputName, string TeklaName, ParameterType ParameterType, ParameterValueType ParameterValueType);
public record SingleTypeQuery(ParameterValueType ParameterValueType, ArrayList QueryNames, IReadOnlyList<PropertySingle> Properties);
public record QueryParameters(SingleTypeQuery StringNames, SingleTypeQuery DoubleNames, SingleTypeQuery IntegerNames);
public record EntityQueryScope(QueryParameters Templates, QueryParameters Udas);

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
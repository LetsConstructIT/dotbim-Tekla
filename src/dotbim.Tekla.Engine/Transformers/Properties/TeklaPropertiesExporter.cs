using dotbimTekla.Engine.Exporters.Properties;
using dotbimTekla.Engine.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Tekla.Structures.Model;

namespace dotbimTekla.Engine.Transformers.Properties;
internal class TeklaPropertiesExporter
{
    private readonly IIfcEntityTypeQuery _ifcEntityTypeQuery;
    private readonly string _teklaVersion;
    private readonly IComparer<string> _dictionaryKeysComparer;

    private readonly Func<object, string> _convertDouble = (value => ((double)value).ToString(CultureInfo.InvariantCulture));
    private readonly Func<object, string> _convertInt = (value => ((int)value).ToString(CultureInfo.InvariantCulture));
    private readonly Func<object, string> _convertString = (value => (string)value);

    public TeklaPropertiesExporter()
    {
        _ifcEntityTypeQuery = new IfcEntityTypeQuery2022();
        _teklaVersion = $"Tekla Structures {Tekla.Structures.TeklaStructuresInfo.GetCurrentProgramVersion()}";
        _dictionaryKeysComparer = new NaturalStringComparer();
    }

    internal SortedDictionary<string, string> ReadProperties(ModelObject modelObject, IfcPropertiesDictionary? ifcPropertiesDictionary)
    {
        var result = new SortedDictionary<string, string>(_dictionaryKeysComparer)
        {
            ["Source"] = _teklaVersion
        };
                
        if (ifcPropertiesDictionary is null)
            return result;

        var entityType = _ifcEntityTypeQuery.GetEntityType(modelObject);
        if (!entityType.HasValue)
            return result;

        var queryScope = ifcPropertiesDictionary.QueryScope(entityType.Value);
        if (queryScope is null)
            return result;

        QueryTemplate(result, modelObject, queryScope.Templates);
        QueryUda(result, modelObject, queryScope.Udas);

        return result;
    }

    private void QueryTemplate(SortedDictionary<string, string> result, ModelObject modelObject, QueryParameters queryParameters)
    {
        static void doubleQuery(ModelObject modelObject, ArrayList queryNames, Hashtable result) => modelObject.GetDoubleReportProperties(queryNames, ref result);
        static void intQuery(ModelObject modelObject, ArrayList queryNames, Hashtable result) => modelObject.GetIntegerReportProperties(queryNames, ref result);
        static void stringQuery(ModelObject modelObject, ArrayList queryNames, Hashtable result) => modelObject.GetStringReportProperties(queryNames, ref result);

        SingleTypeQuery(result, modelObject, queryParameters.DoubleNames, _convertDouble, doubleQuery);
        SingleTypeQuery(result, modelObject, queryParameters.IntegerNames, _convertInt, intQuery);
        SingleTypeQuery(result, modelObject, queryParameters.StringNames, _convertString, stringQuery);
    }

    private void SingleTypeQuery(SortedDictionary<string, string> result, ModelObject modelObject, SingleTypeQuery singleTypeQuery, Func<object, string> resultConversion, Action<ModelObject, ArrayList, Hashtable> teklaQuery)
    {
        if (singleTypeQuery.QueryNames.Count == 0)
            return;

        var values = new Hashtable();
        teklaQuery(modelObject, singleTypeQuery.QueryNames, values);

        if (values.Count == 0)
            return;

        foreach (var property in singleTypeQuery.Properties)
        {
            if (!values.ContainsKey(property.TeklaName))
                continue;

            result[ConstructKey(property)] = resultConversion(values[property.TeklaName]);
        }
    }

    private void QueryUda(SortedDictionary<string, string> result, ModelObject modelObject, QueryParameters udas)
    {
        foreach (var property in udas.DoubleNames.Properties)
        {
            var value = double.MinValue;
            if (modelObject.GetUserProperty(property.TeklaName, ref value) && value != double.MinValue)
                result[ConstructKey(property)] = _convertDouble(value);
        }

        foreach (var property in udas.IntegerNames.Properties)
        {
            var value = int.MinValue;
            if (modelObject.GetUserProperty(property.TeklaName, ref value) && value != int.MinValue)
                result[ConstructKey(property)] = _convertInt(value);
        }

        foreach (var property in udas.StringNames.Properties)
        {
            var value = string.Empty;
            if (modelObject.GetUserProperty(property.TeklaName, ref value) && !string.IsNullOrEmpty(value))
                result[ConstructKey(property)] = _convertString(value);
        }
    }

    private string ConstructKey(PropertySingle property)
        => $"{property.PSet.Name}.{property.OutputName}";
}

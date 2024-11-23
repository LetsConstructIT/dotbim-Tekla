using dotbimTekla.Engine.Exporters.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Tekla.Structures.Model;

namespace dotbimTekla.Engine.Transformers.Properties;
internal class TeklaPropertiesExporter
{
    private readonly IIfcEntityTypeQuery _ifcEntityTypeQuery;
    public TeklaPropertiesExporter()
    {
        _ifcEntityTypeQuery = new IfcEntityTypeQuery2022();
    }

    internal Dictionary<string, string> ReadProperties(ModelObject modelObject, IfcPropertiesDictionary? ifcPropertiesDictionary)
    {
        if (ifcPropertiesDictionary is null)
            return [];

        var entityType = _ifcEntityTypeQuery.GetEntityType(modelObject);
        if (!entityType.HasValue)
            return [];

        var queryScope = ifcPropertiesDictionary.QueryScope(entityType.Value);
        if (queryScope is null)
            return [];

        var result = new Dictionary<string, string>();
        QueryTemplate(result, modelObject, queryScope.Templates);

        return result;
    }

    private void QueryTemplate(Dictionary<string, string> result, ModelObject part, QueryParameters queryParameters)
    {
        var singleTypeQuery = queryParameters.DoubleNames;

        if (singleTypeQuery.QueryNames.Count == 0)
            return;

        var values = new Hashtable();
        part.GetDoubleReportProperties(singleTypeQuery.QueryNames, ref values);

        if (values.Count == 0)
            return;

        foreach (var property in singleTypeQuery.Properties)
        {
            if (!values.ContainsKey(property.TeklaName))
                continue;

            var value = (double)values[property.TeklaName];
            result[ConstructKey(property)] = value.ToString(CultureInfo.InvariantCulture);
        }
    }

    private string ConstructKey(PropertySingle property)
        => $"{property.PSet.Name}.{property.OutputName}";
}

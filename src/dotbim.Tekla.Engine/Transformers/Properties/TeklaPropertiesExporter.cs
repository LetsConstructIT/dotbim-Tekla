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
        QueryTemplate(result, modelObject, queryScope.Templates, queryScope.Properties);

        return result;
    }

    private void QueryTemplate(Dictionary<string, string> result, ModelObject part, QueryParameters queryParameters, List<IfcProperties> properties)
    {
        var propertyNames = queryParameters.DoubleNames;

        if (propertyNames.Count > 0)
        {
            var values = new Hashtable();
            part.GetDoubleReportProperties(propertyNames, ref values);

            foreach (string propertyName in propertyNames)
            {
                if (!values.ContainsKey(propertyName))
                    continue;

                var value = (double)values[propertyName];

                var pSets = properties
                    .Where(p => p.Properties
                    .Any(r => r.ParameterType == ParameterType.Template && r.ParameterValueType == ParameterValueType.Double && r.TeklaName == propertyName));

                foreach (var pSet in pSets)
                {
                    result[ConstructKey(pSet.PSetName, propertyName)] = value.ToString(CultureInfo.InvariantCulture);
                }
            }
        }
    }

    private string ConstructKey(string propertySet, string property)
        => $"{propertySet}.{property}";
}

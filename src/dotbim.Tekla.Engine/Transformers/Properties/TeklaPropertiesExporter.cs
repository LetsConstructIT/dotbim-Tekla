using dotbimTekla.Engine.Exporters.Properties;
using System;
using System.Collections.Generic;
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

    internal Dictionary<string, string> ReadProperties(Part part, IfcPropertiesDictionary? ifcPropertiesDictionary)
    {
        if (ifcPropertiesDictionary is null)
            return [];

        var entityType = _ifcEntityTypeQuery.GetEntityType(part);
        if (!entityType.HasValue)
            return [];


        return new Dictionary<string, string>();
    }
}

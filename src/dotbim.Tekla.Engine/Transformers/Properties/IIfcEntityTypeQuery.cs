using System;
using System.Linq;
using Tekla.Structures.Model;

namespace dotbimTekla.Engine.Transformers.Properties;
public interface IIfcEntityTypeQuery
{
    IncludeEntityType? GetEntityType(ModelObject modelObject);
}

public class IfcEntityTypeQuery2022 : IIfcEntityTypeQuery
{
    private readonly Type _resultType;
    public IfcEntityTypeQuery2022()
    {
        _resultType = typeof(IncludeEntityType);
    }

    public IncludeEntityType? GetEntityType(ModelObject modelObject)
    {
        var entityOverride = string.Empty;

        modelObject.GetReportProperty("IFC_ENTITY_OVERRIDE", ref entityOverride);
        if (string.IsNullOrEmpty(entityOverride))
            return null;

        var entityType = entityOverride.Split('.').First();

        return (IncludeEntityType)Enum.Parse(_resultType, entityType);
    }
}
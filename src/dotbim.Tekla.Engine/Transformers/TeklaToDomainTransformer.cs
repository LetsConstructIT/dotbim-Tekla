using dotbim;
using dotbimTekla.Engine.Entities;
using dotbimTekla.Engine.Transformers.Properties;
using System;
using System.Collections.Generic;
using TSM = Tekla.Structures.Model;
using TSMUI = Tekla.Structures.Model.UI;

namespace dotbimTekla.Engine.Transformers;

public class TeklaToDomainTransformer
{
    private readonly TeklaGeometryTransformer _geometryTransformer;
    private readonly IIfcEntityTypeQuery _ifcEntityTypeQuery;

    public TeklaToDomainTransformer()
    {
        _geometryTransformer = new TeklaGeometryTransformer();
        _ifcEntityTypeQuery = new IfcEntityTypeQuery2022();
    }

    public Solid Transform(TSM.Part part)
        => _geometryTransformer.Transform(part);

    public Color GetColor(TSM.ModelObject modelObject)
    {
        var teklaColor = new TSMUI.Color();
        TSMUI.ModelObjectVisualization.GetRepresentation(modelObject, ref teklaColor);

        return new Color()
        {
            A = 255,
            R = (int)(teklaColor.Red * 255),
            G = (int)(teklaColor.Green * 255),
            B = (int)(teklaColor.Blue * 255)
        };
    }

    public Dictionary<string, string> GetMetadata(TSM.Part part)
    {
        return new Dictionary<string, string>();
    }
}

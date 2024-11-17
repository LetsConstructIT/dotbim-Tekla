using dotbim.Tekla.Engine.Entities;
using dotbim.Tekla.Engine.Exporters;
using dotbim.Tekla.Engine.Extensions;
using dotbim.Tekla.Engine.Selectors;
using dotbim.Tekla.Engine.Transformers;
using dotbim.Tekla.Engine.ValueObjects;
using System.Collections.Generic;
using Tekla.Structures.Model;
using TSM = Tekla.Structures.Model;

namespace dotbim.Tekla.Engine;

public class Exporter
{
    private readonly TeklaSelectorFactory _teklaSelectorFactory;
    private readonly TeklaToDomainTransformer _teklaToDomainTransformer;
    private readonly SolidTesselator _solidTesselator;
    private readonly DotbimExporter _dotbimExporter;

    public Exporter()
    {
        _teklaSelectorFactory = new TeklaSelectorFactory();
        _teklaToDomainTransformer = new TeklaToDomainTransformer();
        _solidTesselator = new SolidTesselator();
        _dotbimExporter = new DotbimExporter();
    }

    public void Export(ExportSettings settings)
    {
        var modelObjects = _teklaSelectorFactory.Create(settings.Mode).Get();
        if (modelObjects.None())
            return;

        var elementsData = QueryElementData(modelObjects);

        var dotbimFile = _dotbimExporter.CreateDotbim(elementsData);
        dotbimFile.Save(settings.FilePath);
    }

    private List<ElementData> QueryElementData(IEnumerable<ModelObject> modelObjects)
    {
        var elementsData = new List<ElementData>();
        foreach (var modelObject in modelObjects)
        {
            if (modelObject is TSM.Part part)
            {
                var solid = _teklaToDomainTransformer.Transform(part);
                var mesh = _solidTesselator.GetMesh(solid);

                var color = _teklaToDomainTransformer.GetColor(part);
                var elementData = new ElementData(mesh, color, new Dictionary<string, string>());
                elementsData.Add(elementData);
            }
        }

        return elementsData;
    }
}

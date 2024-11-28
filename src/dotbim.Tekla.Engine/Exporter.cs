using dotbimTekla.Engine.Entities;
using dotbimTekla.Engine.Exporters;
using dotbimTekla.Engine.Exporters.Properties;
using dotbimTekla.Engine.Extensions;
using dotbimTekla.Engine.Selectors;
using dotbimTekla.Engine.Transformers;
using dotbimTekla.Engine.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using TSM = Tekla.Structures.Model;

namespace dotbimTekla.Engine;

public class Exporter
{
    private readonly TeklaSelectorFactory _teklaSelectorFactory;
    private readonly TeklaToDomainTransformer _teklaToDomainTransformer;
    private readonly SolidTesselator _solidTesselator;
    private readonly DotbimExporter _dotbimExporter;
    private readonly PropertySetBuilder _propertySetBuilder;

    public Exporter()
    {
        _teklaSelectorFactory = new TeklaSelectorFactory();
        _teklaToDomainTransformer = new TeklaToDomainTransformer();
        _solidTesselator = new SolidTesselator();
        _dotbimExporter = new DotbimExporter();
        _propertySetBuilder = new PropertySetBuilder();
    }

    public void Export(ExportSettings settings)
    {
        var modelObjects = _teklaSelectorFactory.Create(settings.Mode).Get().ToList();
        if (modelObjects.None())
            return;

        var ifcPropertiesDictionary = _propertySetBuilder.GetNeededProperties(settings.PropertySetSettingsPath);

        var elementsData = QueryElementData(modelObjects, ifcPropertiesDictionary);

        var dotbimFile = _dotbimExporter.CreateDotbim(elementsData);
        dotbimFile.Save(settings.FilePath, format: false);
    }

    private List<ElementData> QueryElementData(IReadOnlyList<TSM.ModelObject> modelObjects, IfcPropertiesDictionary? ifcPropertiesDictionary)
    {
        var elementsData = new List<ElementData>();
        foreach (var modelObject in modelObjects)
        {
            if (modelObject is TSM.Part part)
            {
                var solid = _teklaToDomainTransformer.Transform(part);
                var mesh = _solidTesselator.GetMesh(solid);

                var color = _teklaToDomainTransformer.GetColor(modelObject);
                var metadata = _teklaToDomainTransformer.GetMetadata(modelObject, ifcPropertiesDictionary);
                var elementData = new ElementData(mesh, color, modelObject.Identifier.GUID, part.Name, metadata);
                elementsData.Add(elementData);
            }
        }

        return elementsData;
    }
}

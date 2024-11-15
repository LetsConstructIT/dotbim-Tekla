using System.Collections.Generic;
using dotbim.Tekla.Engine.Entities;

namespace dotbim.Tekla.Engine.Exporters;

public class DotbimExporter
{
    private readonly DotbimMeshCreator _meshCreator;
    private readonly DotbimElementCreator _elementCreator;

    public DotbimExporter()
    {
        _meshCreator = new DotbimMeshCreator();
        _elementCreator = new DotbimElementCreator();
    }

    public File CreateDotbim(IReadOnlyList<ElementData> elementsData)
    {
        var file = new File
        {
            Meshes = _meshCreator.Create(elementsData),
            Elements = _elementCreator.Create(elementsData),
            Info = new Dictionary<string, string>(),
            SchemaVersion = "1.1.0"
        };

        return file;
    }
}

using dotbimTekla.Engine.Entities;
using dotbimTekla.Engine.Exporters;
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

    public Exporter()
    {
        _teklaSelectorFactory = new TeklaSelectorFactory();
        _teklaToDomainTransformer = new TeklaToDomainTransformer();
        _solidTesselator = new SolidTesselator();
        _dotbimExporter = new DotbimExporter();
    }

    public void Export(ExportSettings settings)
    {
        var times = new List<long>();

        var sw = System.Diagnostics.Stopwatch.StartNew();
        var modelObjects = _teklaSelectorFactory.Create(settings.Mode).Get().ToList();
        if (modelObjects.None())
            return;

        times.Add(sw.ElapsedMilliseconds);
        sw.Restart();

        var elementsData = QueryElementData(modelObjects);

        times.Add(sw.ElapsedMilliseconds);
        sw.Restart();

        var dotbimFile = _dotbimExporter.CreateDotbim(elementsData);
        dotbimFile.Save(settings.FilePath);

        times.Add(sw.ElapsedMilliseconds);
        sw.Stop();
        var message = $"\nIt took {string.Join(" ms, ", times)} ms. Summary: {times.Sum()} ms.";
        Console.WriteLine(message);
        System.IO.File.AppendAllText($"C:\\temp\\benchmark.txt", message);
    }

    private List<ElementData> QueryElementData(IReadOnlyList<TSM.ModelObject> modelObjects)
    {
        var elementsData = new List<ElementData>();
        foreach (var modelObject in modelObjects)
        {
            if (modelObject is TSM.Part part)
            {
                var solid = _teklaToDomainTransformer.Transform(part);
                var mesh = _solidTesselator.GetMesh(solid);

                var color = _teklaToDomainTransformer.GetColor(part);
                var elementData = new ElementData(mesh, color, part.Identifier.GUID, new Dictionary<string, string>());
                elementsData.Add(elementData);
            }
        }

        return elementsData;
    }
}

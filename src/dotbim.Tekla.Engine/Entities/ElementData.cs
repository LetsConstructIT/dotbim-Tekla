using dotbim;
using System;
using System.Collections.Generic;

namespace dotbimTekla.Engine.Entities;

public class ElementData
{
    public IReadOnlyList<Triangle> Triangles { get; }
    public Color Color { get; }
    public Guid Guid { get; }
    public string Name { get; }
    public Dictionary<string, string> Metadata { get; }

    public ElementData(IReadOnlyList<Triangle> triangles, Color color, Guid guid, string name, Dictionary<string, string> metadata)
    {
        Triangles = triangles ?? throw new ArgumentNullException(nameof(triangles));
        Color = color;
        Guid = guid;
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Metadata = metadata ?? throw new ArgumentNullException(nameof(metadata));
    }
}

using dotbim;
using System;
using System.Collections.Generic;

namespace dotbimTekla.Engine.Entities;

public class ElementData
{
    public IReadOnlyList<Triangle> Triangles { get; }
    public Color Color { get; }
    public Guid Guid { get; }
    public IDictionary<string, string> Metadata { get; }

    public ElementData(IReadOnlyList<Triangle> triangles, Color color, Guid guid, IDictionary<string, string> metadata)
    {
        Triangles = triangles ?? throw new ArgumentNullException(nameof(triangles));
        Color = color;
        Guid = guid;
        Metadata = metadata ?? throw new ArgumentNullException(nameof(metadata));
    }
}

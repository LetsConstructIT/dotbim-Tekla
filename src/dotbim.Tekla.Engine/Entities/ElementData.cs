using System;
using System.Collections.Generic;

namespace dotbim.Tekla.Engine.Entities;

public class ElementData
{
    public IReadOnlyList<Triangle> Triangles { get; }
    public Color Color { get; }
    public IDictionary<string, string> Metadata { get; }

    public ElementData(IReadOnlyList<Triangle> triangles, Color color, IDictionary<string, string> metadata)
    {
        Triangles = triangles ?? throw new ArgumentNullException(nameof(triangles));
        Color = color;
        Metadata = metadata ?? throw new ArgumentNullException(nameof(metadata));
    }
}

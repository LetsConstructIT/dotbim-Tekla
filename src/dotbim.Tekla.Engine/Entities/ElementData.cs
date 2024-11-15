using System;
using System.Collections.Generic;

namespace dotbim.Tekla.Engine.Entities;

public class ElementData
{
    public IReadOnlyList<Triangle> Triangles { get; }
    public IDictionary<string, string> Metadata { get; }

    public ElementData(IReadOnlyList<Triangle> triangles, IDictionary<string, string> metadata)
    {
        Triangles = triangles ?? throw new ArgumentNullException(nameof(triangles));
        Metadata = metadata ?? throw new ArgumentNullException(nameof(metadata));
    }
}

using System;
using System.Collections.Generic;
using Tekla.Structures.Geometry3d;

namespace dotbim.Tekla.Engine.Entities;

public class Polygon
{
    public IReadOnlyList<Point> Points{ get; }

    public Polygon(IReadOnlyList<Point> points)
    {
        Points = points ?? throw new ArgumentNullException(nameof(points));
    }
}

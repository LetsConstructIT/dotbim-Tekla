using System;
using System.Collections.Generic;
using Tekla.Structures.Geometry3d;

namespace dotbim.Tekla.Engine.Entities;

public class Polygon
{
    public IEnumerable<Point> Points{ get; }

    public Polygon(IEnumerable<Point> points)
    {
        Points = points ?? throw new ArgumentNullException(nameof(points));
    }
}

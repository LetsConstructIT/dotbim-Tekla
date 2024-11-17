using System;
using System.Collections.Generic;
using Tekla.Structures.Geometry3d;

namespace dotbimTekla.Engine.Entities;

public class Polygon
{
    public IReadOnlyList<Point> Points { get; }

    public Polygon(IReadOnlyList<Point> points)
    {
        Points = points ?? throw new ArgumentNullException(nameof(points));
    }

    public Polygon TransformBy(Matrix matrix)
    {
        var local = new Point[Points.Count];
        for (int i = 0; i < Points.Count; i++)
        {
            local[i] = matrix.Transform(Points[i]);
        }

        return new Polygon(local);
    }
}

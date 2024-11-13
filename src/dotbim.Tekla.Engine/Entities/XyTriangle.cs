using System;
using Tekla.Structures.Geometry3d;

namespace dotbim.Tekla.Engine.Entities;

public class XyTriangle
{
    public Point Point1 { get; }
    public Point Point2 { get; }
    public Point Point3 { get; }

    public XyTriangle(Point point1, Point point2, Point point3)
    {
        Point1 = point1 ?? throw new ArgumentNullException(nameof(point1));
        Point2 = point2 ?? throw new ArgumentNullException(nameof(point2));
        Point3 = point3 ?? throw new ArgumentNullException(nameof(point3));
    }

    public Triangle Transform(Matrix matrix)
    {
        return new Triangle(matrix.Transform(Point1),
                            matrix.Transform(Point2),
                            matrix.Transform(Point3));
    }
}

public class Triangle
{
    public Point Point1 { get; }
    public Point Point2 { get; }
    public Point Point3 { get; }

    public Triangle(Point point1, Point point2, Point point3)
    {
        Point1 = point1 ?? throw new ArgumentNullException(nameof(point1));
        Point2 = point2 ?? throw new ArgumentNullException(nameof(point2));
        Point3 = point3 ?? throw new ArgumentNullException(nameof(point3));
    }
}
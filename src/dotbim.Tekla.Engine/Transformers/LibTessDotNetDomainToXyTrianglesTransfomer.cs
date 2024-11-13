using dotbim.Tekla.Engine.Entities;
using System.Collections.Generic;
using LibTessDotNet.Double;
using TSG = Tekla.Structures.Geometry3d;

namespace dotbim.Tekla.Engine.Transformers;

public class LibTessDotNetDomainToXyTrianglesTransfomer : IDomainToTrianglesTransformer
{
    private readonly Tess _tess = new();

    public IReadOnlyList<XyTriangle> Transform(XyFace face)
    {
        _tess.AddContour(TransformToLibTess(face.Contour), ContourOrientation.Clockwise);

        foreach (var hole in face.Holes)
            _tess.AddContour(TransformToLibTess(hole), ContourOrientation.CounterClockwise);

        _tess.Tessellate(WindingRule.EvenOdd, ElementType.Polygons, 3);

        var triangles = new XyTriangle[_tess.ElementCount];

        for (int i = 0; i < _tess.ElementCount; i++)
        {
            triangles[i] = new XyTriangle(Transform(_tess.Vertices[_tess.Elements[i * 3]].Position),
                                          Transform(_tess.Vertices[_tess.Elements[i * 3 + 1]].Position),
                                          Transform(_tess.Vertices[_tess.Elements[i * 3 + 2]].Position));
        }

        return triangles;
    }

    private ContourVertex[] TransformToLibTess(Polygon contour)
    {
        var result = new ContourVertex[contour.Points.Count];
        for (int i = 0; i < contour.Points.Count; i++)
        {
            var point = contour.Points[i];
            result[i].Position = Transform(point);
        }

        return result;
    }

    private Vec3 Transform(TSG.Point point)
        => new(point.X, point.Y, point.Z);

    private TSG.Point Transform(Vec3 point)
        => new(point.X, point.Y, point.Z);
}

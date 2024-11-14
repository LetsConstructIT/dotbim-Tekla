using System;
using System.Collections.Generic;
using System.Linq;
using TSG = Tekla.Structures.Geometry3d;

namespace dotbim.Tekla.Engine.Entities;

public class Face
{
    public TSG.Vector Normal { get; }
    public Polygon Contour { get; }
    public IReadOnlyList<Polygon> Holes { get; }
    private TSG.CoordinateSystem _coord;

    public Face(Polygon contour, TSG.Vector normal) : this(contour, normal, Array.Empty<Polygon>())
    {

    }

    public Face(Polygon contour, TSG.Vector normal, IReadOnlyList<Polygon> holes)
    {
        Contour = contour ?? throw new ArgumentNullException(nameof(contour));
        Normal = normal ?? throw new ArgumentNullException(nameof(normal));
        Holes = holes ?? throw new ArgumentNullException(nameof(holes));

        _coord = GetCoordinateSystem();
    }

    public TSG.CoordinateSystem GetCoordinateSystem()
    {
        var axisX = new TSG.Vector(Contour.Points[1] - Contour.Points[0]);
        var axisY = Normal.Cross(axisX);

        return new TSG.CoordinateSystem(Contour.Points.First(),
                                        axisX.GetNormal(),
                                        axisY.GetNormal());
    }

    public XyFace TransformToLocal()
    {
        var matrix = TSG.MatrixFactory.ToCoordinateSystem(_coord);

        var localContour = Contour.TransformBy(matrix);
        var localHoles = Holes.Select(h => h.TransformBy(matrix));

        return new XyFace(localContour, localHoles.ToArray());
    }

    public TSG.Matrix TransformationFromCoordinateSystem()
    {
        return TSG.MatrixFactory.FromCoordinateSystem(_coord);
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using Tekla.Structures.Geometry3d;

namespace dotbim.Tekla.Engine.Entities;

public class Face
{
    public Vector Normal { get; }
    public Polygon Contour { get; }
    public IReadOnlyList<Polygon> Holes { get; }

    public Face(Polygon contour, Vector normal) : this(contour, normal, Array.Empty<Polygon>())
    {

    }

    public Face(Polygon contour, Vector normal, IReadOnlyList<Polygon> holes)
    {
        Contour = contour ?? throw new ArgumentNullException(nameof(contour));
        Normal = normal ?? throw new ArgumentNullException(nameof(normal));
        Holes = holes ?? throw new ArgumentNullException(nameof(holes));
    }

    public CoordinateSystem GetCoordinateSystem()
    {
        var axisX = new Vector(Contour.Points[1] - Contour.Points[0]);
        var axisY = Normal.Cross(axisX);

        return new CoordinateSystem(Contour.Points.First(),
                                    axisX.GetNormal(),
                                    axisY.GetNormal());
    }
}

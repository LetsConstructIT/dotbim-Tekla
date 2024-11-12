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
}

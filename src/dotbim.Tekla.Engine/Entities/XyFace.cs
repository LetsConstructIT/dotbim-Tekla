using System;
using System.Collections.Generic;

namespace dotbim.Tekla.Engine.Entities;

public class XyFace
{
    public Polygon Contour { get; }
    public IReadOnlyList<Polygon> Holes { get; }

    public XyFace(Polygon contour) : this(contour, Array.Empty<Polygon>())
    {

    }

    public XyFace(Polygon contour, IReadOnlyList<Polygon> holes)
    {
        Contour = contour ?? throw new ArgumentNullException(nameof(contour));
        Holes = holes ?? throw new ArgumentNullException(nameof(holes));
    }
}

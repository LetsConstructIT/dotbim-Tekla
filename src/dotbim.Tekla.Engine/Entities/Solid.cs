using System;
using System.Collections.Generic;

namespace dotbim.Tekla.Engine.Entities;

public class Solid
{
    public IReadOnlyList<Face> Faces { get; }

    public Solid(IReadOnlyList<Face> faces)
    {
        Faces = faces ?? throw new ArgumentNullException(nameof(faces));
    }
}

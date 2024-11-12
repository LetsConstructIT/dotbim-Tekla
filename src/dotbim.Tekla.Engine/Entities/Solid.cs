using System;
using System.Collections.Generic;

namespace dotbim.Tekla.Engine.Entities;

public class Solid
{
    public IEnumerable<Face> Faces { get; }

    public Solid(IEnumerable<Face> faces)
    {
        Faces = faces ?? throw new ArgumentNullException(nameof(faces));
    }
}

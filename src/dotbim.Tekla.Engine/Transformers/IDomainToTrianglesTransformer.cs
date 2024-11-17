using dotbimTekla.Engine.Entities;
using System.Collections.Generic;

namespace dotbimTekla.Engine.Transformers;

public interface IDomainToTrianglesTransformer
{
    IReadOnlyList<XyTriangle> Transform(XyFace face);
}
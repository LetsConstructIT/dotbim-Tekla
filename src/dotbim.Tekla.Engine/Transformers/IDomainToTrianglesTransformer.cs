using dotbim.Tekla.Engine.Entities;
using System.Collections.Generic;

namespace dotbim.Tekla.Engine.Transformers;

public interface IDomainToTrianglesTransformer
{
    IReadOnlyList<XyTriangle> Transform(XyFace face);
}
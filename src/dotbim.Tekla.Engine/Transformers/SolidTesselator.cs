using dotbim.Tekla.Engine.Entities;
using System;
using System.Collections.Generic;

namespace dotbim.Tekla.Engine.Transformers
{
    public class SolidTesselator
    {
        private readonly IDomainToTrianglesTransformer _tesselator;

        public SolidTesselator()
        {
            _tesselator = new LibTessDotNetDomainToXyTrianglesTransfomer();
        }

        public IReadOnlyList<Triangle> GetMesh(Solid solid)
        {
            var triangles = new List<Triangle>();

            foreach (var face in solid.Faces)
            {
                var xyFace = face.TransformToLocal();

                var xyTriangles = _tesselator.Transform(xyFace);

                var transformationBack = face.TransformationFromCoordinateSystem();
                foreach (var xyTriangle in xyTriangles)
                {
                    triangles.Add(xyTriangle.Transform(transformationBack));
                }
            }

            return triangles;
        }
    }
}

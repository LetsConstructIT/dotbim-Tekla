using dotbimTekla.Engine.Entities;
using System.Collections.Generic;
using System.Linq;
using Tekla.Structures.Geometry3d;
using TSM = Tekla.Structures.Model;
using TSS = Tekla.Structures.Solid;

namespace dotbimTekla.Engine.Transformers;

public class TeklaGeometryTransformer
{
    public Solid Transform(TSM.Part part)
    {
        var faces = new List<Face>();
        var teklaSolid = part.GetSolid();

        var faceEnum = teklaSolid.GetFaceEnumerator();
        while (faceEnum.MoveNext())
        {
            var face = Transform(faceEnum.Current);
            faces.Add(face);
        }

        return new Solid(faces);
    }

    private Face Transform(TSS.Face teklaFace)
    {
        var polygons = new List<Polygon>();
        var loopEnum = teklaFace.GetLoopEnumerator();
        while (loopEnum.MoveNext())
        {
            var loop = loopEnum.Current;

            polygons.Add(Transform(loop));
        }

        return new Face(polygons.First(), teklaFace.Normal, polygons.Skip(1).ToArray());
    }

    private Polygon Transform(TSS.Loop loop)
    {
        var points = new List<Point>();

        var vertexEnum = loop.GetVertexEnumerator();
        while (vertexEnum.MoveNext())
        {
            points.Add(vertexEnum.Current);
        }

        return new Polygon(points);
    }
}
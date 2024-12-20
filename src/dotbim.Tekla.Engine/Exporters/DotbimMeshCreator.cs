﻿using dotbim;
using dotbimTekla.Engine.Entities;
using System.Collections.Generic;
using Tekla.Structures.Geometry3d;

namespace dotbimTekla.Engine.Exporters;

public class DotbimMeshCreator
{
    private const int _scale = 1000;

    public List<Mesh> Create(IReadOnlyList<ElementData> elements)
    {
        var meshes = new List<Mesh>();
        for (int i = 0; i < elements.Count; i++)
        {
            meshes.Add(CreateMesh(elements[i].Triangles, i));
        }

        return meshes;
    }

    private Mesh CreateMesh(IReadOnlyList<Triangle> triangles, int meshId)
    {
        var pointDict = FindUniquePoints(triangles);

        return new Mesh()
        {
            MeshId = meshId,
            Coordinates = GetMeshCoordinates(pointDict),
            Indices = GetMeshIndicies(triangles, pointDict)
        };
    }

    private List<int> GetMeshIndicies(IReadOnlyList<Triangle> triangles, Dictionary<Point, int> pointDict)
    {
        var indicies = new List<int>(triangles.Count * 3);
        foreach (var triangle in triangles)
        {
            indicies.Add(pointDict[triangle.Point1]);
            indicies.Add(pointDict[triangle.Point2]);
            indicies.Add(pointDict[triangle.Point3]);
        }

        return indicies;
    }

    private List<double> GetMeshCoordinates(Dictionary<Point, int> pointDict)
    {
        var coords = new List<double>(pointDict.Count * 3);
        foreach (var item in pointDict)
        {
            coords.Add(item.Key.X / _scale);
            coords.Add(item.Key.Y / _scale);
            coords.Add(item.Key.Z / _scale);
        }

        return coords;
    }

    private Dictionary<Point, int> FindUniquePoints(IReadOnlyList<Triangle> triangles)
    {
        var pointDict = new Dictionary<Point, int>();
        var index = -1;
        foreach (var triangle in triangles)
        {
            foreach (var point in triangle.Points)
            {
                if (!pointDict.ContainsKey(point))
                {
                    index++;
                    pointDict[point] = index;
                }
            }
        }

        return pointDict;
    }
}

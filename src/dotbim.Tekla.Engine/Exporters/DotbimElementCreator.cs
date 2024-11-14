using dotbim.Tekla.Engine.Entities;
using System;
using System.Collections.Generic;

namespace dotbim.Tekla.Engine.Exporters;

public class DotbimElementCreator
{
    public List<Element> Create(IReadOnlyList<ElementData> elementsData)
    {
        var elements = new List<Element>();

        for (int i = 0; i < elementsData.Count; i++)
        {
            elements.Add(Create(elementsData[i], i));
        }

        return elements;
    }

    private Element Create(ElementData elementData, int meshId)
    {
        return new Element()
        {
            Color = new Color() { A = 122, R = 255, G = 0, B = 255 },
            Guid = Guid.NewGuid().ToString(),
            Vector = new Vector() { X=0, Y=0, Z=0},
            Rotation = new Rotation() { Qx = 0, Qy=0, Qz=0, Qw=1},
            MeshId = meshId
        };
    }
}

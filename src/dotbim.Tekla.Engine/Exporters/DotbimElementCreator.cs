﻿using dotbim;
using dotbimTekla.Engine.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace dotbimTekla.Engine.Exporters;

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
            Type = elementData.Name,
            Color = elementData.Color,
            Guid = elementData.Guid.ToString(),
            Vector = new Vector() { X = 0, Y = 0, Z = 0 },
            Rotation = new Rotation() { Qx = 0, Qy = 0, Qz = 0, Qw = 1 },
            MeshId = meshId,
            Info = elementData.Metadata.ToDictionary(x => x.Key, x => x.Value)
        };
    }
}

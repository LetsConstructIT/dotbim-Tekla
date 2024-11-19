using System;
using System.Collections.Generic;
using System.Text;

namespace dotbimTekla.Engine.Exporters.Properties;
public class PropertySetBuilder
{
    private readonly XmlFileSerializer _xmlFileSerializer;

    public PropertySetBuilder()
    {
        _xmlFileSerializer = new();
    }

    public void GetNeededProperties(string filePath)
    {
        var properties = _xmlFileSerializer.ReadFile(filePath);
    }
}

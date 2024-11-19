using System.IO;
using System.Xml.Serialization;

namespace dotbimTekla.Engine.Exporters.Properties;

public class XmlFileSerializer
{
    public PropertySetConfiguration? ReadFile(string filePath)
    {
        if (!File.Exists(filePath))
            return null;

        using TextReader textReader = new StreamReader(filePath);
        return (PropertySetConfiguration)new XmlSerializer(typeof(PropertySetConfiguration)).Deserialize(textReader);
    }
}

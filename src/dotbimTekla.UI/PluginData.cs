using Tekla.Structures.Plugins;

namespace dotbimTekla.UI;
public class PluginData
{
    [StructuresField("SelectionMode")]
    public int SelectionMode;

    [StructuresField("OutputPath")]
    public string OutputPath;

    [StructuresField("PropertySets")]
    public string PropertySets;
}

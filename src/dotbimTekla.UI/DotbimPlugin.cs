using dotbimTekla.Engine.ValueObjects;
using dotbimTekla.Engine;
using System.Collections.Generic;
using Tekla.Structures.Plugins;

namespace dotbimTekla.UI;

[Plugin("dotbim Export")]
[PluginUserInterface("dotbimTekla.UI.MainWindow")]
[InputObjectDependency(InputObjectDependency.NOT_DEPENDENT)]
public class DotbimPlugin : PluginBase
{
    private readonly PluginData _pluginData;
    private readonly PropertySetsDefinitionSearcher _propertySetsDefinitionSearcher;

    public DotbimPlugin(PluginData pluginData)
    {
        _pluginData = pluginData;

        _propertySetsDefinitionSearcher = new PropertySetsDefinitionSearcher();
    }

    public override List<InputDefinition> DefineInput()
    {
        return new List<InputDefinition>();
    }

    public override bool Run(List<InputDefinition> Input)
    {
        var settings = GetSettings();

        var sut = new Exporter();
        sut.Export(settings);

        return true;
    }

    private ExportSettings GetSettings()
    {
        var pSetSettings = _propertySetsDefinitionSearcher.FindSettingsPath(_pluginData.PropertySets);

        return new ExportSettings((ExportMode)_pluginData.SelectionMode,
                                  _pluginData.OutputPath,
                                  pSetSettings);
    }
}

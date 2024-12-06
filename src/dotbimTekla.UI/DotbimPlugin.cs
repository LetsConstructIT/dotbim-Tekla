using dotbimTekla.Engine.ValueObjects;
using dotbimTekla.Engine;
using System.Collections.Generic;
using Tekla.Structures.Plugins;
using Trimble.Remoting.Proxies;
using Tekla.Structures.Datatype;

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
        try
        {
            var settings = GetSettings();

            var sut = new Exporter();
            sut.Export(settings);

            Tekla.Structures.Model.Operations.Operation.DisplayPrompt($".bim file saved as {settings.FilePath}");
            return true;
        }
        catch (System.Exception ex)
        {
            Tekla.Structures.ModelInternal.Operation.dotWriteErrorToSessionLog(ex.ToString());
            Tekla.Structures.Model.Operations.Operation.DisplayPrompt("Export to .bim failed. Check Tekla's session log for more info.");
            return false;
        }
    }

    private ExportSettings GetSettings()
    {
        var pSetSettings = _pluginData.PropertySets == "<empty>" ?
            string.Empty :
            _propertySetsDefinitionSearcher.FindSettingsPath(_pluginData.PropertySets);

        return new ExportSettings((ExportMode)_pluginData.SelectionMode,
                                  GetFullPath(_pluginData.OutputPath),
                                  pSetSettings);
    }

    private string GetFullPath(string path)
        => RelativePathHelper.ExpandRelativePath(path);
}

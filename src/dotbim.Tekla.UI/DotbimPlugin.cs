using dotbim.Tekla.Engine;
using dotbim.Tekla.Engine.ValueObjects;
using System.Collections.Generic;
using Tekla.Structures.Plugins;

namespace dotbim.Tekla.UI;

[Plugin("dotbim Export")]
[PluginUserInterface("dotbim.Tekla.UI.MainWindow")]
[InputObjectDependency(InputObjectDependency.NOT_DEPENDENT)]
public class DotbimPlugin : PluginBase
{
    public override List<InputDefinition> DefineInput()
    {
        List<InputDefinition> inputDefinitions = new List<InputDefinition>();
        return inputDefinitions;
    }

    public override bool Run(List<InputDefinition> input)
    {
        var settings = new ExportSettings(ExportMode.Selection, @"C:\temp\test.bim");

        var sut = new Exporter();
        sut.Export(settings);

        return true;
    }
}

using dotbimTekla.Engine;
using dotbimTekla.Engine.ValueObjects;
using System.Collections.Generic;
using Tekla.Structures.Plugins;

namespace dotbimTekla.UI;

[Plugin("dotbim Export")]
[PluginUserInterface("dotbimTekla.UI.MainWindow")]
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

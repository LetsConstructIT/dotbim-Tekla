using dotbimTekla.Engine.ValueObjects;
using dotbimTekla.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading.Tasks;
using Tekla.Structures.Plugins;

namespace dotbimTekla.UI;

[Plugin("dotbim Export")]
[PluginUserInterface("dotbimTekla.UI.MainWindow")]
[InputObjectDependency(InputObjectDependency.NOT_DEPENDENT)]
public class DotbimPlugin : PluginBase
{
    public override List<InputDefinition> DefineInput()
    {
        return new List<InputDefinition>();
    }

    public override bool Run(List<InputDefinition> Input)
    {
        var powerFabSettings = @"C:\TeklaStructures\2024.0\Environments\common\system\AdditionalPSets\Tekla PowerFab.xml";
        var settings = new ExportSettings(ExportMode.Selection,
                                          @"C:\temp\test.bim",
                                          powerFabSettings);

        var sut = new Exporter();
        sut.Export(settings);

        return true;
    }
}

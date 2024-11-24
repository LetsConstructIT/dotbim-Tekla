using dotbimTekla.Engine.ValueObjects;
using dotbimTekla.Engine;
using System;

namespace dotbimTekla.Benchmark
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ExportSample();
        }

        private static void ExportSample()
        {
            var powerFabSettings = @"C:\TeklaStructures\2024.0\Environments\common\system\AdditionalPSets\Tekla PowerFab.xml";
            var coordinationSettings = @"C:\TeklaStructures\2024.0\environments\common\inp\Coordination view default properties.xml";
            var settings = new ExportSettings(ExportMode.Selection,
                                              @"C:\temp\test.bim",
                                              coordinationSettings);

            var sut = new Exporter();
            sut.Export(settings);
        }
    }
}

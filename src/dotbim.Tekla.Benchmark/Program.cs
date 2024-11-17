﻿using dotbim.Tekla.Engine.ValueObjects;
using dotbim.Tekla.Engine;
using System;

namespace dotbim.Tekla.Benchmark
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ExportSample();
        }

        private static void ExportSample()
        {
            var settings = new ExportSettings(ExportMode.Selection, @"C:\temp\test.bim");

            var sut = new Exporter();
            sut.Export(settings);
        }
    }
}

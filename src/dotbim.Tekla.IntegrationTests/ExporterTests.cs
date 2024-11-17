﻿using dotbim.Tekla.Engine;
using dotbim.Tekla.Engine.ValueObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotbim.Tekla.IntegrationTests
{
    [TestClass]
    public class ExporterTests
    {
        [TestMethod]
        public void Export_DummyTest()
        {
            var settings = new ExportSettings(ExportMode.Selection, @"C:\temp\test.bim");

            var sut = new Exporter();
            sut.Export(settings);
        }
    }
}
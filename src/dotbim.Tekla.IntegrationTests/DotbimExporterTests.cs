using dotbim;
using dotbimTekla.Engine.Entities;
using dotbimTekla.Engine.Exporters;
using dotbimTekla.Engine.TestHelpers;
using dotbimTekla.Engine.Transformers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tekla.Structures.Geometry3d;

namespace dotbimTekla.IntegrationTests
{
    [TestClass]
    public class DotbimExporterTests
    {
        [TestMethod]
        public void CreateDotbim_ForSingleFace()
        {
            var elements = new List<ElementData>()
            {
                new ElementData(
                    new List<Triangle>()
                    {
                        new Triangle(new Point(0,0,0), new Point(10,0,0), new Point(0,5,0)),
                        new Triangle(new Point(10,0,0), new Point(0,5,0), new Point(10,5,0)),
                    },
                    new Color(),
                    new Dictionary<string,string>())
            };

            var sut = new DotbimExporter();

            var result = sut.CreateDotbim(elements);
        }

        [TestMethod]
        public void DummyTest()
        {
            var identifier = "6d89f5a3-d869-46e2-aac6-b8593c0e8e8c";
            var part = TeklaHelper.GetPart(identifier);

            var result = new TeklaToDomainTransformer().Transform(part);
            var triangles = new SolidTesselator().GetMesh(result);

            var elements = new List<ElementData>()
            {
                new ElementData(
                    triangles,
                    new Color(),
                    new Dictionary<string,string>())
            };

            var sut = new DotbimExporter();

            var dotbim = sut.CreateDotbim(elements);
            dotbim.Save(@"C:\temp\test.bim");
        }
    }
}

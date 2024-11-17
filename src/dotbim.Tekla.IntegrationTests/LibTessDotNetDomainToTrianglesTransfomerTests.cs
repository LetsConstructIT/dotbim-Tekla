using dotbimTekla.Engine.Entities;
using dotbimTekla.Engine.Transformers;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tekla.Structures.Geometry3d;

namespace dotbimTekla.IntegrationTests
{
    [TestClass]
    public class LibTessDotNetDomainToTrianglesTransfomerTests
    {
        [TestMethod]
        public void Transform_ForRectangle_ReturnsTwoTriangles()
        {
            var contour = new Polygon(new Point[]
            {
                new Point(0,0,0),
                new Point(1000,0,0),
                new Point(1000,500,0),
                new Point(0,500,0),
            });
            var face = new XyFace(contour);
            var sut = new LibTessDotNetDomainToXyTrianglesTransfomer();

            var result = sut.Transform(face);
            result.Should().HaveCount(2);
        }
    }
}

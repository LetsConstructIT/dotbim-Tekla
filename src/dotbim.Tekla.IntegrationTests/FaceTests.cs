using dotbim.Tekla.Engine.Entities;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tekla.Structures.Geometry3d;
using TSG = Tekla.Structures.Geometry3d;

namespace dotbim.Tekla.IntegrationTests
{
    [TestClass]
    public class FaceTests
    {
        [TestMethod]
        public void GetCoordinateSystem_ReturnCorrectCS()
        {
            var contour = new Polygon(new Point[]
            {
                new Point(1000,0,0),
                new Point(1000,1000,0)
            });

            var face = new Face(contour, new TSG.Vector(0, 0, 1));

            var result = face.GetCoordinateSystem();
            result.Origin.Should().Be(new Point(1000, 0, 0));
            result.AxisX.Should().Be(new TSG.Vector(0, 1, 0));
            result.AxisY.Should().Be(new TSG.Vector(-1, 0, 0));
        }

        [TestMethod]
        public void TransformToLocal_ForXyFace_ReturnCorrectCS()
        {
            var contour = new Polygon(new Point[]
            {
                new Point(10000,0,5000),
                new Point(10000,10000,5000),
                new Point(0,0,5000)
            });

            var face = new Face(contour, new TSG.Vector(0, 0, 1));

            var result = face.TransformToLocal();
            result.Contour.Points.Should().HaveCount(3);
            result.Contour.Points[0].Should().Be(new Point(0, 0, 0));
            result.Contour.Points[1].Should().Be(new Point(10000, 0, 0));
            result.Contour.Points[2].Should().Be(new Point(0, 10000, 0));
            result.Holes.Should().BeEmpty();
        }

        [TestMethod]
        public void TransformToLocal_ForNonXyFace_ReturnCorrectCS()
        {
            var contour = new Polygon(new Point[]
            {
                new Point(-4447.79,-2266.56,39375.03),
                new Point(-1477.94,-5236.41,39375.03),
                new Point(-1477.94,-5236.41,35775.03),
                new Point(-4447.79,-2266.56,35775.03)
            });

            var normal = new TSG.Vector(new Point(-4447.79, -2266.56, 35775.03) - new Point(-4334.66, -2153.42, 35775.03));

            var face = new Face(contour, normal);

            var result = face.TransformToLocal();
            result.Contour.Points.Should().HaveCount(4);
            result.Contour.Points[0].Should().Be(new Point(0, 0, 0));
            result.Contour.Points[1].Should().Be(new Point(4200.002, 0, 0));
            result.Contour.Points[2].Should().Be(new Point(4200.002, -3600, 0));
            result.Contour.Points[3].Should().Be(new Point(0, -3600, 0));
            result.Holes.Should().BeEmpty();
        }
    }
}

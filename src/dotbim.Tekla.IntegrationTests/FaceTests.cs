using dotbim.Tekla.Engine.Entities;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tekla.Structures.Geometry3d;

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

            var face = new Face(contour, new Vector(0, 0, 1));

            var result = face.GetCoordinateSystem();
            result.Origin.Should().Be(new Point(1000, 0, 0));
            result.AxisX.Should().Be(new Vector(0, 1, 0));
            result.AxisY.Should().Be(new Vector(-1, 0, 0));
        }
    }
}

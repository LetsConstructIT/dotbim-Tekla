using dotbimTekla.Engine.TestHelpers;
using dotbimTekla.Engine.Transformers;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace dotbimTekla.IntegrationTests
{
    [TestClass]
    public class TeklaToDomainTransfomerTests
    {
        [TestMethod]
        public void Transform_ReturnsCorrectGeometry()
        {
            var identifier = "b8d9003c-d9ad-4e31-9a03-84ac09bdbec7";
            var part = TeklaHelper.GetPart(identifier);

            var sut = new TeklaToDomainTransformer();

            var result = sut.Transform(part);

            result.Faces.Should().HaveCount(6);
            result.Faces.First().Contour.Points.Should().HaveCount(4);
        }

        [TestMethod]
        public void DummyTest()
        {
            var identifier = "506929d7-0000-0028-3133-343930373137";
            var part = TeklaHelper.GetPart(identifier);

            var sut = new TeklaToDomainTransformer();

            var result = sut.Transform(part);
            var triangles = new SolidTesselator().GetMesh(result);
            var drawer = new PolylineDrawer();
            foreach (var tri in triangles)
            {
                drawer.Draw(tri);
            }
        }
        
    }
}

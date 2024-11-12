using dotbim.Tekla.Engine.TestHelpers;
using dotbim.Tekla.Engine.Transformers;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace dotbim.Tekla.IntegrationTests
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
    }
}

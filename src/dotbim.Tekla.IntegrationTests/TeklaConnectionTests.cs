using dotbimTekla.Engine;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace dotbimTekla.IntegrationTests
{
    [TestClass]
    public class TeklaConnectionTests
    {
        [TestMethod]
        public void GetModelName_Should_ReturnName()
        {
            var sut = new TeklaProjectInfoQuery();

            var result = sut.GetModelName();

            Assert.AreNotEqual(string.Empty, result);
        }
    }
}

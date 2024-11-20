using dotbimTekla.Engine.Exporters.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace dotbimTekla.IntegrationTests
{
    [TestClass]
    public class PropertySetBuilderTests
    {
        [TestMethod]
        public void GetDemandedProperties_DummyTest()
        {
            var path = @"C:\TeklaStructures\2024.0\Environments\common\system\AdditionalPSets\Tekla PowerFab.xml";

            var sut = new PropertySetBuilder();

            var result = sut.GetNeededProperties(path);
        }
    }
}

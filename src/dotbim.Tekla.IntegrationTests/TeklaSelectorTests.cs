using dotbim.Tekla.Engine.Selectors;
using dotbim.Tekla.Engine.TestHelpers;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace dotbim.Tekla.IntegrationTests
{
    [TestClass]
    public class TeklaSelectorTests
    {

        [TestMethod]
        public void ForAssemblies_Get_ReturnsMoreThanOneObject()
        {
            var sut = new TeklaAssemblies();

            var objects = sut.Get();

            objects.Should().HaveCountGreaterThan(1);
        }

        [TestMethod]
        public void ForSelection_Get_ReturnNothing()
        {
            TeklaHelper.UnselectAll();

            var sut = new SelectedObjects();

            var objects = sut.Get();

            objects.Should().BeEmpty();
        }

        [TestMethod]
        public void ForSelection_Get_ForSelected_ReturnsOneObject()
        {
            TeklaHelper.Select("4f0d9ace-0000-0004-3133-323632393136");

            var sut = new SelectedObjects();

            var objects = sut.Get();

            objects.Should().HaveCount(1);
        }
    }
}

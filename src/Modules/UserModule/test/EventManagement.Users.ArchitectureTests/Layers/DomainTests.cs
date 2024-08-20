using EventManagement.Users.ArchitectureTests.Abstractions;
using NetArchTest.Rules;

namespace EventManagement.Users.ArchitectureTests.Layers
{
    public class LayerTests : TestBase
    {
        [Fact]
        public void DomainLayer_ShouldNotHaveDependencyOn_ApplicationLayer()
        {
            Types.InAssembly(DomainAssembly)
                .Should().NotHaveDependencyOn(ApplicationAssembly.GetName().Name)
                .GetResult().ShouldBeSuccessful();
        }

        [Fact]
        public void DomainLayer_ShouldNotHaveDependencyOn_InfrastructureLayer()
        {
            Types.InAssembly(DomainAssembly)
                .Should().NotHaveDependencyOn(InfrastructureAssembly.GetName().Name)
                .GetResult().ShouldBeSuccessful();
        }


        [Fact]
        public void ApplicationLayer_ShouldNotHaveDependencyOn_InfrastructureLayer()
        {
            Types.InAssembly(ApplicationAssembly)
                .Should().NotHaveDependencyOn(InfrastructureAssembly.GetName().Name)
                .GetResult().ShouldBeSuccessful();
        }
    }
}

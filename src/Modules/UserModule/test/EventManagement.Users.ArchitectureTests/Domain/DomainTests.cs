using EventManagement.Common.Domain;
using EventManagement.Common.Domain.DomainEvents;
using EventManagement.Users.ArchitectureTests.Abstractions;
using FluentAssertions;
using NetArchTest.Rules;
using System.Reflection;

namespace EventManagement.Users.ArchitectureTests.Domain
{
    public class DomainTests : TestBase
    {
        #region DomainEvent Tests
        [Fact]
        public void DomainEvents_Should_BeSealed()
        {
            Types.InAssembly(DomainAssembly)
                .That().ImplementInterface(typeof(IDomainEvent))
                .Should().BeSealed()
                .GetResult().ShouldBeSuccessful();
        }
        [Fact]
        public void DomainEvents_ShouldHave_DomainEventPostfix()
        {
            Types.InAssembly(DomainAssembly)
                .That().ImplementInterface(typeof(IDomainEvent))
                .Should().HaveNameEndingWith("DomainEvent")
                .GetResult().ShouldBeSuccessful();
        }
        #endregion
        
        #region Entity Tests
        [Fact]
        public void Entities_ShouldHave_PrivateParameterlessConstructor()
        {
            var entityTypes = Types.InAssembly(DomainAssembly)
                .That().Inherit(typeof(Entity))
                .GetTypes();

            var failedTypes = new List<Type>();
            foreach (var entityType in entityTypes)
            {
                var constructors = entityType.GetConstructors(BindingFlags.Public | BindingFlags.Instance);

                if (constructors.Any())
                {
                    failedTypes.Add(entityType);
                }
            }

            failedTypes.Should().BeEmpty();
        }

        [Fact]
        public void Entities_ShouldOnlyHave_PrivateConstructors()
        {
            var entityTypes = Types.InAssembly(DomainAssembly)
                .That().Inherit(typeof(Entity))
                .GetTypes();

            var failedTypes = new List<Type>();
            foreach (var entityType in entityTypes)
            {
                var constructors = entityType.GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance);

                if (!constructors.Any(c => c.IsPrivate && c.GetParameters().Length == 0))
                {
                    failedTypes.Add(entityType);
                }
            }

            failedTypes.Should().BeEmpty();
        }
        #endregion
    }
}

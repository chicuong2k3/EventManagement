using System.Reflection;

namespace EventManagement.Users.ArchitectureTests.Abstractions
{
    public abstract class TestBase
    {
        protected static readonly Assembly ApplicationAssembly
            = typeof(EventManagement.Users.Application.AssemblyReference).Assembly;

        protected static readonly Assembly DomainAssembly
            = typeof(EventManagement.Users.Domain.Entities.User).Assembly;

        protected static readonly Assembly InfrastructureAssembly
            = typeof(EventManagement.Users.Infrastructure.DependencyInjection).Assembly;


    }
}

using System.Reflection;
using EventManagement.Users.Domain.Users;

namespace EventManagement.Users.ArchitectureTests.Abstractions
{
    public abstract class TestBase
    {
        protected static readonly Assembly ApplicationAssembly
            = typeof(EventManagement.Users.Application.AssemblyReference).Assembly;

        protected static readonly Assembly DomainAssembly
            = typeof(User).Assembly;

        protected static readonly Assembly InfrastructureAssembly
            = typeof(EventManagement.Users.Infrastructure.DependencyInjection).Assembly;


    }
}

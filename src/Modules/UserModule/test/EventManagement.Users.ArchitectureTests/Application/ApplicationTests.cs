using EventManagement.Common.Application.Messaging;
using EventManagement.Users.ArchitectureTests.Abstractions;
using FluentValidation;
using NetArchTest.Rules;

namespace EventManagement.Users.ArchitectureTests.Application
{
    public class ApplicationTests : TestBase
    {
        #region Command Tests
        [Fact]
        public void Commands_Should_BeSealed()
        {
            Types.InAssembly(ApplicationAssembly)
                .That().ImplementInterface(typeof(ICommand))
                .Or().ImplementInterface(typeof(ICommand<>))
                .Should().BeSealed()
                .GetResult().ShouldBeSuccessful();
        }

        [Fact]
        public void Commands_ShouldHave_CommandPostfix()
        {
            Types.InAssembly(ApplicationAssembly)
                .That().ImplementInterface(typeof(ICommand))
                .Or().ImplementInterface(typeof(ICommand<>))
                .Should().HaveNameEndingWith("Command")
                .GetResult().ShouldBeSuccessful();
        }
        #endregion

        #region CommandHandler Tests
        [Fact]
        public void CommandHandlers_Should_NotBePublic()
        {
            Types.InAssembly(ApplicationAssembly)
                .That().ImplementInterface(typeof(ICommandHandler<>))
                .Or().ImplementInterface(typeof(ICommandHandler<,>))
                .Should().NotBePublic()
                .GetResult().ShouldBeSuccessful();
        }

        [Fact]
        public void CommandHandlers_ShouldHave_CommandHandlerPostfix()
        {
            Types.InAssembly(ApplicationAssembly)
                .That().ImplementInterface(typeof(ICommandHandler<>))
                .Or().ImplementInterface(typeof(ICommandHandler<,>))
                .Should().HaveNameEndingWith("CommandHandler")
                .GetResult().ShouldBeSuccessful();
        }
        #endregion

        #region Query Tests
        [Fact]
        public void Queries_Should_BeSealed()
        {
            Types.InAssembly(ApplicationAssembly)
                .That().ImplementInterface(typeof(IQuery<>))
                .Should().BeSealed()
                .GetResult().ShouldBeSuccessful();
        }

        [Fact]
        public void Queries_ShouldHave_QueryPostfix()
        {
            Types.InAssembly(ApplicationAssembly)
                .That().ImplementInterface(typeof(ICommand<>))
                .Should().HaveNameEndingWith("Query")
                .GetResult().ShouldBeSuccessful();
        }
        #endregion

        #region QueryHandler Tests
        [Fact]
        public void QueryHandlers_Should_NotBePublic()
        {
            Types.InAssembly(ApplicationAssembly)
                .That().ImplementInterface(typeof(IQueryHandler<,>))
                .Should().NotBePublic()
                .GetResult().ShouldBeSuccessful();
        }

        [Fact]
        public void QueryHandlers_ShouldHave_QueryHandlerPostfix()
        {
            Types.InAssembly(ApplicationAssembly)
                .That().ImplementInterface(typeof(IQueryHandler<,>))
                .Should().HaveNameEndingWith("QueryHandler")
                .GetResult().ShouldBeSuccessful();
        }
        #endregion

        #region Validator Tests
        [Fact]
        public void Validators_Should_BeSealed()
        {
            Types.InAssembly(ApplicationAssembly)
                .That().Inherit(typeof(AbstractValidator<>))
                .Should().BeSealed()
                .GetResult().ShouldBeSuccessful();
        }

        [Fact]
        public void Validators_ShouldHave_ValidatorPostfix()
        {
            Types.InAssembly(ApplicationAssembly)
                .That().Inherit(typeof(AbstractValidator<>))
                .Should().HaveNameEndingWith("Validator")
                .GetResult().ShouldBeSuccessful();
        }
        #endregion

        #region DomainEventHandler Tests
        [Fact]
        public void DomainEventHandlers_Should_BeSealed()
        {
            Types.InAssembly(ApplicationAssembly)
                .That().ImplementInterface(typeof(IDomainEventHandler<>))
                .Should().BeSealed()
                .GetResult().ShouldBeSuccessful();
        }

        [Fact]
        public void DomainEventHandlers_Should_NotBePublic()
        {
            Types.InAssembly(ApplicationAssembly)
                .That().ImplementInterface(typeof(IDomainEventHandler<>))
                .Should().NotBePublic()
                .GetResult().ShouldBeSuccessful();
        }
        [Fact]
        public void DomainEventHandlers_ShouldHave_DomainEventHandlerPostfix()
        {
            Types.InAssembly(ApplicationAssembly)
                .That().ImplementInterface(typeof(IDomainEventHandler<>))
                .Should().HaveNameEndingWith("DomainEventHandler")
                .GetResult().ShouldBeSuccessful();
        }
        #endregion
    }
}

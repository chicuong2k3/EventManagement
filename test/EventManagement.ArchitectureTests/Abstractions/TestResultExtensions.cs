using FluentAssertions;
using NetArchTest.Rules;

namespace EventManagement.ArchitectureTests.Abstractions
{
    public static class TestResultExtensions
    {
        internal static void ShouldBeSuccessful(this TestResult result)
        {
            result.FailingTypes?.Should().BeEmpty();
        }
    }
}
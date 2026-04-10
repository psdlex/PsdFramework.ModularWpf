using FluentAssertions;
using PsdFramework.ModularWpf.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PsdFramework.ModularWpf.Tests.Unit.ContextualParameters;

public sealed class ContextualParametersTests
{
    [Fact]
    public void AddAndBuild_ShouldGetAllValuesAndThrowRequiredException()
    {
        var builder = new ContextualParametersBuilder();
        builder.Add("p1", "value");
        builder.Add(123);

        var parameters = builder.Build();

        parameters.Invoking(p => p.GetValue<int>("p1")).Should().Throw<InvalidCastException>();
        parameters.Invoking(p => p.GetValue<string>("p2")).Should().Throw<KeyNotFoundException>();
        parameters.Invoking(p => p.GetValue<string>()).Should().Throw<KeyNotFoundException>();

        parameters.GetValue<string>("p1").Should().Be("value");
        parameters.GetValue<int>().Should().Be(123);

        parameters.TryGetValue<string>("p1", out var p1Value).Should().BeTrue();
        p1Value.Should().Be("value");

        parameters.TryGetValue<int>(out var intValue).Should().BeTrue();
        intValue.Should().Be(123);
    }
}

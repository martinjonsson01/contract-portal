﻿using Client.Pages.Contracts;

using Domain.Contracts;

using FluentAssertions.Execution;

namespace Presentation.Tests.Client.Pages.Contracts;

public class ContractDetailsTests : UITestFixture
{
    [Fact]
    public void InspirationalImage_IsShown_WhenItExists()
    {
        // Arrange
        const string inspirationalImagePath = "images/inspirational.jpg";
        const string logoImagePath = "images/logo.jpg";
        var contract = new Contract
        {
            InspirationalImagePath = inspirationalImagePath,
            SupplierLogoImagePath = logoImagePath,
        };

        void ParameterBuilder(ComponentParameterCollectionBuilder<ContractDetails> parameters) =>
            parameters.Add(property => property.Contract, contract);

        const string thumbnail = "#contract-thumbnail";

        // Act
        IRenderedComponent<ContractDetails> cut = Context.RenderComponent<ContractDetails>(ParameterBuilder);
        cut.WaitForElement(thumbnail);

        // Assert
        using (new AssertionScope())
        {
            string? srcValue = cut.Find(thumbnail).Attributes.GetNamedItem("src")?.Value;
            srcValue.Should().NotBeNull();
            srcValue.Should().Be(inspirationalImagePath);
        }
    }

    [Fact]
    public void SupplierLogo_IsShown_WhenInspirationalImageDoesNotExist()
    {
        // Arrange
        const string inspirationalImagePath = "";
        const string logoImagePath = "images/logo.jpg";
        var contract = new Contract
        {
            InspirationalImagePath = inspirationalImagePath,
            SupplierLogoImagePath = logoImagePath,
        };

        void ParameterBuilder(ComponentParameterCollectionBuilder<ContractDetails> parameters) =>
            parameters.Add(property => property.Contract, contract);

        const string thumbnail = "#contract-thumbnail";

        // Act
        IRenderedComponent<ContractDetails> cut = Context.RenderComponent<ContractDetails>(ParameterBuilder);
        cut.WaitForElement(thumbnail);

        // Assert
        using (new AssertionScope())
        {
            string? srcValue = cut.Find(thumbnail).Attributes.GetNamedItem("src")?.Value;
            srcValue.Should().NotBeNull();
            srcValue.Should().Be(logoImagePath);
        }
    }

    [Fact]
    public void FAQSection_IsShown_WhenFAQExists()
    {
        // Arrange
        var contract = new Contract
        {
            FAQ = "Frequently asked question",
        };

        void ParameterBuilder(ComponentParameterCollectionBuilder<ContractDetails> parameters) =>
            parameters.Add(property => property.Contract, contract);

        const string section = "#contract-FAQ";

        // Act
        IRenderedComponent<ContractDetails> cut = Context.RenderComponent<ContractDetails>(ParameterBuilder);
        cut.WaitForElement(section);

        // Assert
        cut.Find(section).TextContent.Should().Contain("Vanliga frågor");
    }

    [Fact]
    public void FAQSection_IsNotShown_WhenThereIsNoContractFAQ()
    {
        // Arrange
        Contract contract = new();

        void ParameterBuilder(ComponentParameterCollectionBuilder<ContractDetails> parameters) =>
            parameters.Add(property => property.Contract, contract);

        const string section = "#contract-FAQ";

        // Act
        IRenderedComponent<ContractDetails> cut = Context.RenderComponent<ContractDetails>(ParameterBuilder);
        cut.WaitForElement(section);

        // Assert
        cut.Find(section).MarkupMatches("<div id=\"contract-FAQ\"></div>");
    }
}

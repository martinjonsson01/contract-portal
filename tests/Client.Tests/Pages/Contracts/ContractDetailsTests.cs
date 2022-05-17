using System.Threading.Tasks;

using AngleSharp.Dom;

using Application.Users;

using Blazored.SessionStorage;

using Client.Pages.Contracts;
using Client.Services.Authentication;

using Domain.Contracts;
using Domain.Users;

using FluentAssertions.Execution;

using Microsoft.Extensions.DependencyInjection;

namespace Client.Tests.Pages.Contracts;

public class ContractDetailsTests : UITestFixture
{
    [Fact]
    public async Task InspirationalImage_IsShown_WhenItExists()
    {
        // Arrange
        await SessionStorage.SetItemAsync("user", new AuthenticateResponse(LoggedInUser, FakeToken));

        const string inspirationalImagePath = "images/inspirational.jpg";
        const string logoImagePath = "images/logo.jpg";
        var contract = new Contract
        {
            InspirationalImagePath = inspirationalImagePath, SupplierLogoImagePath = logoImagePath,
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
    public async Task SupplierLogo_IsShown_WhenInspirationalImageDoesNotExist()
    {
        // Arrange
        await SessionStorage.SetItemAsync("user", new AuthenticateResponse(LoggedInUser, FakeToken));

        const string inspirationalImagePath = "";
        const string logoImagePath = "images/logo.jpg";
        var contract = new Contract
        {
            InspirationalImagePath = inspirationalImagePath, SupplierLogoImagePath = logoImagePath,
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
    public async Task FAQSection_ContainsCorrectFAQ()
    {
        // Arrange
        await SessionStorage.SetItemAsync("user", new AuthenticateResponse(LoggedInUser, FakeToken));

        string faqText = "Frequently asked question";

        var contract = new Contract { FAQ = faqText, };

        void ParameterBuilder(ComponentParameterCollectionBuilder<ContractDetails> parameters) =>
            parameters.Add(property => property.Contract, contract);

        // Act
        IRenderedComponent<ContractDetails> cut = Context.RenderComponent<ContractDetails>(ParameterBuilder);
        IElement? faqElement = cut.FindAll(".accordion-body").ToList()
                                  .Find(p => p.InnerHtml.Contains(faqText, StringComparison.CurrentCulture));

        // Assert
        faqElement.Should().NotBeNull();
    }

    [Fact]
    public async Task FAQSection_IsShown_WhenFAQExists()
    {
        // Arrange
        await SessionStorage.SetItemAsync("user", new AuthenticateResponse(LoggedInUser, FakeToken));

        var contract = new Contract { FAQ = "Frequently asked question", };

        void ParameterBuilder(ComponentParameterCollectionBuilder<ContractDetails> parameters) =>
            parameters.Add(property => property.Contract, contract);

        // Act
        IRenderedComponent<ContractDetails> cut = Context.RenderComponent<ContractDetails>(ParameterBuilder);
        IElement? titleElement = cut.FindAll(".accordion-item").ToList()
                                    .Find(p => p.InnerHtml.Contains("faq-title", StringComparison.CurrentCulture));

        // Assert
        titleElement.Should().NotBeNull();
    }

    [Fact]
    public async Task FAQSection_IsNotShown_WhenThereIsNoContractFAQ()
    {
        // Arrange
        await SessionStorage.SetItemAsync("user", new AuthenticateResponse(LoggedInUser, FakeToken));

        var contract = new Contract { FAQ = string.Empty, };

        void ParameterBuilder(ComponentParameterCollectionBuilder<ContractDetails> parameters) =>
            parameters.Add(property => property.Contract, contract);

        // Act
        IRenderedComponent<ContractDetails> cut = Context.RenderComponent<ContractDetails>(ParameterBuilder);
        IElement? titleElement = cut.FindAll(".accordion-item").ToList()
                                    .Find(p => p.InnerHtml.Contains("faq-title", StringComparison.CurrentCulture));

        // Assert
        titleElement.Should().BeNull();
    }

    [Fact]
    public void RegisterPrompt_IsShown_WhenUserIsNotLoggedIn()
    {
        // Arrange
        MockSession.Setup(session => session.IsAuthenticated).Returns(false);

        void ParameterBuilder(ComponentParameterCollectionBuilder<ContractDetails> parameters) =>
            parameters.Add(property => property.Contract, new Contract());

        // Act
        IRenderedComponent<ContractDetails> cut = Context.RenderComponent<ContractDetails>(ParameterBuilder);

        // Assert
        Action findPrompt = () => cut.Find(".register-prompt");
        cut.WaitForAssertion(() => findPrompt.Should().NotThrow());
    }

    [Fact]
    public async Task RegisterPrompt_IsNotShown_WhenUserIsLoggedIn()
    {
        // Arrange
        await SessionStorage.SetItemAsync("user", new AuthenticateResponse(LoggedInUser, FakeToken));

        void ParameterBuilder(ComponentParameterCollectionBuilder<ContractDetails> parameters) =>
            parameters.Add(property => property.Contract, new Contract());

        // Act
        IRenderedComponent<ContractDetails> cut = Context.RenderComponent<ContractDetails>(ParameterBuilder);

        // Assert
        Action findPrompt = () => cut.Find(".register-prompt");
        cut.WaitForAssertion(() => findPrompt.Should().Throw<ElementNotFoundException>());
    }

    [Fact]
    public async Task AdditionalDocumentSection_IsShown_WhenAdditionalDocumentExists()
    {
        // Arrange
        await SessionStorage.SetItemAsync("user", new AuthenticateResponse(LoggedInUser, FakeToken));

        var contract = new Contract { AdditionalDocument = "/link/to/additional.document", };

        void ParameterBuilder(ComponentParameterCollectionBuilder<ContractDetails> parameters) =>
            parameters.Add(property => property.Contract, contract);

        // Act
        IRenderedComponent<ContractDetails> cut = Context.RenderComponent<ContractDetails>(ParameterBuilder);

        // Assert
        cut.WaitForAssertion(() => cut.Find("#additional-document").Should().NotBeNull());
    }

    [Fact]
    public async Task AdditionalDocumentSectionSection_IsNotShown_WhenThereIsNoAdditionalDocument()
    {
        // Arrange
        await SessionStorage.SetItemAsync("user", new AuthenticateResponse(LoggedInUser, FakeToken));

        var contract = new Contract { AdditionalDocument = string.Empty, };

        void ParameterBuilder(ComponentParameterCollectionBuilder<ContractDetails> parameters) =>
            parameters.Add(property => property.Contract, contract);

        // Act
        IRenderedComponent<ContractDetails> cut = Context.RenderComponent<ContractDetails>(ParameterBuilder);
        Action findLink = () => cut.Find("#additional-document");

        // Assert
        cut.WaitForAssertion(() => findLink.Should().Throw<ElementNotFoundException>());
    }
}

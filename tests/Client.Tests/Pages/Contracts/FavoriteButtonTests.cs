﻿using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AngleSharp.Dom;
using Bunit;
using Client.Pages.Contracts;

using Domain.Contracts;
using Microsoft.AspNetCore.Components.Web;

namespace Client.Tests.Pages.Contracts;

public class FavoriteButtonTests : UITestFixture
{
    public FavoriteButtonTests(ITestOutputHelper outputHelper)
        : base(outputHelper)
    {
    }

    [Fact]
    public void ContractCard_ShowsFavoriteIcon_WhenContractIsFavoriteMarked()
    {
        // Arrange
        var userId = Guid.NewGuid();
        Contract contract = new();

        void ParameterBuilder(ComponentParameterCollectionBuilder<FavoriteButton> parameters) =>
            parameters.Add(property => property.Contract, contract);

        MockHttp.When($"/api/v1/users/{userId}/favorites/{contract.Id}").Respond(req => new HttpResponseMessage(HttpStatusCode.OK));
        MockSession.Setup(session => session.UserId).Returns(userId);

        // Act
        IRenderedComponent<FavoriteButton> cut =
            Context.RenderComponent<FavoriteButton>(ParameterBuilder);

        // Assert
        cut.WaitForAssertion(() => cut.Find(".bi-heart-fill").Should().NotBeNull());
        Action tryFind = () => cut.Find(".bi-heart");
        cut.WaitForAssertion(() => tryFind.Should().Throw<ElementNotFoundException>());
    }

    [Fact]
    public void ContractCard_ShowsNonFavoriteIcon_WhenContractIsNotFavoriteMarked()
    {
        // Arrange
        var userId = Guid.NewGuid();
        Contract contract = new();

        void ParameterBuilder(ComponentParameterCollectionBuilder<FavoriteButton> parameters) =>
            parameters.Add(property => property.Contract, contract);

        MockHttp.When($"/api/v1/users/{userId}/favorites/{contract.Id}").Respond(req => new HttpResponseMessage(HttpStatusCode.NotFound));
        MockSession.Setup(session => session.UserId).Returns(userId);

        // Act
        IRenderedComponent<FavoriteButton> cut =
            Context.RenderComponent<FavoriteButton>(ParameterBuilder);
        cut.WaitForElement(".bi-heart");

        // Assert
        cut.WaitForAssertion(() => cut.Find(".bi-heart").Should().NotBeNull());
        Action tryFind = () => cut.Find(".bi-heart-fill");
        cut.WaitForAssertion(() => tryFind.Should().Throw<ElementNotFoundException>());
    }

    [Fact]
    public async Task FavoriteButton_CallsOnFavoriteChange_WhenClickedAsync()
    {
        // Arrange
        var userId = Guid.NewGuid();
        Contract contract = new();

        bool eventCalled = false;

        MockHttp.When($"/api/v1/users/{userId}/favorites/{contract.Id}").Respond(req => new HttpResponseMessage(HttpStatusCode.OK));
        MockHttp.When(HttpMethod.Post, $"/api/v1/users/{userId}/favorites").Respond(HttpStatusCode.OK);
        MockSession.Setup(session => session.UserId).Returns(userId);

        void ParameterBuilder(ComponentParameterCollectionBuilder<FavoriteButton> parameters) =>
                      parameters.Add(property => property.Contract, contract)
                                .Add(property => property.OnFavoriteChange, () => { eventCalled = true; });

        IRenderedComponent<FavoriteButton> cut = Context.RenderComponent<FavoriteButton>(ParameterBuilder);

        cut.WaitForElement("#favorite-button");

        // Act
        await cut.Find("#favorite-button").ClickAsync(new MouseEventArgs());

        // Assert
        cut.WaitForAssertion(() => eventCalled.Should().BeTrue());
    }

    [Fact]
    public void FavoriteButton_DoesNotChangeAppearance_WhenRequestFailed()
    {
        // Arrange
        var userId = Guid.NewGuid();
        Contract contract = new();

        MockHttp.When($"/api/v1/users/{userId}/favorites/{contract.Id}").Respond(HttpStatusCode.OK); // Check whether the contract is marked as favorite and receive that it is
        MockHttp.When(HttpMethod.Post, $"/api/v1/users/{userId}/favorites").Respond(HttpStatusCode.BadRequest); // Post a request to unmark the contract as a favorite and receive that it was not unmarked
        MockSession.Setup(session => session.UserId).Returns(userId);

        void ParameterBuilder(ComponentParameterCollectionBuilder<FavoriteButton> parameters) =>
                      parameters.Add(property => property.Contract, contract);

        IRenderedComponent<FavoriteButton> cut = Context.RenderComponent<FavoriteButton>(ParameterBuilder);

        cut.WaitForElement(".bi-heart-fill");
        IElement beforeClicked = cut.Find(".bi-heart-fill");
        cut.WaitForElement("#favorite-button");

        // Act
        cut.Find("#favorite-button").Click();
        cut.WaitForElement(".bi-heart-fill");
        IElement afterClicked = cut.Find(".bi-heart-fill");

        // Assert
        cut.WaitForAssertion(() => beforeClicked.Should().NotBeNull());
        cut.WaitForAssertion(() => afterClicked.Should().NotBeNull());
    }

    [Fact]
    public void FavoriteButton_ChangesAppearance_WhenRequestSucceeds()
    {
        // Arrange
        var userId = Guid.NewGuid();
        Contract contract = new();

        MockHttp.When($"/api/v1/users/{userId}/favorites/{contract.Id}").Respond(req => new HttpResponseMessage(HttpStatusCode.OK)); // Check whether the contract is marked as favorite and receive that it is
        MockHttp.When(HttpMethod.Post, $"/api/v1/users/{userId}/favorites").Respond(req => new HttpResponseMessage(HttpStatusCode.OK)); // Post a request to unmark the contract as a favorite and receive that is was
        MockSession.Setup(session => session.UserId).Returns(userId);

        void ParameterBuilder(ComponentParameterCollectionBuilder<FavoriteButton> parameters) =>
            parameters.Add(property => property.Contract, contract);

        IRenderedComponent<FavoriteButton> cut = Context.RenderComponent<FavoriteButton>(ParameterBuilder);

        cut.WaitForElement(".bi-heart-fill");
        IElement beforeClicked = cut.Find(".bi-heart-fill");
        cut.WaitForElement("#favorite-button");

        // Act
        cut.Find("#favorite-button").Click();
        cut.WaitForElement(".bi-heart");
        IElement afterClicked = cut.Find(".bi-heart");

        // Assert
        cut.WaitForAssertion(() => beforeClicked.Should().NotBeNull());
        cut.WaitForAssertion(() => afterClicked.Should().NotBeNull());
    }
}

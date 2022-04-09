﻿using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using Client.Pages.Admin;

using Domain.Contracts;

namespace Presentation.Tests.Client.Pages.Admin;

public class AdminPageTests : UITestFixture
{
    [Fact]
    public void AdminPage_ShouldSayLoading_WhenThereAreNoContractsFetched()
    {
        // Arrange
        MockHttp.When("/api/v1/Contracts/All").Respond(async () =>
        {
            // Simulate slow network.
            await Task.Delay(TimeSpan.FromSeconds(10)).ConfigureAwait(false);
            return new HttpResponseMessage(HttpStatusCode.OK);
        });

        // Act
        IRenderedComponent<ContractsPage> cut = Context.RenderComponent<ContractsPage>();

        // Assert
        cut.Find("p em").TextContent.Should().BeEquivalentTo("Loading...");
    }

    [Fact]
    public void AdminPage_ShouldContainCorrectAmountOfContract()
    {
        // Arrange
        Context.JSInterop.SetupVoid("Blazor._internal.InputFile.init", _ => true);
        IEnumerable<Contract> fakeContracts = new Faker<Contract>().Generate(10);
        MockHttp.When("/api/v1/Contracts/All").RespondJson(fakeContracts);

        // Act
        IRenderedComponent<AdminPage> cut = Context.RenderComponent<AdminPage>();
        cut.WaitForElement(".list-group");

        // Assert
        cut.FindAll(".list-group-item").Count.Should().Be(10);
    }
}

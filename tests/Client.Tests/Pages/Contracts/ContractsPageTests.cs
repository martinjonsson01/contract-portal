﻿using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using Client.Pages.Contracts;

using Domain.Contracts;

namespace Client.Tests.Pages.Contracts;

public class ContractsPageTests : UITestFixture
{
    public ContractsPageTests(ITestOutputHelper outputHelper)
        : base(outputHelper)
    {
    }

    [Fact]
    public void ContractPage_ShouldShowLoadingIndicator_WhenThereAreNoContractsFetched()
    {
        // Arrange
        MockHttp.When("/api/v1/contracts").Respond(async () =>
        {
            // Simulate slow network.
            await Task.Delay(TimeSpan.FromSeconds(10)).ConfigureAwait(false);
            return new HttpResponseMessage(HttpStatusCode.OK);
        });
        MockHttp.When("/api/v1/contracts/recent").RespondJson(Array.Empty<object>());

        // Act
        IRenderedComponent<ContractsPage> cut = Context.RenderComponent<ContractsPage>();

        // Assert
        cut.WaitForElement(".loading-indicator");
    }

    [Fact]
    public void ContractPage_ShouldContainContract_WhenItHasFetchedAContract()
    {
        // Arrange
        const string name = "SJ";
        var contract = new Contract() { Name = name, SupplierLogoImagePath = "/img/test", };
        MockHttp.When("/api/v1/contracts").RespondJson(new[] { contract, });
        MockHttp.When("/api/v1/contracts/recent").RespondJson(Array.Empty<object>());

        // Act
        IRenderedComponent<ContractsPage> cut = Context.RenderComponent<ContractsPage>();

        // Assert
        cut.WaitForAssertion(() => cut.Find(".card").TextContent.Should().Contain(name));
    }
}

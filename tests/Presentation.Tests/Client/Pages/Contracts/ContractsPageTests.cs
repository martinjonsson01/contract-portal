using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using Client.Pages.Contracts;

using Domain.Contracts;

namespace Presentation.Tests.Client.Pages.Contracts;

public class ContractsPageTests : UITestFixture
{
    [Fact]
    public void ContractPage_ShouldSayLoading_WhenThereAreNoContractsFetched()
    {
        // Arrange
        MockHttp.When("/api/v1/contracts").Respond(async () =>
        {
            // Simulate slow network.
            await Task.Delay(TimeSpan.FromSeconds(10)).ConfigureAwait(false);
            return new HttpResponseMessage(HttpStatusCode.OK);
        });

        // Act
        IRenderedComponent<ContractsPage> cut = Context.RenderComponent<ContractsPage>();

        // Assert
        cut.Find("p").TextContent.Should().BeEquivalentTo("Laddar...");
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
        cut.WaitForElement(".card");

        // Assert
        cut.Find(".card").TextContent.Should().Contain(name);
    }
}

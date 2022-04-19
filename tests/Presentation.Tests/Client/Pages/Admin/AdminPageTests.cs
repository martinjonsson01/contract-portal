using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using Client.Pages.Admin;
using Client.Pages.Contracts;

using Domain.Contracts;

namespace Presentation.Tests.Client.Pages.Admin;

public class AdminPageTests : UITestFixture
{
    [Fact]
    public void AdminPage_ShouldSayLoading_WhenThereAreNoContractsFetched()
    {
        // Arrange
        MockHttp.When("/api/v1/contracts").Respond(async () =>
        {
            // Simulate slow network.
            await Task.Delay(TimeSpan.FromSeconds(10)).ConfigureAwait(false);
            return new HttpResponseMessage(HttpStatusCode.OK);
        });
        MockHttp.When("/api/v1/contracts/recent").RespondJson(new List<Contract>());

        // Act
        IRenderedComponent<ContractsPage> cut = Context.RenderComponent<ContractsPage>();

        // Assert
        cut.Find("p").TextContent.Should().BeEquivalentTo("Laddar...");
    }

    [Fact]
    public void AdminPage_ShouldContainCorrectAmountOfContract()
    {
        // Arrange
        Context.JSInterop.SetupVoid("Blazor._internal.InputFile.init", _ => true);
        IEnumerable<Contract> fakeContracts = new Faker<Contract>().Generate(10);
        MockHttp.When("/api/v1/contracts").RespondJson(fakeContracts);

        // Act
        IRenderedComponent<AdminPage> cut = Context.RenderComponent<AdminPage>();
        cut.WaitForElement(".list-group");

        // Assert
        cut.FindAll(".list-group-item").Count.Should().Be(10);
    }
}

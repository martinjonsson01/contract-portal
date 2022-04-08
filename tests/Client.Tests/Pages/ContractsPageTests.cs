using Domain.Contracts;

namespace Client.Tests.Pages;

public class ContractsPageTests : BlazorTestFixture
{
    [Fact]
    public void ContractPage_ShouldSayLoading_WhenThereAreNoContractsFetched()
    {
        // Arrange
        MockHttp.When("/").RespondJson(new Contract[1]);

        // Act
        IRenderedComponent<ContractsPage> cut = Context.RenderComponent<ContractsPage>();

        // Assert
        cut.Find("p em").TextContent.Should().BeEquivalentTo("Loading...");
    }

    [Fact]
    public void ContractPage_ShouldContainContract_WhenItHasFetchedAContract()
    {
        // Arrange
        const string name = "SJ";
        var contract = new Contract() { Name = name, ImagePath = "/img/test" };
        MockHttp.When("/api/v1/Contracts/All").RespondJson(new[] { contract });

        // Act
        IRenderedComponent<ContractsPage> cut = Context.RenderComponent<ContractsPage>();
        cut.WaitForElement(".card");

        // Assert
        cut.Find(".card").TextContent.Should().Contain(name);
    }
}

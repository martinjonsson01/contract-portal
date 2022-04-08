using Domain.Contracts;

namespace Presentation.Tests.Client.Pages;

public class AdminPageTests : UITestFixture
{
    [Fact]
    public void AdminPage_ShouldSayLoading_WhenThereAreNoContractsFetched()
    {
        // Act
        IRenderedComponent<ContractsPage> cut = Context.RenderComponent<ContractsPage>();

        // Assert
        cut.Find("p em").TextContent.Should().BeEquivalentTo("Loading...");
    }

    [Fact]
    public void AdminPage_ShouldContainCorrectAmountOfContract()
    {
        // Arrange
        IEnumerable<Contract> fakeContracts = new Faker<Contract>().Generate(10);
        MockHttp.When("/api/v1/Contracts/All").RespondJson(fakeContracts);

        // Act
        IRenderedComponent<Admin> cut = Context.RenderComponent<Admin>();
        cut.WaitForElement(".list-group");

        // Assert
        cut.FindAll(".list-group-item").Count.Should().Be(10);
    }
}

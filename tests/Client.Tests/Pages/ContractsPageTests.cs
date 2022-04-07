using Domain.Contracts;

namespace Client.Tests.Pages;

public class ContractsPageTests : IDisposable
{
    private readonly TestContext _context;
    private readonly MockHttpMessageHandler _mockHttp;

    public ContractsPageTests()
    {
        _context = new TestContext();
        _mockHttp = _context.Services.AddMockHttpClient();
    }

    public void Dispose()
    {
        _context.Dispose();
        _mockHttp.Dispose();
    }

    [Fact]
    public void ContractPage_ShouldSayLoading_WhenThereAreNoContractsFetched()
    {
        // Arrange
        _mockHttp.When("/").RespondJson(new Contract[1]);

        // Act
        IRenderedComponent<ContractsPage> cut = _context.RenderComponent<ContractsPage>();

        // Assert
        cut.Find("p em").TextContent.Should().BeEquivalentTo("Loading...");
    }

    [Fact]
    public void ContractPage_ShouldContainContract_WhenItHasFetchedAContract()
    {
        // Arrange
        const string name = "SJ";
        var contract = new Contract() { Name = name, ImagePath = "/img/test" };
        _mockHttp.When("/api/v1/Contracts/All").RespondJson(new[] { contract });

        // Act
        IRenderedComponent<ContractsPage> cut = _context.RenderComponent<ContractsPage>();
        cut.WaitForElement(".card");

        // Assert
        cut.Find(".card").TextContent.Should().Contain(name);
    }
}

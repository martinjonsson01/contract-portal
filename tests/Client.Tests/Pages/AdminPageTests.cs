using Domain.Contracts;

namespace Client.Tests.Pages;

public class AdminPageTests : IDisposable
{
    private readonly TestContext _context;
    private readonly MockHttpMessageHandler _mockHttp;

    public AdminPageTests()
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
    public void AdminPage_ShouldSayLoading_WhenThereAreNoContractsFetched()
    {
        // Arrange
        _mockHttp.When("/").RespondJson(new Contract[1]);

        // Act
        IRenderedComponent<ContractsPage> cut = _context.RenderComponent<ContractsPage>();

        // Assert
        cut.Find("p em").TextContent.Should().BeEquivalentTo("Loading...");
    }

    [Fact]
    public void AdminPage_ShouldContainCorrectAmountOfContract()
    {
        // Arrange
        IEnumerable<Contract> fakeContracts = new Faker<Contract>().Generate(10);
        _mockHttp.When("/api/v1/Contracts/All").RespondJson(fakeContracts);

        // Act
        IRenderedComponent<Admin> cut = _context.RenderComponent<Admin>();
        cut.WaitForElement(".list-group");

        // Assert
        cut.FindAll(".list-group-item").Count.Should().Be(10);
    }
}

namespace Client.Tests.Pages;

public class ContractCardTests : IDisposable
{
    private readonly TestContext _context;

    public ContractCardTests()
    {
        _context = new TestContext();
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    [Fact]
    public void ContractCard_ContainsGivenInformation()
    {
        // Arrange
        IRenderedComponent<ContractCard> cut =
            _context.RenderComponent<ContractCard>(parameters =>
                parameters.Add(property => property.Name, "Sj")
                    .Add(property => property.ImagePath, "/img/test"));

        // Act

        // Assert
        cut.Find("h3").TextContent.Should().BeEquivalentTo("SJ");
        cut.Find("img").OuterHtml.Should().Contain("/img/test");
    }
}

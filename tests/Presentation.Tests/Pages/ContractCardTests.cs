namespace Presentation.Tests.Pages;

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
        const string name = "SJ";
        const string path = "test";

        static void ParameterBuilder(ComponentParameterCollectionBuilder<ContractCard> parameters) =>
            parameters.Add(property => property.Name, name)
                      .Add(property => property.ImagePath, path);

        // Act
        IRenderedComponent<ContractCard> cut =
            _context.RenderComponent<ContractCard>(ParameterBuilder);

        // Assert
        cut.Find("#contract-name").TextContent.Should().Contain(name);
        cut.Find("#contract-thumbnail").OuterHtml.Should().Contain(path);
    }
}

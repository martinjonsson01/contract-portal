namespace Client.Tests.Pages;

public class ContractListItemTests : IDisposable
{
    private readonly TestContext _context;

    public ContractListItemTests()
    {
        _context = new TestContext();
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    [Fact]
    public void ContractListItem_ContainsTitle()
    {
        // Arrange
        const string name = "SJ";

        static void ParameterBuilder(ComponentParameterCollectionBuilder<ContractListItem> parameters) =>
            parameters.Add(property => property.Name, name);

        // Act
        IRenderedComponent<ContractListItem> cut =
            _context.RenderComponent<ContractListItem>(ParameterBuilder);

        // Assert
        cut.Find($"#{name}").TextContent.Should().Contain(name);
    }
}

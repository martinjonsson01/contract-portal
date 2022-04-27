using Client.Pages.Admin;
using Domain.Contracts;

namespace Presentation.Tests.Client.Pages.Admin;

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
        var contract = new Contract() { Name = name };

        void ParameterBuilder(ComponentParameterCollectionBuilder<ContractTableRow> parameters) =>
            parameters.Add(property => property.Contract, contract);

        // Act
        IRenderedComponent<ContractTableRow> cut =
            _context.RenderComponent<ContractTableRow>(ParameterBuilder);

        // Assert
        cut.Find($"#contract_id_{contract.Id}").TextContent.Should().Contain(name);
    }
}

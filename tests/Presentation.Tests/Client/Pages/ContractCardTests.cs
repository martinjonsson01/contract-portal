using Client.Pages.Contract;

using Domain.Contracts;

namespace Presentation.Tests.Client.Pages;

public class ContractCardTests : UITestFixture
{
    [Fact]
    public void ContractCard_ContainsGivenInformation()
    {
        // Arrange
        const string name = "SJ";
        const string path = "test";
        var contract = new Contract { Name = name, SupplierLogoImagePath = path, };

        void ParameterBuilder(ComponentParameterCollectionBuilder<ContractCard> parameters) =>
            parameters.Add(property => property.Contract, contract);

        // Act
        IRenderedComponent<ContractCard> cut =
            Context.RenderComponent<ContractCard>(ParameterBuilder);

        // Assert
        cut.Find("#contract-name").TextContent.Should().Contain(name);
        cut.Find("#contract-thumbnail").OuterHtml.Should().Contain(path);
    }
}

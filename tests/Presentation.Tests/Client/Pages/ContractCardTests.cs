using Domain.Contracts;

namespace Presentation.Tests.Client.Pages;

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
        var contract = new Contract() { Name = name, ImagePath = path };

        void ParameterBuilder(ComponentParameterCollectionBuilder<ContractCard> parameters) =>
            parameters.Add(property => property.Contract, contract);

        // Act
        IRenderedComponent<ContractCard> cut =
            _context.RenderComponent<ContractCard>(ParameterBuilder);

        // Assert
        cut.Find("#contract-name").TextContent.Should().Contain(name);
        cut.Find("#contract-thumbnail").OuterHtml.Should().Contain(path);
    }

    [Fact]
    public void ContractCard_ShowsFavoriteIcon_WhenContractIsFavoriteMarked()
    {
        // Arrange
        const bool favorite = true;
        var contract = new Contract() { IsFavorite = favorite, };

        void ParameterBuilder(ComponentParameterCollectionBuilder<ContractCard> parameters) =>
            parameters.Add(property => property.Contract, contract);

        // Act
        IRenderedComponent<ContractCard> cut =
            _context.RenderComponent<ContractCard>(ParameterBuilder);

        // Assert
        cut.Find(".bi-heart-fill").InnerHtml.Should().Contain("M8 1.314C12.438-3.248 23.534 4.735 8 15-7.534 4.736 3.562-3.248 8 1.314z");
    }

    [Fact]
    public void ContractCard_ShowsNonFavoriteIcon_WhenContractIsNotFavoriteMarked()
    {
        // Arrange
        const bool favorite = false;
        var contract = new Contract() { IsFavorite = favorite, };

        void ParameterBuilder(ComponentParameterCollectionBuilder<ContractCard> parameters) =>
            parameters.Add(property => property.Contract, contract);

        // Act
        IRenderedComponent<ContractCard> cut =
            _context.RenderComponent<ContractCard>(ParameterBuilder);

        // Assert
        cut.Find(".bi-heart").InnerHtml.Should().Contain("m8 2.748-.717-.737C5.6.281 2.514.878 1.4");
    }
}

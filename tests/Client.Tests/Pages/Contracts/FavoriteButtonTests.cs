using Client.Pages.Contracts;

using Domain.Contracts;

namespace Client.Tests.Pages.Contracts;

public class FavoriteButtonTests : UITestFixture
{
    [Fact]
    public void ContractCard_ShowsFavoriteIcon_WhenContractIsFavoriteMarked()
    {
        // Arrange
        const bool favorite = true;
        var contract = new Contract() { IsFavorite = favorite, };

        void ParameterBuilder(ComponentParameterCollectionBuilder<FavoriteButton> parameters) =>
            parameters.Add(property => property.Contract, contract);

        // Act
        IRenderedComponent<FavoriteButton> cut =
            Context.RenderComponent<FavoriteButton>(ParameterBuilder);

        // Assert
        cut.Find(".bi-heart-fill").Should().NotBeNull();
        Assert.Throws<ElementNotFoundException>(() => cut.Find(".bi-heart"));
    }

    [Fact]
    public void ContractCard_ShowsNonFavoriteIcon_WhenContractIsNotFavoriteMarked()
    {
        // Arrange
        const bool favorite = false;
        var contract = new Contract() { IsFavorite = favorite, };

        void ParameterBuilder(ComponentParameterCollectionBuilder<FavoriteButton> parameters) =>
            parameters.Add(property => property.Contract, contract);

        // Act
        IRenderedComponent<FavoriteButton> cut =
            Context.RenderComponent<FavoriteButton>(ParameterBuilder);

        // Assert
        cut.Find(".bi-heart").Should().NotBeNull();
        Assert.Throws<ElementNotFoundException>(() => cut.Find(".bi-heart-fill"));
    }
}

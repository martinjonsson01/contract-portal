namespace Presentation.Tests.Client.Pages;

public class ContractCardTests : UITestFixture
{
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
            Context.RenderComponent<ContractCard>(ParameterBuilder);

        // Assert
        cut.Find("#contract-name").TextContent.Should().Contain(name);
        cut.Find("#contract-thumbnail").OuterHtml.Should().Contain(path);
    }

    [Fact]
    public void ContractCard_ModalShown_WhenClicked()
    {
        // Arrange
        IRenderedComponent<ContractCard> cut = Context.RenderComponent<ContractCard>();

        // Act
        cut.Find(".card").Click();

        // Assert
        cut.Find(".modal.show").Should().NotBeNull();
    }

    [Fact]
    public void ContractCard_ModalNotShown_WhenNotClicked()
    {
        // Arrange
        IRenderedComponent<ContractCard> cut = Context.RenderComponent<ContractCard>();

        // Act
        Action act = () => cut.Find(".modal.show");

        // Assert
        act.Should().Throw<ElementNotFoundException>();
    }
}

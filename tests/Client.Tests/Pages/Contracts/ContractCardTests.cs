﻿using Client.Pages.Contracts;

using Domain.Contracts;

namespace Client.Tests.Pages.Contracts;

public class ContractCardTests : UITestFixture
{
    public ContractCardTests(ITestOutputHelper outputHelper)
        : base(outputHelper)
    {
    }

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
        cut.WaitForAssertion(() => cut.Find("#contract-name").TextContent.Should().Contain(name));
        cut.WaitForAssertion(() => cut.Find("#contract-thumbnail").OuterHtml.Should().Contain(path));
    }
}

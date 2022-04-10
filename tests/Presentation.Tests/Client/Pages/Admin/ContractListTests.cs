using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using AngleSharp.Dom;

using Client.Pages.Admin;

using Domain.Contracts;
using Microsoft.AspNetCore.Components.Web;

namespace Presentation.Tests.Client.Pages.Admin;

public class ContractListTests : UITestFixture
{
    [Fact]
    public void AddingContract_RendersTheNewContract()
    {
        // Arrange
        Contract[] contracts = { new Contract() { Name = "First", }, new Contract() { Name = "Second", }, };
        MockHttp.When("/api/v1/Contracts/All").RespondJson(contracts);

        IRenderedComponent<ContractList> cut = Context.RenderComponent<ContractList>();
        const string itemSelector = ".list-group-item";
        cut.WaitForElement(itemSelector);

        const string newContractName = "New Contract";
        var newContract = new Contract { Name = newContractName, };

        // Act
        cut.Instance.Add(newContract);
        cut.WaitForState(() => cut.FindAll(itemSelector).Count == 3);

        // Assert
        Expression<Func<IElement, bool>>
            elementWithNewName = contract => contract.TextContent.Contains(newContractName);
        cut.FindAll(itemSelector).Should().Contain(elementWithNewName);
    }

    [Fact]
    public void AddingContract_DoesNotThrow_BeforeContractsAreFetched()
    {
        // Arrange
        MockHttp.When("/api/v1/Contracts/All").Respond(async () =>
        {
            // Simulate slow network.
            await Task.Delay(TimeSpan.FromSeconds(10)).ConfigureAwait(false);
            return new HttpResponseMessage(HttpStatusCode.OK);
        });
        IRenderedComponent<ContractList> cut = Context.RenderComponent<ContractList>();

        const string newContractName = "New Contract";
        var newContract = new Contract { Name = newContractName, };

        // Act
        Action add = () => cut.Instance.Add(newContract);

        // Assert
        add.Should().NotThrow();
    }

    [Fact]
    public async Task RemovingContract_RendersWithoutTheContractAsync()
    {
        // Arrange
        var firstContract = new Contract() { Name = "first", };
        Contract[] contracts = { firstContract, new Contract() { Name = "Second", }, };
        MockHttp.When("/api/v1/Contracts/All").RespondJson(contracts);
        MockHttp.When(HttpMethod.Delete, $"/api/v1/Contracts/{firstContract.Id}").Respond(req => new HttpResponseMessage(HttpStatusCode.OK));

        IRenderedComponent<ContractList> cut = Context.RenderComponent<ContractList>();
        const string removeButton = ".btn.btn-danger";
        cut.WaitForElement(removeButton);

        // Act
        await cut.Find(removeButton).ClickAsync(new MouseEventArgs()).ConfigureAwait(false);
        cut.WaitForState(() => cut.FindAll(removeButton).Count == 1);

        // Assert
        Expression<Func<IElement, bool>>
            elementWithNewName = contract => contract.TextContent.Contains(firstContract.Name);
        cut.FindAll(".list-group-item").Should().NotContain(elementWithNewName);
    }
}

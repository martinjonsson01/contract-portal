using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

using Domain.Contracts;

namespace Server.IntegrationTests.Contracts;

public class ContractApiIntegrationTests : IntegrationTest
{
    public ContractApiIntegrationTests(TestWebApplicationFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task PutContract_CreatesNewContract_WhenItDoesNotExist()
    {
        // Arrange
        await ArrangeAuthenticatedAdminAsync();
        var newContract = new Contract();

        // Act
        HttpResponseMessage response = await Client.PutAsJsonAsync("api/v1/contracts", newContract);

        // Assert
        response.Should().BeSuccessful();
        var contracts = await Client.GetFromJsonAsync<IEnumerable<Contract>>("api/v1/contracts");
        contracts.Should().ContainEquivalentOf(newContract);
    }

    [Fact]
    public async Task PutContract_UpdatesExistingContract_WhenItExists()
    {
        // Arrange
        var contract = new Contract();
        const string endpointUrl = $"api/v1/contracts";
        await PutResourceAsync(endpointUrl, contract);
        await ArrangeAuthenticatedAdminAsync();
        contract.Name = "Modified name";

        // Act
        HttpResponseMessage response = await Client.PutAsJsonAsync(endpointUrl, contract);

        // Assert
        response.Should().BeSuccessful();
        var contracts = await Client.GetFromJsonAsync<IEnumerable<Contract>>("api/v1/contracts");
        contracts.Should().ContainEquivalentOf(contract);
    }
}

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
    public async Task PutContract_CreatesNewContract_IfItDoesNotExist()
    {
        // Arrange
        await ArrangeAuthenticatedAdminAsync();
        var newContract = new Contract();

        // Act
        HttpResponseMessage response = await Client.PutAsJsonAsync($"api/v1/contracts/{newContract.Id}", newContract);

        // Assert
        response.Should().BeSuccessful();
        var contracts = await Client.GetFromJsonAsync<IEnumerable<Contract>>("api/v1/contracts");
        contracts.Should().ContainEquivalentOf(newContract);
    }
}

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
}

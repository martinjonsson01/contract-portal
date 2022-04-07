using Application.Contracts;
using Domain.Contracts;

namespace Application.Tests.Contracts;

public class ContractServiceTests
{
    [Fact]
    public void FetchAllContracts_ReturnsAllContractsInTheDatabase()
    {
        // Arrange
        const int numberOfContracts = 10;
        var mockRepo = new Mock<IContractRepository>();
        List<Contract> mockContracts = new Faker<Contract>().Generate(numberOfContracts);
        mockRepo.Setup(repository => repository.Contracts).Returns(mockContracts);
        var cut = new ContractService(mockRepo.Object);

        // Act
        IEnumerable<Contract> contracts = cut.FetchAllContracts();

        // Assert
        contracts.Should().HaveCount(numberOfContracts);
    }
}

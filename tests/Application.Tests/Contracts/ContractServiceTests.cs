using System;

using Application.Contracts;
using Application.Exceptions;

using Domain.Contracts;

namespace Application.Tests.Contracts;

public class ContractServiceTests
{
    private readonly ContractService _cut;
    private readonly Mock<IContractRepository> _mockRepo;

    public ContractServiceTests()
    {
        _mockRepo = new Mock<IContractRepository>();
        _cut = new ContractService(_mockRepo.Object);
    }

    [Fact]
    public void FetchAllContracts_ReturnsAllContractsInTheDatabase()
    {
        // Arrange
        const int numberOfContracts = 10;
        List<Contract> mockContracts = new Faker<Contract>().Generate(numberOfContracts);
        _mockRepo.Setup(repository => repository.All).Returns(mockContracts);

        // Act
        IEnumerable<Contract> contracts = _cut.FetchAllContracts();

        // Assert
        contracts.Should().HaveCount(numberOfContracts);
    }

    [Fact]
    public void AddingContract_ThrowsIDException_IfIDAlreadyTaken()
    {
        // Arrange
        var contract = new Contract();
        _mockRepo.Setup(repository => repository.All).Returns(new[] { contract, });

        // Act
        Action add = () => _cut.Add(contract);

        // Assert
        add.Should().Throw<IdentifierAlreadyTakenException>();
    }

    [Fact]
    public void AddingContract_DoesNotThrow_IfIDIsUnique()
    {
        // Arrange
        _mockRepo.Setup(repository => repository.All).Returns(new List<Contract>());

        // Act
        Action add = () => _cut.Add(new Contract());

        // Assert
        add.Should().NotThrow();
    }

    [Fact]
    public void RemovingContractFromRepo_DoesReturnTrue_WhenContractExists()
    {
        // Arrange
        var contract = new Contract();
        _mockRepo.Setup(repository => repository.Remove(contract.Id)).Returns(true);

        // Act
        bool actual = _cut.Remove(contract.Id);

        // // Assert
        actual.Should().BeTrue();
    }

    [Fact]
    public void RemovingContractFrom_DoesReturnFalse_WhenRepoIsEmpty()
    {
        // Arrange
        _mockRepo.Setup(repository => repository.All).Returns(new List<Contract>());

        var id = Guid.NewGuid();

        // Act
        bool actual = _cut.Remove(id);

        // Assert
        actual.Should().BeFalse();
    }
}

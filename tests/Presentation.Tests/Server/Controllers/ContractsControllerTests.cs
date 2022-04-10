﻿using Application.Contracts;

using Domain.Contracts;

namespace Presentation.Tests.Server.Controllers;

public class ContractsControllerTests
{
    private readonly ContractsController _cut;
    private readonly Mock<IContractService> _mockContracts;

    public ContractsControllerTests()
    {
        _mockContracts = new Mock<IContractService>();
        _cut = new ContractsController(Mock.Of<ILogger<ContractsController>>(), _mockContracts.Object);
    }

    [Fact]
    public void Get_AllContracts()
    {
        // Arrange
        List<Contract> fakeContracts = new Faker<Contract>().Generate(10);
        _mockContracts.Setup(service => service.FetchAllContracts()).Returns(fakeContracts);

        // Act
        IEnumerable<Contract> actualWeather = _cut.AllContracts();

        // Assert
        actualWeather.Should().BeEquivalentTo(fakeContracts);
    }
}

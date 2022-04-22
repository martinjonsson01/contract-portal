using Application.Exceptions;
using Application.Users;
using Domain.Users;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Tests.Server.Controllers;

public class UsersControllerTests
{
    private readonly UsersController _cut;
    private readonly Mock<IUserService> _mockUsers;

    public UsersControllerTests()
    {
        _mockUsers = new Mock<IUserService>();
        _cut = new UsersController(Mock.Of<ILogger<UsersController>>(), _mockUsers.Object);
    }

    [Fact]
    public void Create_ReturnsBadRequest_IfIDAlreadyTaken()
    {
        // Arrange
        var user = new User();
        _mockUsers.Setup(service => service.Add(user)).Throws<IdentifierAlreadyTakenException>();

        // Act
        IActionResult actual = _cut.Create(user);

        // Assert
        actual.Should().BeOfType<BadRequestResult>();
    }

    [Fact]
    public void Create_ReturnsOk_IfIDIsUnique()
    {
        // Arrange

        // Act
        IActionResult actual = _cut.Create(new User());

        // Assert
        actual.Should().BeOfType<OkResult>();
    }

    [Fact]
    public void Get_ReturnsAll_WhenQueryIsNull()
    {
        // Arrange
        List<User> fakeContracts = new Faker<User>().Generate(10);
        _mockUsers.Setup(service => service.Search(string.Empty)).Returns(fakeContracts);

        // Act
        IEnumerable<User> contracts = _cut.Search(null);

        // Assert
        contracts.Should().BeEquivalentTo(fakeContracts);
    }

    [Fact]
    public void Get_ReturnsSearchResults_WhenQueryIsSet()
    {
        // Arrange
        List<User> searchResults = new Faker<User>().Generate(10);
        const string searchQuery = "keyword1 keyword2";
        _mockUsers.Setup(service => service.Search(searchQuery)).Returns(searchResults);

        // Act
        IEnumerable<User> actual = _cut.Search(searchQuery);

        // Assert
        actual.Should().BeEquivalentTo(searchResults);
    }
}

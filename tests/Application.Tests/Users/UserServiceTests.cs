using System;
using Application.Exceptions;
using Application.Search;
using Application.Users;
using Domain.Users;

namespace Application.Tests.Users;

public class UserServiceTests
{
    private readonly UserService _cut;
    private readonly Mock<IUserRepository> _mockRepo;

    public UserServiceTests()
    {
        _mockRepo = new Mock<IUserRepository>();
        _cut = new UserService(_mockRepo.Object, new SearchEngine<User>());
    }

    [Fact]
    public void AddingUser_ThrowsIDException_IfIDAlreadyTaken()
    {
        // Arrange
        var user = new User();
        _mockRepo.Setup(repository => repository.All).Returns(new[] { user });

        // Act
        Action add = () => _cut.Add(user);

        // Assert
        add.Should().Throw<IdentifierAlreadyTakenException>();
    }

    [Fact]
    public void AddingUser_DoesNotThrow_IfIDIsUnique()
    {
        // Arrange
        _mockRepo.Setup(repository => repository.All).Returns(new List<User>());

        // Act
        Action add = () => _cut.Add(new User());

        // Assert
        add.Should().NotThrow();
    }

    [Fact]
    public void FetchAllUsers_ReturnsAllUsersInTheDatabase()
    {
        // Arrange
        const int numberOfUsers = 10;
        List<User> mockUsers = new Faker<User>().Generate(numberOfUsers);
        _mockRepo.Setup(repository => repository.All).Returns(mockUsers);

        // Act
        IEnumerable<User> users = _cut.FetchAllUsers();

        // Assert
        users.Should().HaveCount(numberOfUsers);
    }

    [Fact]
    public void Searching_ReturnsAllUsers_WhenQueryIsEmpty()
    {
        // Arrange
        List<User> expected = new Faker<User>().Generate(10);
        _mockRepo.Setup(repository => repository.All).Returns(expected);

        // Act
        IEnumerable<User> actual = _cut.Search(string.Empty);

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }
}

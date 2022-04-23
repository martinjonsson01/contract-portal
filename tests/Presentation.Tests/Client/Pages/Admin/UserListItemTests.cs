using Client.Pages.Admin;
using Domain.Users;

namespace Presentation.Tests.Client.Pages.Admin;

public class UserListItemTests : IDisposable
{
    private readonly TestContext _context;

    public UserListItemTests()
    {
        _context = new TestContext();
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    [Fact]
    public void UserListItem_ContainsName()
    {
        // Arrange
        const string name = "Foo";
        var user = new User() { Name = name };

        void ParameterBuilder(ComponentParameterCollectionBuilder<UserListItem> parameters) =>
            parameters.Add(property => property.User, user);

        // Act
        IRenderedComponent<UserListItem> cut =
            _context.RenderComponent<UserListItem>(ParameterBuilder);

        // Assert
        cut.Find($"#user_id_{user.Id}").TextContent.Should().Contain(name);
    }
}

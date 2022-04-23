using System.Linq.Expressions;
using AngleSharp.Dom;
using Client.Pages.Admin;
using Domain.Users;

namespace Presentation.Tests.Client.Pages.Admin;

public class UserListTests : UITestFixture
{
    [Fact]
    public void AddingUser_RendersTheNewUser()
    {
        // Arrange
        User[] users = { new User() { Name = "First", }, new User() { Name = "Second", } };
        MockHttp.When("/api/v1/users").RespondJson(users);

        IRenderedComponent<UserList> cut = Context.RenderComponent<UserList>();
        const string itemSelector = ".list-group-item";
        cut.WaitForElement(itemSelector);

        const string newUserName = "New User";
        var newUser = new User { Name = newUserName };

        // Act
        cut.Instance.Add(newUser);
        cut.WaitForState(() => cut.FindAll(itemSelector).Count == 3);

        // Assert
        Expression<Func<IElement, bool>>
            elementWithNewName = contract => contract.TextContent.Contains(newUserName);
        cut.FindAll(itemSelector).Should().Contain(elementWithNewName);
    }
}

using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using AngleSharp.Dom;
using Client.Pages.Admin;
using Domain.Users;

using Microsoft.AspNetCore.Components.Web;

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

    [Fact]
    public async Task RemovingUsers_RendersWithoutTheUsersAsync()
    {
        // Arrange
        var firstUser = new User() { Name = "first", };
        User[] users = { firstUser, new User() { Name = "second", }, };
        MockHttp.When("/api/v1/users/All").RespondJson(users);
        MockHttp.When(HttpMethod.Delete, $"/api/v1/Users/{firstUser.Id}").Respond(req => new HttpResponseMessage(HttpStatusCode.OK));

        IRenderedComponent<UserList> cut = Context.RenderComponent<UserList>();
        const string removeButton = ".btn.btn-danger";
        cut.WaitForElement(removeButton);

        // Act
        await cut.Find(removeButton).ClickAsync(new MouseEventArgs()).ConfigureAwait(false);
        cut.WaitForState(() => cut.FindAll(removeButton).Count == 1);

        // Assert
        Expression<Func<IElement, bool>>
            elementWithNewName = user => user.TextContent.Contains(firstUser.Name);
        cut.FindAll(".list-group-item").Should().NotContain(elementWithNewName);
    }
}

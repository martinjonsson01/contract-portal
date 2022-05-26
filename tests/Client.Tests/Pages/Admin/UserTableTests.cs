using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using AngleSharp.Dom;

using Client.Pages.Admin;
using Client.Pages.Admin.UserComponents;

using Domain.Users;

using Microsoft.AspNetCore.Components.Web;

namespace Client.Tests.Pages.Admin;

public class UserTableTests : UITestFixture
{
    public UserTableTests(ITestOutputHelper outputHelper)
        : base(outputHelper)
    {
    }

    [Fact]
    public void AddingUser_RendersTheNewUser()
    {
        // Arrange
        var user = new User() { Name = "First", };
        User[] users = { user, new User() { Name = "Second", } };
        MockHttp.When("/api/v1/users").RespondJson(users);

        IRenderedComponent<UserTable> cut = Context.RenderComponent<UserTable>();

        string itemSelector = ".user-table-row";

        const string newUserName = "New User";
        cut.WaitForElement(itemSelector);
        var newUser = new User { Name = newUserName };

        // Act
        cut.Instance.AddOrUpdate(newUser);
        cut.WaitForState(() => cut.FindAll(itemSelector).Count == 3);

        // Assert
        Expression<Func<IElement, bool>>
            elementWithNewName = user => user.TextContent.Contains(newUserName);
        cut.FindAll(itemSelector).Should().Contain(elementWithNewName);
    }

    [Fact]
    public async Task RemovingUser_RendersWithoutTheUserAsync()
    {
        // Arrange
        var firstUser = new User() { Name = "first", };
        User[] users = { firstUser, new User() { Name = "Second", }, };
        MockHttp.When("/api/v1/users").RespondJson(users);
        MockHttp.When(HttpMethod.Delete, $"/api/v1/users/{firstUser.Id}").Respond(req => new HttpResponseMessage(HttpStatusCode.OK));

        IRenderedComponent<UserTable> cut = Context.RenderComponent<UserTable>();
        const string removeButton = "#confirm-remove";
        cut.WaitForElement(removeButton);

        // Act
        await cut.Find(removeButton).ClickAsync(new MouseEventArgs()).ConfigureAwait(false);
        cut.WaitForState(() => cut.FindAll(removeButton).Count == 1);

        // Assert
        Expression<Func<IElement, bool>>
            elementWithNewName = user => user.TextContent.Contains(firstUser.Name);
        cut.FindAll(".user-table-row").Should().NotContain(elementWithNewName);
    }
}

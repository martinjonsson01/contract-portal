using Client.Pages.Admin;

using Domain.Users;

namespace Client.Tests.Pages.Admin;

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

        void ParameterBuilder(ComponentParameterCollectionBuilder<UserTableRow> parameters) =>
            parameters.Add(property => property.User, user);

        // Act
        IRenderedComponent<UserTableRow> cut =
            _context.RenderComponent<UserTableRow>(ParameterBuilder);

        // Assert
        cut.WaitForAssertion(() => cut.Find($"#user_id_{user.Id}").TextContent.Should().Contain(name));
    }

    [Fact]
    public void UserListItem_ContainsLatestPaymentDate()
    {
        // Arrange
        DateTime latestPaymentDate = DateTime.Now;
        var user = new User() { LatestPaymentDate = latestPaymentDate };

        void ParameterBuilder(ComponentParameterCollectionBuilder<UserTableRow> parameters) =>
            parameters.Add(property => property.User, user);

        // Act
        IRenderedComponent<UserTableRow> cut =
            _context.RenderComponent<UserTableRow>(ParameterBuilder);

        // Assert
        cut.WaitForAssertion(() =>
            cut.Find($"#user_id_{user.Id}").TextContent.Should().Contain(latestPaymentDate.ToShortDateString()));
    }
}

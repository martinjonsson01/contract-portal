using Client.Pages.Admin.NotificationComponents;

using Domain.StatusUpdates;

namespace Client.Tests.Pages.Admin;

public class NotificationListItemTests : IDisposable
{
    private readonly TestContext _context;

    public NotificationListItemTests()
    {
        _context = new TestContext();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}

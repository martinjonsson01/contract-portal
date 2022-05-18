using Application.StatusUpdates;

namespace Application.Tests.StatusUpdates;

public class NotificationServiceTests
{
    private readonly NotificationService _cut;
    private readonly Mock<IStatusUpdateRepository> _mockRepo;

    public NotificationServiceTests()
    {
        _mockRepo = new Mock<IStatusUpdateRepository>();
        _cut = new NotificationService(_mockRepo.Object);
    }
}

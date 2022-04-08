using Server.Pages;

namespace Presentation.Tests;

public class DummyTests
{
    [Fact]
    public void GeneratedFiles_ShouldBeCovered()
    {
        var model = new ErrorModel();

        model.ShowRequestId.Should().BeFalse();
        model.RequestId = "1";
        model.ShowRequestId.Should().BeTrue();
    }
}

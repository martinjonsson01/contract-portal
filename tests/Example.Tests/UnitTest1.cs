namespace Example.Tests;

using Xunit;

public class UnitTest1
{
    [Fact]
    public void Test_WillAlways_Fail()
    {
        Assert.True(false);
    }

    [Fact]
    public void Test_WillAlways_Succeed()
    {
        Assert.True(true);
    }
}
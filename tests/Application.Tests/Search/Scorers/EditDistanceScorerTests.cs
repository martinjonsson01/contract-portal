using Application.Search.Scorers;

using Domain.Contracts;

namespace Application.Tests.Search.Scorers;

public class EditDistanceScorerTests
{
    private const double Precision = 0.0000001d;

    [Fact]
    public void Score_IsOne_WhenQueryIsSameAsProperty()
    {
        // Arrange
        var cut = new EditDistanceScorer(contract => contract.Name);

        const string name = "Name of contract";
        var contract = new Contract { Name = name, };

        // Act
        double score = cut.Score(contract, name);

        // Assert
        score.Should().BeApproximately(1.0d, Precision);
    }
}

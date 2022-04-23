using Application.Search.Scorers;

using Domain.Contracts;

namespace Application.Tests.Search.Scorers;

public class EditDistanceScorerTests
{
    private const double Precision = 1e-7;

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

    [Fact]
    public void Score_IsZero_WhenQueryIsEmpty()
    {
        // Arrange
        var cut = new EditDistanceScorer(contract => contract.Name);

        var contract = new Contract { Name = "Name of contract", };

        // Act
        double score = cut.Score(contract, string.Empty);

        // Assert
        score.Should().BeApproximately(0.0d, Precision);
    }

    [Fact]
    public void Score_IsHalf_WhenQueryIsHalfOfProperty()
    {
        // Arrange
        var cut = new EditDistanceScorer(contract => contract.Name);

        const string name = "Name of contract";
        var contract = new Contract { Name = name, };

        // Act
        double score = cut.Score(contract, name.Substring(name.Length / 2));

        // Assert
        score.Should().BeApproximately(0.5d, Precision);
    }
}

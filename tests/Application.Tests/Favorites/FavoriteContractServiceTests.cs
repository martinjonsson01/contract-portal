using System;

using Application.Contracts;
using Application.Exceptions;
using Application.FavoriteContracts;
using Application.Search;

using Domain.Contracts;

namespace Application.Tests.Contracts;

public class FavoriteContractServiceTests
{
    private readonly FavoriteContractService _cut;
    private readonly Mock<IFavoriteContractRepository> _mockRepo;

    public FavoriteContractServiceTests()
    {
        _mockRepo = new Mock<IFavoriteContractRepository>();
        _cut = new FavoriteContractService(_mockRepo.Object);
    }

    [Fact]
    public void FetchFavorite_CallsFavoriteFromRepoExactlyOnce()
    {
        // Act
        _cut.FetchFavorites();

        // Assert
        _mockRepo.Verify(repo => repo.Favorites, Times.Exactly(1));
    }
}

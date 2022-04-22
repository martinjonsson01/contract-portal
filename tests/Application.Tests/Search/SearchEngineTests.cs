﻿using System;
using System.Globalization;

using Application.Search;

namespace Application.Tests.Search;

public class SearchEngineTests
{
    private readonly SearchEngine<int> _cut;

    public SearchEngineTests()
    {
        _cut = new SearchEngine<int>();
    }

    [Fact]
    public void Search_ReturnsAllResults_WhenQueryIsEmpty()
    {
        // Arrange
        int[] entities = { 1, 2, 3, 4, };

        // Act
        IEnumerable<int> results = _cut.Search(string.Empty, entities);

        // Assert
        results.Should().BeEquivalentTo(entities);
    }

    [Fact]
    public void Search_ReturnsAllEntities_WhenThereAreNoModules()
    {
        // Arrange
        int[] entities = { 1, 2, 3, 4, };

        // Act
        IEnumerable<int> results = _cut.Search("59", entities);

        // Assert
        results.Should().BeEquivalentTo(entities);
    }

    [Fact]
    public void SearchWithModule_ReturnsResultsDependingOnTheModule()
    {
        // Arrange
        ISearchModule<int> module = new TextMatcher();
        _cut.AddModule(module);
        int[] numbers = { 1, 2, 3, };

        // Act
        IEnumerable<int> actual = _cut.Search("1", numbers);

        // Assert
        actual.Should().BeEquivalentTo(new[] { 1, });
    }

    [Fact]
    public void SearchWithModule_ReturnsMultipleMatches_WhenThereAreMultipleElementsThatMatch()
    {
        // Arrange
        ISearchModule<int> module = new TextMatcher();
        _cut.AddModule(module);
        int[] numbers = { 1, 1, 1, 2, 3, };

        // Act
        IEnumerable<int> actual = _cut.Search("1", numbers);

        // Assert
        actual.Should().BeEquivalentTo(new[] { 1, 1, 1, });
    }

    [Fact]
    public void SearchWithModule_ReturnsNothing_WhenQueryDoesNotMatchAnything()
    {
        // Arrange
        ISearchModule<int> module = new TextMatcher();
        _cut.AddModule(module);
        int[] numbers = { 2, 3, };

        // Act
        IEnumerable<int> actual = _cut.Search("1", numbers);

        // Assert
        actual.Should().BeEquivalentTo(new List<int>());
    }

    [Fact]
    public void SearchWithModules_ReturnsMultipleEntities_WhenMultipleEntitiesMatchWithDifferentModules()
    {
        // Arrange
        _cut.AddModule(new DivisibleMatcher());
        _cut.AddModule(new TextMatcher());
        int[] numbers = { 1, 2, 3, 4, };

        // Act
        IEnumerable<int> actual = _cut.Search("2", numbers);

        // Assert
        actual.Should().BeEquivalentTo(new[] { 2, 4, }); // 2 == "2", 4 is divisible by 2
    }

    private class TextMatcher : ISearchModule<int>
    {
        public virtual bool Match(int entity, string query)
        {
            return entity.ToString(CultureInfo.InvariantCulture).Equals(query, StringComparison.Ordinal);
        }
    }

    private class DivisibleMatcher : ISearchModule<int>
    {
        public virtual bool Match(int entity, string query)
        {
            if (int.TryParse(query, out int divisor))
            {
                return entity % divisor == 0;
            }

            return false;
        }
    }
}
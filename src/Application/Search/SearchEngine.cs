using System.Diagnostics.CodeAnalysis;

using Application.Search.Modules;

namespace Application.Search;

/// <summary>
/// A general search engine that can be used for querying a collection of entities.
/// </summary>
/// <typeparam name="TEntity">The type of the entities to perform queries on.</typeparam>
public class SearchEngine<TEntity>
{
    private readonly ICollection<ISearchModule<TEntity>> _modules = new List<ISearchModule<TEntity>>
    {
        new EmptySearch<TEntity>(), // By default the only module is the one that matches on empty queries.
    };

    /// <summary>
    /// Performs a search on the entities, returning the ones that match the query.
    /// </summary>
    /// <param name="query">The input search criteria.</param>
    /// <param name="entities">The entities to search through.</param>
    /// <returns>The entities that match the query.</returns>
    [SuppressMessage("Style", "IDE0046:Convert to conditional expression", Justification = "Readability.")]
    public IEnumerable<TEntity> Search(string query, IEnumerable<TEntity> entities)
    {
        return entities.Where(entity => _modules.Any(module => module.Match(entity, query)))
                       .ToList();
    }

    /// <summary>
    /// Registers a new <see cref="ISearchModule{TEntity}"/> to be used when checking for query matches.
    /// </summary>
    /// <param name="module">The module to add.</param>
    public void AddModule(ISearchModule<TEntity> module)
    {
        _modules.Add(module);
    }
}

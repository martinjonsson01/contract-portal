namespace Application.Search;

/// <summary>
/// A general search engine that can be used for querying a collection of entities.
/// </summary>
/// <typeparam name="TEntity">The type of the entities to perform queries on.</typeparam>
public class SearchEngine<TEntity>
    where TEntity : notnull
{
    private readonly ICollection<ISearchModule<TEntity>> _modules = new List<ISearchModule<TEntity>>();

    /// <summary>
    /// Performs a search on the entities, returning the ones that match the query.
    /// </summary>
    /// <param name="query">The input search criteria.</param>
    /// <param name="entities">The entities to search through.</param>
    /// <returns>The entities that match the query.</returns>
    public IEnumerable<TEntity> Search(string query, IEnumerable<TEntity> entities)
    {
        if (string.IsNullOrEmpty(query) || !_modules.Any())
            return entities;

        var entitiesMatchedByModule = (from entity in entities
                                       from module in _modules
                                       where module.Match(entity, query)
                                       select (entity, module)).ToList();

        var entitiesWithWeights = (from entity in entitiesMatchedByModule.Select(tuple => tuple.entity).Distinct()
                                   let weights = entitiesMatchedByModule.Where(pair => pair.entity.Equals(entity))
                                                                        .Select(pair => pair.module.Weight)
                                   let maxWeight = weights.Max()
                                   select (entity, maxWeight)).ToList();

        return entitiesWithWeights.OrderByDescending(tuple => tuple.maxWeight).Select(tuple => tuple.entity).ToList();
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

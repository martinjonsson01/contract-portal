using Application.Search.Modules;

namespace Application.Search;

/// <summary>
/// A general search engine that can be used for querying a collection of entities.
/// </summary>
/// <typeparam name="TEntity">The type of the entities to perform queries on.</typeparam>
public class SearchEngine<TEntity>
    where TEntity : notnull
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
    public IEnumerable<TEntity> Search(string query, IEnumerable<TEntity> entities)
    {
        ICollection<(TEntity entity, double moduleWeight)> entitiesWithModuleWeights = FindMatches(query, entities);

        IEnumerable<(TEntity entity, double weight)> weightedEntities = CalculateTotalWeights(entitiesWithModuleWeights);

        return SortByWeights(weightedEntities);
    }

    /// <summary>
    /// Registers a new <see cref="ISearchModule{TEntity}"/> to be used when checking for query matches.
    /// </summary>
    /// <param name="module">The module to add.</param>
    public void AddModule(ISearchModule<TEntity> module)
    {
        _modules.Add(module);
    }

    private static IEnumerable<(TEntity entity, double weight)> CalculateTotalWeights(
        ICollection<(TEntity entity, double moduleWeight)> entitiesWithModuleWeights)
    {
        return from entity in entitiesWithModuleWeights.Select(tuple => tuple.entity).Distinct()
               let totalWeight = CalculateTotalWeight(entity, entitiesWithModuleWeights)
               select (entity, totalWeight);
    }

    private static double CalculateTotalWeight(
        TEntity entity,
        IEnumerable<(TEntity entity, double moduleWeight)> entitiesWithModuleWeights)
    {
        return entitiesWithModuleWeights.Where(pair => pair.entity.Equals(entity)).Select(pair => pair.moduleWeight)
                                      .Sum();
    }

    private static IEnumerable<TEntity> SortByWeights(
        IEnumerable<(TEntity entity, double weight)> weightedEntities)
    {
        return weightedEntities.OrderByDescending(tuple => tuple.weight)
                                  .Select(tuple => tuple.entity);
    }

    private ICollection<(TEntity entity, double moduleWeight)> FindMatches(
        string query,
        IEnumerable<TEntity> entities)
    {
        return (from entity in entities
                from module in _modules
                where module.Match(entity, query)
                select (entity, module.Weight)).ToList();
    }
}

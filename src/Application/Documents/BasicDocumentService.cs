using Domain.Contracts;

namespace Application.Documents;

/// <summary>
/// Basic handler of documents.
/// </summary>
public class BasicDocumentService : IDocumentService
{
    private readonly IDocumentRepository _repo;

    /// <summary>
    /// Initializes a new instance of the <see cref="BasicDocumentService"/> class.
    /// </summary>
    /// <param name="repo">A place to store documents.</param>
    public BasicDocumentService(IDocumentRepository repo)
    {
        _repo = repo;
    }

    /// <inheritdoc />
    public async Task<Document> StoreAsync(string name, Stream stream)
    {
        string fileName = await _repo.StoreAsync(stream).ConfigureAwait(false);
        return new Document { Name = name, Path = fileName, };
    }
}

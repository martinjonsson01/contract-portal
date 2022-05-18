using Domain.Contracts;

namespace Application.Documents;

/// <summary>
/// A way of interacting with <see cref="Documents"/>.
/// </summary>
public interface IDocumentService
{
    /// <summary>
    ///     Stores a stream as a <see cref="Document"/>.
    /// </summary>
    /// <param name="name">What to name the <see cref="Document"/>.</param>
    /// <param name="stream">The stream of the data to store.</param>
    /// <returns>The stored <see cref="Document"/>.</returns>
    Task<Document> StoreAsync(string name, Stream stream);
}

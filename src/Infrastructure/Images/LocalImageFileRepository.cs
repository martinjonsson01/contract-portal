using Application.Images;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Images;

/// <summary>
///     Stores and reads images from the local file system.
/// </summary>
public class LocalImageFileRepository : IImageRepository
{
    private readonly IHostEnvironment _environment;
    private readonly ILogger<LocalImageFileRepository> _logger;

    /// <summary>
    ///     Constructs a local image file repository with a given environment.
    /// </summary>
    /// <param name="environment">The environment of the local host.</param>
    /// <param name="logger">A logger.</param>
    public LocalImageFileRepository(IHostEnvironment environment, ILogger<LocalImageFileRepository> logger)
    {
        _environment = environment;
        _logger = logger;
    }

    /// <summary>
    ///     Stores the image as a file.
    /// </summary>
    /// <param name="imageStream">The stream of the image to store.</param>
    /// <returns>The file identifier.</returns>
    public async Task<string> StoreAsync(Stream imageStream)
    {
        if (imageStream is null)
            throw new ArgumentNullException(nameof(imageStream));

        string fileName = Path.GetRandomFileName();
        string path = Path.Combine(
            _environment.ContentRootPath,
            _environment.EnvironmentName,
            "unsafe_uploads",
            fileName);

#pragma warning disable CA2007
        await using var fs = new FileStream(path, FileMode.Create);
#pragma warning restore CA2007
        await imageStream.CopyToAsync(fs).ConfigureAwait(false);

        _logger.LogInformation("{FileName} saved at {Path}", fileName, path);
        return fileName;
    }
}

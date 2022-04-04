using System.Diagnostics;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BlazorApp.Pages;

/// <summary>
///     Test.
/// </summary>
[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
[IgnoreAntiforgeryToken]
public class ErrorModel : PageModel
{
    private readonly ILogger<ErrorModel> _logger;

    /// <summary>
    ///     Test.
    /// </summary>
    /// <param name="logger">What.</param>
    public ErrorModel(ILogger<ErrorModel> logger) => _logger = logger;

    /// <summary>
    ///     Gets or sets test.
    /// </summary>
    public string? RequestId { get; set; }

    /// <summary>
    ///     Gets a value indicating whether test.
    /// </summary>
    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

    /// <summary>
    ///     TEst.
    /// </summary>
    public void OnGet()
    {
        _logger.LogInformation("something happened");
        RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
    }
}

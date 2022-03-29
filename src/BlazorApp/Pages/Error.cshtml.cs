namespace BlazorApp.Pages;

using Microsoft.AspNetCore.Mvc;

using System.Diagnostics;

using Microsoft.AspNetCore.Mvc.RazorPages;

/// <summary>
/// Test.
/// </summary>
[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
[IgnoreAntiforgeryToken]
public class ErrorModel : PageModel
{
    private readonly ILogger<ErrorModel> logger;

    /// <summary>
    /// Test.
    /// </summary>
    /// <param name="logger">What.</param>
    public ErrorModel(ILogger<ErrorModel> logger)
    {
        this.logger = logger;
    }

    /// <summary>
    /// Gets or sets test.
    /// </summary>
    public string? RequestId { get; set; }

    /// <summary>
    /// Gets a value indicating whether test.
    ///
    /// </summary>
    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

    /// <summary>
    /// TEst.
    /// </summary>
    public void OnGet()
    {
        RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
    }
}
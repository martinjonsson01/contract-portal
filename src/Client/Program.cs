using BlazorApp.Data;

namespace BlazorApp;

/// <summary>
///     Test.
/// </summary>
public static class Program
{
    /// <summary>
    ///     Test.
    /// </summary>
    /// <param name="args">Arguments.</param>
    public static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        _ = builder.Services.AddRazorPages();
        _ = builder.Services.AddServerSideBlazor();
        _ = builder.Services.AddSingleton<WeatherForecastService>();

        WebApplication app = builder.Build();

        TestingMethod();

        static void TestingMethod() => Console.WriteLine("gamer");

        const int test = 1;
        Console.WriteLine(test);

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            _ = app.UseExceptionHandler("/Error");
        }

        _ = app.UseStaticFiles();

        _ = app.UseRouting();

        _ = app.MapBlazorHub();
        _ = app.MapFallbackToPage("/_Host");

        app.Run();
    }
}

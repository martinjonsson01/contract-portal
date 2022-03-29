namespace BlazorApp;

using Data;

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
        builder.Services.AddRazorPages();
        builder.Services.AddServerSideBlazor();
        builder.Services.AddSingleton<WeatherForecastService>();

        WebApplication app = builder.Build();

        static void TestingMethod()
        {
            Console.WriteLine("gamer");
        }

        TestingMethod();

        const int test = 1;
        Console.WriteLine(test);

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
            app.UseExceptionHandler("/Error");

        app.UseStaticFiles();

        app.UseRouting();

        app.MapBlazorHub();
        app.MapFallbackToPage("/_Host");

        app.Run();
    }
}
using Blazor.Analytics;

using Blazored.SessionStorage;

using Blazorise;
using Blazorise.Bootstrap5;
using Blazorise.Icons.FontAwesome;

using Client;
using Client.Services.Authentication;

using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(_ => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress), });

builder.Services
       .AddGoogleAnalytics("G-01KVKPEFVQ")
       .AddBlazorise(options => { options.Immediate = true; })
       .AddBootstrap5Providers()
       .AddFontAwesomeIcons()
       .AddBlazoredSessionStorage()
       .AddScoped<ISessionService, SessionManagerService>(); // Scoped behaves like Singleton.

await builder.Build().RunAsync().ConfigureAwait(false);

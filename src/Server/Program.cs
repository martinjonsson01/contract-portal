using System.Text;

using Application;

using Infrastructure;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

IdentityModelEventSource.ShowPII = true;

builder.Services.AddAuthentication(options =>
       {
           options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
           options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
       })
       .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
       {
           options.Audience = "https://localhost:7223/";
           options.Authority = "https://localhost:7223/";

           options.Configuration = new OpenIdConnectConfiguration();

           const string environmentVariableKey = "prodigo_portal_jwt_secret";
           string? jwtSecret = Environment.GetEnvironmentVariable(environmentVariableKey);
           if (jwtSecret is null)
               throw new ArgumentException("No environment variable defined for " + environmentVariableKey);

           options.TokenValidationParameters = new TokenValidationParameters
           {
               ValidateIssuer = true,
               ValidateAudience = true,
               ValidateLifetime = true,
               ValidateIssuerSigningKey = true,
               ValidIssuer = builder.Configuration["Jwt:Issuer"],
               ValidAudience = builder.Configuration["Jwt:Issuer"],
               IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
           };
       });
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(
        "Bearer",
        new AuthorizationPolicyBuilder()
            .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
            .RequireAuthenticatedUser().Build());
});

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages(options =>
{
    _ = options.Conventions.AllowAnonymousToAreaFolder("root", "/wwwroot").AllowAnonymousToPage("/index.html");
});
builder.Services.AddApplication();
builder.Services.AddInfrastructure();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers()
       .AddNewtonsoftJson();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    _ = app.UseSwagger();
    _ = app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Blazor API V1"); });
}
else
{
    _ = app.UseExceptionHandler("/Error");

    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    _ = app.UseHsts();
}

app.UseHttpLogging();

app.UseHttpsRedirection();

string imagesDirectory = Path.Combine(
    app.Environment.ContentRootPath,
    app.Environment.EnvironmentName,
    "unsafe_uploads");
Directory.CreateDirectory(imagesDirectory);
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(imagesDirectory),
    RequestPath = "/images",
});

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.UseAuthentication();
app.UseAuthorization();

app.Run();

/// <summary>
/// A partial class that makes the entire class public.
/// </summary>
#pragma warning disable CA1050
public partial class Program
#pragma warning restore CA1050
{
}

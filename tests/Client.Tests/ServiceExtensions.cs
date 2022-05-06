using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;

using Microsoft.Extensions.DependencyInjection;

namespace Client.Tests;

public static class ServiceExtensions
{
    public static MockHttpMessageHandler AddMockHttpClient(this TestServiceProvider services)
    {
        var mockHttpHandler = new MockHttpMessageHandler();
        var httpClient = mockHttpHandler.ToHttpClient();
        httpClient.BaseAddress = new Uri("http://localhost");
        services.AddSingleton(httpClient);
        return mockHttpHandler;
    }

    public static MockedRequest RespondJson<T>(this MockedRequest request, T content)
    {
        request.Respond(req =>
        {
            var response = new HttpResponseMessage { StatusCode = HttpStatusCode.OK, };
            response.Content = new StringContent(JsonSerializer.Serialize(content));
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return response;
        });
        return request;
    }

    public static MockedRequest RespondJson<T>(this MockedRequest request, Func<T> contentProvider)
    {
        request.Respond(req =>
        {
            var response = new HttpResponseMessage { StatusCode = HttpStatusCode.OK, };
            response.Content = new StringContent(JsonSerializer.Serialize(contentProvider()));
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return response;
        });
        return request;
    }
}

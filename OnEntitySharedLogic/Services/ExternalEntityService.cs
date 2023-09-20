using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using OnEntitySharedLogic.Extensions;

namespace OnEntitySharedLogic.Services;

[Obsolete]
public class ExternalEntityService : IExternalEntityService
{
    private readonly JsonSerializerOptions _serializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    public async Task<T> GetExternalEntityAsync<T>(string requestUri, HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
    {
        //used to make http requests from the current app to external apis
        var accessToken = httpContextAccessor.GetAccessToken();

        if (!accessToken.IsNullOrEmpty()) //sometimes i don't require the access token
        {
            //Include the access token in the request headers so you can make HTTP requests to secured endpoints by authorization attribute, of University Scheduler Api
            httpClient.AddAccessTokenBearerScheme(accessToken!);
        }

        var response = await httpClient.GetAsync($"{requestUri}");
        response.EnsureSuccessStatusCode();

        //Stream = sequence of bits used for communicating between 2 parts
        var responseString = await response.Content.ReadAsStringAsync();
        var resource = JsonSerializer.Deserialize<T>(responseString, _serializerOptions);

        return resource ?? throw new InvalidDataException("Could not deserialize the response stream");
    }

    public async Task AddExternalEntityAsync<T>(T entityToPost, string requestUri, HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
    {
        //used to make http requests from the current app to external apis
        var accessToken = httpContextAccessor.GetAccessToken();

        if (!accessToken.IsNullOrEmpty()) //sometimes i don't require the access token
        {
            //Include the access token in the request headers so you can make HTTP requests to secured endpoints by authorization attribute, of University Scheduler Api
            httpClient.AddAccessTokenBearerScheme(accessToken!);
        }

        var serialisedEntity = JsonSerializer.Serialize(entityToPost, _serializerOptions);

        var contentToPost = new StringContent(serialisedEntity, Encoding.UTF8, "application/json");
        var response = await httpClient.PostAsync($"{requestUri}", contentToPost);
        response.EnsureSuccessStatusCode();
    }
}

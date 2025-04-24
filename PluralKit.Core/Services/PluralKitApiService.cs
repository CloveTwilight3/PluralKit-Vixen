using System.Net.Http.Headers;
using System.Text.Json;

using Newtonsoft.Json.Linq;

using Serilog;

namespace PluralKit.Core;

public class PluralKitApiService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger _logger;
    private readonly string _apiBaseUrl = "https://api.pluralkit.me/v2";

    public PluralKitApiService(ILogger logger)
    {
        _logger = logger.ForContext<PluralKitApiService>();
        _httpClient = new HttpClient();
    }

    public async Task<(bool success, JObject? data, string? error)> ValidateToken(string token)
    {
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{_apiBaseUrl}/systems/@me");
            request.Headers.Authorization = new AuthenticationHeaderValue("Token", token);

            var response = await _httpClient.SendAsync(request);
            
            if (!response.IsSuccessStatusCode)
            {
                return (false, null, $"API returned status code {response.StatusCode}");
            }

            var content = await response.Content.ReadAsStringAsync();
            var systemData = JObject.Parse(content);
            
            return (true, systemData, null);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error validating PluralKit token");
            return (false, null, $"Error: {ex.Message}");
        }
    }
    
    public async Task<(bool success, JObject? data, string? error)> GetSystem(string token)
    {
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{_apiBaseUrl}/systems/@me");
            request.Headers.Authorization = new AuthenticationHeaderValue("Token", token);

            var response = await _httpClient.SendAsync(request);
            
            if (!response.IsSuccessStatusCode)
            {
                return (false, null, $"API returned status code {response.StatusCode}");
            }

            var content = await response.Content.ReadAsStringAsync();
            var systemData = JObject.Parse(content);
            
            return (true, systemData, null);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error fetching system from PluralKit API");
            return (false, null, $"Error: {ex.Message}");
        }
    }

    public async Task<(bool success, JArray? data, string? error)> GetMembers(string token)
    {
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{_apiBaseUrl}/systems/@me/members");
            request.Headers.Authorization = new AuthenticationHeaderValue("Token", token);

            var response = await _httpClient.SendAsync(request);
            
            if (!response.IsSuccessStatusCode)
            {
                return (false, null, $"API returned status code {response.StatusCode}");
            }

            var content = await response.Content.ReadAsStringAsync();
            var membersData = JArray.Parse(content);
            
            return (true, membersData, null);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error fetching members from PluralKit API");
            return (false, null, $"Error: {ex.Message}");
        }
    }
    
    public async Task<(bool success, JArray? data, string? error)> GetFronters(string token)
    {
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{_apiBaseUrl}/systems/@me/fronters");
            request.Headers.Authorization = new AuthenticationHeaderValue("Token", token);

            var response = await _httpClient.SendAsync(request);
            
            // If we get a 404, it means there are no fronters
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return (true, new JArray(), null);
            }
            
            if (!response.IsSuccessStatusCode)
            {
                return (false, null, $"API returned status code {response.StatusCode}");
            }

            var content = await response.Content.ReadAsStringAsync();
            var data = JObject.Parse(content);
            
            if (data.TryGetValue("members", out var members) && members is JArray membersArray)
            {
                return (true, membersArray, null);
            }
            
            return (true, new JArray(), null);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error fetching fronters from PluralKit API");
            return (false, null, $"Error: {ex.Message}");
        }
    }
}

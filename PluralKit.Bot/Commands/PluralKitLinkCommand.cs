using Myriad.Types;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace PluralKit.Bot;

public class PluralKitLinkCommand
{
    private readonly ModelRepository _repo;
    private readonly HttpClient _httpClient;

    public PluralKitLinkCommand(ModelRepository repo, HttpClient httpClient)
    {
        _repo = repo;
        _httpClient = httpClient;
    }

    public async Task LinkPluralKitToken(InteractionContext ctx)
    {
        await ctx.Defer();

        var token = ctx.Event.Data!.Options![0].Value!.ToString();
        
        // Validate the token by making a request to the PluralKit API
        var isValid = await ValidatePluralKitTokenAsync(token);
        
        if (!isValid.success)
        {
            await ctx.Reply($"{Emojis.Error} Could not validate your PluralKit token. Please check that it's correct and try again.");
            return;
        }

        // Store the token in the database for the user
        await _repo.SavePluralKitToken(ctx.User.Id, token);

        // Send success message with the system name
        await ctx.Reply($"{Emojis.Success} Successfully linked to PluralKit system **{isValid.systemName}**! You can now use The Vixen to interact with your PluralKit data.");
    }

    private async Task<(bool success, string systemName)> ValidatePluralKitTokenAsync(string token)
    {
        try
        {
            // Make a request to the PluralKit API to verify the token
            var request = new HttpRequestMessage(HttpMethod.Get, "https://api.pluralkit.me/v2/systems/@me");
            request.Headers.Add("Authorization", token);
            
            var response = await _httpClient.SendAsync(request);
            
            if (!response.IsSuccessStatusCode)
                return (false, string.Empty);
                
            var json = await response.Content.ReadAsStringAsync();
            var systemData = JsonSerializer.Deserialize<JsonElement>(json);
            
            var systemName = systemData.TryGetProperty("name", out var name) && name.ValueKind != JsonValueKind.Null 
                ? name.GetString() 
                : "Unnamed System";
                
            return (true, systemName ?? "Unnamed System");
        }
        catch
        {
            return (false, string.Empty);
        }
    }
}

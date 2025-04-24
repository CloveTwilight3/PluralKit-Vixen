using SqlKata;

namespace PluralKit.Core;

public partial class ModelRepository
{
    public Task<UserPluralKitToken?> GetUserPluralKitToken(ulong userId)
    {
        var query = new Query("user_pluralkit_tokens").Where("user_id", userId);
        return _db.QueryFirst<UserPluralKitToken?>(query);
    }

    public async Task SaveUserPluralKitToken(ulong userId, string token, Guid? systemId = null)
    {
        _logger.Information("Saving PluralKit token for user {UserId}", userId);
        
        var query = new Query("user_pluralkit_tokens").AsInsert(new {
            user_id = userId,
            token = token,
            system_id = systemId,
            created = new UnsafeLiteral("now()")
        });
        
        await _db.ExecuteQuery(query, 
            "on conflict (user_id) do update set token = EXCLUDED.token, system_id = EXCLUDED.system_id");
    }

    public async Task DeleteUserPluralKitToken(ulong userId)
    {
        _logger.Information("Removing PluralKit token for user {UserId}", userId);
        
        var query = new Query("user_pluralkit_tokens").AsDelete().Where("user_id", userId);
        await _db.ExecuteQuery(query);
    }
    
    public async Task UpdateUserPluralKitSystemId(ulong userId, Guid systemId)
    {
        _logger.Information("Updating PluralKit system ID for user {UserId}", userId);
        
        var query = new Query("user_pluralkit_tokens")
            .AsUpdate(new { system_id = systemId })
            .Where("user_id", userId);
        
        await _db.ExecuteQuery(query);
    }
}

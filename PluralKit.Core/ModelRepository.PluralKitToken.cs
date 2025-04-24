using Dapper;
using NodaTime;
using System.Threading.Tasks;

namespace PluralKit.Core;

public partial class ModelRepository
{
    public async Task SavePluralKitToken(ulong userId, string token)
    {
        var now = SystemClock.Instance.GetCurrentInstant();
        
        await _db.Execute(conn => conn.ExecuteAsync(
            "INSERT INTO user_pluralkit_tokens (user_id, token, created) VALUES (@UserId, @Token, @CreatedAt) " +
            "ON CONFLICT (user_id) DO UPDATE SET token = @Token, created = @CreatedAt",
            new
            {
                UserId = userId,
                Token = token,
                CreatedAt = now
            }
        ));
    }

    public async Task<string?> GetPluralKitToken(ulong userId)
    {
        return await _db.Execute(conn => conn.QueryFirstOrDefaultAsync<string>(
            "SELECT token FROM user_pluralkit_tokens WHERE user_id = @UserId",
            new { UserId = userId }
        ));
    }
    
    public async Task RemovePluralKitToken(ulong userId)
    {
        await _db.Execute(conn => conn.ExecuteAsync(
            "DELETE FROM user_pluralkit_tokens WHERE user_id = @UserId",
            new { UserId = userId }
        ));
    }
}

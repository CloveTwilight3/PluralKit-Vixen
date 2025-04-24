using NodaTime;

namespace PluralKit.Core;

public class UserPluralKitToken
{
    public ulong UserId { get; set; }
    public string Token { get; set; }
    public Guid? SystemId { get; set; }
    public Instant Created { get; set; }
}

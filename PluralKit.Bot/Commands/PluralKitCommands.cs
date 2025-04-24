using PluralKit.Core;

namespace PluralKit.Bot;

public class PluralKitCommands
{
    private readonly PluralKitLink _pkLink;
    
    public PluralKitCommands(PluralKitLink pkLink)
    {
        _pkLink = pkLink;
    }
    
    public void RegisterCommands(Bot bot)
    {
        // Register the PluralKit Link commands
        bot.Command("pklink", "Link your PluralKit system from the official bot", async (ctx) => 
            await _pkLink.LinkPluralKit(ctx));
        
        bot.Command("pkunlink", "Unlink your PluralKit system", async (ctx) => 
            await _pkLink.UnlinkPluralKit(ctx));
        
        bot.Command("pksystem", "View your linked PluralKit system", async (ctx) => 
            await _pkLink.ShowPluralKitSystem(ctx));
        
        bot.Command("pkmembers", "View members from your linked PluralKit system", async (ctx) => 
            await _pkLink.ShowPluralKitMembers(ctx));
        
        bot.Command("pkfronters", "View current fronters from your linked PluralKit system", async (ctx) => 
            await _pkLink.ShowPluralKitFronters(ctx));
    }
}

using Autofac;

namespace PluralKit.Bot;

public static class BotServiceExtensions
{
    public static void RegisterBotServices(this ContainerBuilder builder)
    {
        // Register command handlers
        builder.RegisterType<PluralKitLink>().AsSelf().SingleInstance();
        builder.RegisterType<PluralKitCommands>().AsSelf().SingleInstance();
        
        // You'll need to find the appropriate place to call this method
        // Typically in the Bot's initialization or where other command handlers are registered
    }
}

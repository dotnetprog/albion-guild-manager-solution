using AGM.Application;
using AGM.Database;
using AGM.DiscordBot;
using AGM.DiscordBot.Configuration;
using AGM.DiscordBot.Processing;
using AGM.Domain.Abstractions;
using AGM.Domain.Identity;
using AGM.EntityFramework;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<AGMDiscordBotWorker>();
builder.Services
    .ConfigureAGMDatabase(builder.Configuration)
    .AddDatabaseServices()
    .ConfigureAGMApplicationLayer()
    .Configure<DiscordSettings>(builder.Configuration.GetSection("Discord"))
    .AddSingleton<DiscordSocketClient>((sp) =>
    {
        var config = new DiscordSocketConfig()
        {
            AlwaysDownloadUsers = true,
            GatewayIntents = GatewayIntents.All,
            LogLevel = LogSeverity.Info,
            UseInteractionSnowflakeDate = false
        };
        var client = new DiscordSocketClient(config);
        return client;
    })
    .AddScoped<ITenantProvider, MemoryTenantProvider>()
    .AddSingleton<InteractionService>((sp) =>
    {
        var client = sp.GetRequiredService<DiscordSocketClient>();
        return new InteractionService(client, new InteractionServiceConfig()
        {
            LogLevel = LogSeverity.Info
        });
    })
    .AddScoped<IDiscordIdentityManager, SocketDiscordIdentityManager>()
    .AddScoped<IScopedDiscordProcessingService, AGMDiscordBotProcessingService>()
    .AddLogging((builder) =>
            builder
            .AddFilter("Microsoft.EntityFrameworkCore.Database", LogLevel.Error)
            .AddConsole()); ;
var host = builder.Build();
host.Run();

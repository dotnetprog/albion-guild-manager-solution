﻿
using AGM.DiscordBot.Factory;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using System.Reflection;

namespace AGM.DiscordBot.Interactions
{
    public class DiscordInterfactionRegisteringService : InteractionRegisteringService
    {
        private readonly DiscordSocketClient _client;
        private readonly InteractionService _commands;
        private readonly IServiceProvider _services;
        private readonly IScopedDiscordFactory _scopedDiscordFactory;
        private readonly ILogger _logger;
        public DiscordInterfactionRegisteringService(
            DiscordSocketClient client,
            InteractionService commands,
            IServiceProvider services,
            ILogger<DiscordInterfactionRegisteringService> logger,
            IScopedDiscordFactory scopedDiscordFactory)
        {
            _client = client;
            _commands = commands;
            _services = services;
            _logger = logger;
            _scopedDiscordFactory = scopedDiscordFactory;
        }
        public async Task InitializeAsync()
        {
            // add the public modules that inherit InteractionModuleBase<T> to the InteractionService
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);

            await _commands.RegisterCommandsGloballyAsync(true);

            // process the InteractionCreated payloads to execute Interactions commands
            _client.InteractionCreated += HandleInteraction;

            // process the command execution results 
            _commands.SlashCommandExecuted += SlashCommandExecuted;
            _commands.ContextCommandExecuted += ContextCommandExecuted;
            _commands.ComponentCommandExecuted += ComponentCommandExecuted;
            _commands.ComponentCommandExecuted += ComponentCommandExecuted;
        }
        private Task ComponentCommandExecuted(ComponentCommandInfo arg1, Discord.IInteractionContext arg2, IResult arg3)
        {
            if (!arg3.IsSuccess)
            {
                switch (arg3.Error)
                {
                    case InteractionCommandError.UnmetPrecondition:
                    case InteractionCommandError.UnknownCommand:
                    case InteractionCommandError.BadArgs:
                    case InteractionCommandError.Exception:
                    case InteractionCommandError.Unsuccessful:
                    default:
                        _logger.LogError($"{arg3.Error} {arg3.ErrorReason}");
                        break;
                }
            }

            return Task.CompletedTask;
        }

        private Task ContextCommandExecuted(ContextCommandInfo arg1, Discord.IInteractionContext arg2, IResult arg3)
        {
            if (!arg3.IsSuccess)
            {
                switch (arg3.Error)
                {
                    case InteractionCommandError.UnmetPrecondition:
                    case InteractionCommandError.UnknownCommand:
                    case InteractionCommandError.BadArgs:
                    case InteractionCommandError.Exception:
                    case InteractionCommandError.Unsuccessful:
                    default:
                        _logger.LogError($"{arg3.Error} {arg3.ErrorReason}");
                        break;
                }
            }

            return Task.CompletedTask;
        }

        private Task SlashCommandExecuted(SlashCommandInfo arg1, Discord.IInteractionContext arg2, IResult arg3)
        {
            if (!arg3.IsSuccess)
            {
                switch (arg3.Error)
                {
                    case InteractionCommandError.UnmetPrecondition:
                    case InteractionCommandError.UnknownCommand:
                    case InteractionCommandError.BadArgs:
                    case InteractionCommandError.Exception:
                    case InteractionCommandError.Unsuccessful:
                    default:
                        _logger.LogError($"{arg3.Error} {arg3.ErrorReason}");
                        break;
                }
            }

            return Task.CompletedTask;
        }

        private async Task HandleInteraction(SocketInteraction arg)
        {
            try
            {


                // We don't want the bot to respond to itself or other bots.
                if (arg.User.Id == _client.CurrentUser.Id || arg.User.IsBot)
                    return;
                if (!arg.GuildId.HasValue)
                {
                    return;
                }
                var guild = _client.GetGuild(arg.GuildId.Value);
                var guildUser = guild.GetUser(arg.User.Id);
                using (var scope = await _scopedDiscordFactory.Create(guild, guildUser))
                {
                    var ctx = new SocketInteractionContext(_client, arg);

                    await _commands.ExecuteCommandAsync(ctx, _services);
                }

                // create an execution context that matches the generic type parameter of your InteractionModuleBase<T> modules

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                // if a Slash Command execution fails it is most likely that the original interaction acknowledgement will persist. It is a good idea to delete the original
                // response, or at least let the user know that something went wrong during the command execution.
                if (arg.Type == InteractionType.ApplicationCommand)
                {
                    await arg.GetOriginalResponseAsync().ContinueWith(async (msg) => await msg.Result.DeleteAsync());
                }
            }
        }
    }
}

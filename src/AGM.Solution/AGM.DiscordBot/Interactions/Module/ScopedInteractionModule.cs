using AGM.DiscordBot.Factory;
using Discord;
using Discord.Interactions;
using System.Text.RegularExpressions;

namespace AGM.DiscordBot.Interactions.Module
{
    public abstract class ScopedInteractionModule : InteractionModuleBase<SocketInteractionContext>
    {
        protected readonly IScopedDiscordFactory _ScopeFactory;
        private string CommandTitle { get; set; }
        protected ScopedInteractionModule(IScopedDiscordFactory scopeFactory, string commandTitle)
        {
            _ScopeFactory = scopeFactory;
            CommandTitle = commandTitle;
        }
        protected Task<IServiceScope> CreateScope() => _ScopeFactory.Create(Context.Guild, (IGuildUser)Context.User);
        protected async Task ShowInProgress()
        {
            var embed = new EmbedBuilder()
                .WithTitle($"{CommandTitle} notification")
                .WithDescription("In progress...")
                .WithColor(Color.Blue)
                .Build();

            await RespondAsync(embed: embed, ephemeral: true);

        }
        protected DateTime GetTimeFromWhen(string input)
        {
            if (input.ToLower() == "now")
            {
                return DateTime.UtcNow;
            }
            var regexResult = Regex.Matches(input, @"\d+");
            var hour = int.Parse(regexResult[0].Value);
            var min = int.Parse(regexResult[1].Value);
            return DateTime.UtcNow.AddHours(hour).AddMinutes(min);
        }
        protected bool ValidateWhen(string input)
        {
            if (input.ToLower() == "now")
            {
                return true;
            }
            var regexResult = Regex.Matches(input, @"\d+");
            return regexResult.Count == 2;
        }
        protected bool ValidateIsGuid(string input)
        {
            return Guid.TryParse(input, out Guid SubTypeId);
        }

        protected async Task ShowValidation(string message)
        {

            var embed = new EmbedBuilder().WithTitle($"{CommandTitle} Validation").WithDescription(message).WithColor(Color.Red).Build();
            await FollowupAsync(embed: embed, ephemeral: true);

        }
    }
}

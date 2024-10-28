using Discord;
using Discord.Interactions;
using System.Text.RegularExpressions;

namespace AGM.DiscordBot.Interactions.Module.Utility
{
    public class AssignRolesToUserListModule : InteractionModuleBase<SocketInteractionContext>
    {
        private readonly Regex regexPlayers = new Regex(@"\<\@(.*?)\>");
        [EnabledInDm(false)]
        [DefaultMemberPermissions(GuildPermission.ManageRoles)]
        [SlashCommand("assign-role", "This command assign a role to selected users", false, RunMode.Async)]
        public async Task Run(IRole role, [Summary("users", "Mention the users you want to assign the role")] string usersRaw)
        {

            await RespondAsync(embed:
                new EmbedBuilder()
                .WithTitle("Assign Role Command Notification")
                .WithDescription("In progress...")
                .WithColor(Color.Blue)
                .Build(), ephemeral: true);

            var matches = regexPlayers.Matches(usersRaw);
            try
            {
                foreach (Match match in matches)
                {
                    var userid = MentionUtils.ParseUser(match.Value);
                    var guilduser = Context.Guild.GetUser(userid);
                    await guilduser.AddRoleAsync(role.Id);
                }
            }
            catch
            {
                await FollowupAsync(embed:
                   new EmbedBuilder()
                   .WithTitle("Assign Role Command Error")
                   .WithDescription("Seems like the bot does not have enough permissions to assign the role to those users. Make sure the role that represents the bot is higher than the role he tries to assign.")
                   .WithColor(Color.Red)
                   .Build(), ephemeral: true);
            }





            await FollowupAsync(embed:
                new EmbedBuilder()
                .WithTitle("Assign Role Command Notification")
                .WithDescription("Role assigned Succeded !")
                .WithColor(Color.Green)
                .Build(), ephemeral: true);
        }
        [ComponentInteraction("user_selection")]
        public async Task UserSelection(IGuildUser[] selectedUsers)
        {

        }
    }
}

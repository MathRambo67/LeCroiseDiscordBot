using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using Discord;
using System.Threading.Tasks;
using System.Linq;
using Discord.WebSocket;

namespace DiscordBot.Modules
{
    [Group("admin")]
    [RequireUserPermission(GuildPermission.Administrator)]
    public class Admin : ModuleBase<SocketCommandContext>
    {
        [Command("clean")]
        [Alias("cls", "eff")]
        public async Task CleanAsync(int count = 10, string id = "")
        {
            var embed = new EmbedBuilder();
            var channelId = !String.IsNullOrEmpty(id) ? Convert.ToUInt64(id) : Context.Channel.Id;
            var Channel = Context.Guild.GetChannel(channelId) as SocketTextChannel;

            var messages = await Channel.GetMessagesAsync(count).FlattenAsync();
            var filteredMessages = messages.Where(x => (DateTimeOffset.UtcNow - x.Timestamp).TotalDays <= 14);
            var resCount = filteredMessages.Count();

            if (resCount == 0)
            {
                embed.WithTitle("Erreur durant la suppression")
                     .WithDescription("Il n'y a aucun message à supprimer")
                     .WithThumbnailUrl(Context.Client.CurrentUser.GetAvatarUrl(ImageFormat.Auto, 512))
                     .AddField($@"Commande demandée par ", $@"{Context.User.Mention}", true)
                     .WithColor(Color.Orange);

                await ReplyAsync(embed: embed.Build());
            }
            else
            {
                await (Channel as ITextChannel).DeleteMessagesAsync(messages);
                await ReplyAsync($"** {resCount} messages supprimé(s) dans le canal {Channel.Mention} :wastebasket: !** :ok_hand: ");
                await Task.Delay(300);
                var RemoveLast = await Context.Channel.GetMessagesAsync(1, CacheMode.AllowDownload).FlattenAsync();
                await (Context.Channel as ITextChannel).DeleteMessagesAsync(RemoveLast);
            }
        }
        [Command("info")]
        public async Task ServerInfo()
        {
             var RemoveLast = await Context.Channel.GetMessagesAsync(1, CacheMode.AllowDownload).FlattenAsync();
             await (Context.Channel as ITextChannel).DeleteMessagesAsync(RemoveLast);
            var embed = new EmbedBuilder();

            var userCount = Context.Guild.Users.Count.ToString();

            embed.WithTitle("Server Informations").WithDescription("This command provide server informations")
                .AddField("Members in server : ", $"*{userCount} user(s)*", false)
                .AddField("Numbers of Roles availables", $"*{Context.Guild.Roles.Count}*", false)
                .AddField("Numbers of Text Channels", $"*{Context.Guild.TextChannels.Count} text channels*", false)
                .AddField("Numbers of Voices Channels", $"*{Context.Guild.VoiceChannels.Count}*", false)
                .WithFooter($"Requested by {Context.User.Username}")
                .WithTimestamp(DateTimeOffset.Now)
                .WithThumbnailUrl(Context.Guild.IconUrl)
                .WithColor(Discord.Color.Blue);

            await ReplyAsync(embed: embed.Build());
        }
    }
}

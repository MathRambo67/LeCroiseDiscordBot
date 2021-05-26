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
                     .WithDescription("Il n'y a aucun message à supprimé")
                     .WithThumbnailUrl(Context.Client.CurrentUser.GetAvatarUrl(ImageFormat.Auto, 512))
                     .AddField($@"Commande demandé par ", $@"{Context.User.Mention}", true)
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

    }
}

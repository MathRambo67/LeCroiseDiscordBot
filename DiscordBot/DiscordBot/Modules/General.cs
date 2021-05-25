using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Modules
{
    public class General : ModuleBase<SocketCommandContext>
    {
        [Command("ping")]
        public async Task PingAsync()
        {
            await ReplyAsync("Pong !");
        }

        [Command("info")]
        public async Task ServerInfo()
        {
            var embed = new EmbedBuilder();
            
            var userCount = Context.Guild.Users.Count.ToString();

            embed.WithTitle("Server Informations").WithDescription("This command provide server informations")
                .AddField("Members in server : ", $"*{userCount} user(s)*", false)
                .AddField("Numbers of Roles availables", $"*{Context.Guild.Roles.Count}*", false)
                .AddField("Numbers of Text Channels", $"*{Context.Guild.TextChannels.Count} text channels*", false)
                .AddField("Numbers of Voices Channels",$"*{Context.Guild.VoiceChannels.Count}*", false)
                .WithFooter($"Requested by {Context.User.Username}")
                .WithTimestamp(DateTimeOffset.Now)
                .WithThumbnailUrl(Context.Guild.IconUrl)
                .WithColor(Discord.Color.Blue);

            await ReplyAsync(embed: embed.Build());


        }
    }
}

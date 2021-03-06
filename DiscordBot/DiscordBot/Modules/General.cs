using Discord;
using Discord.Commands;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DiscordBot.Tools;

namespace DiscordBot.Modules
{
    public class General : ModuleBase<SocketCommandContext>
    {
        [Command("ping")]
        public async Task PingAsync()
        {
            await ReplyAsync("Pong !");
        }

        [Command("steam")]

        public async Task SteamAsync(string id)
        {
            var url = $"https://store.steampowered.com/app/{id}";
            var meta = SteamHtmlEmbed.GetMetaFromUrl(url, Context);
            await ReplyAsync(":thumbsup::thumbsup: ", embed: meta.Build()) ;
        }


    }
}

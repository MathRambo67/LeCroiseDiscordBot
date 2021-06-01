using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Modules
{

    [Group("invites")]
    public class GameInvites : ModuleBase<SocketCommandContext>
    {

        [Command("create")]
        public async Task CreateInvite()
        {
            var embed = new EmbedBuilder();
           //embed.WithColor(Color.Blue)
           //     .WithTitle("Créer vos invitations pour avertir les membres inscrits")
           //     .WithField("")

        }

    }
}

using Discord.Addons.Hosting;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DiscordBot.Services
{
    public class EventsHandler : InitializedService
    {
        private readonly IServiceProvider _provider;
        private readonly DiscordSocketClient _client;
        private readonly CommandService _service;

        public EventsHandler(IServiceProvider provider, DiscordSocketClient client, CommandService service)
        {
            _client = client;
            _provider = provider;
            _service = service;
        }

        public override async Task InitializeAsync(CancellationToken cancellationToken)
        {
            //Manage Event 
            _client.UserJoined += OnUserJoin;
            _client.UserLeft += OnUserLeft;
            await _service.AddModulesAsync(Assembly.GetEntryAssembly(), this._provider);
        }

        private async Task OnUserLeft(SocketGuildUser guildUser)
        {
            var channel = _client.GetChannel(guildUser.Guild.DefaultChannel.Id) as SocketTextChannel;
            await channel.SendMessageAsync($"See you later {guildUser.Username} !");

        }

        private async Task OnUserJoin(SocketGuildUser guildUser)
        {
            var channel = _client.GetChannel(guildUser.Guild.DefaultChannel.Id) as SocketTextChannel;
            await channel.SendMessageAsync($"Welcome {guildUser.Mention} to {channel.Guild.Name}");

        }
    }
}

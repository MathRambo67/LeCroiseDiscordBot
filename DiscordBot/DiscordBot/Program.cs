using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Discord.Addons.Hosting;
using Discord.WebSocket;
using Discord.Commands;
using Discord;
using Microsoft.Extensions.DependencyInjection;
using DiscordBot.Services;

namespace DiscordBot
{
    class Program
    {
        static async Task Main()
        {
            var builder = new HostBuilder()
            .ConfigureAppConfiguration(x =>
            {
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", false, true)
                    .Build();

                x.AddConfiguration(configuration);
            })
            .ConfigureLogging(x =>
            {
                x.AddConsole();
                x.SetMinimumLevel(LogLevel.Debug);

            })
            .ConfigureDiscordHost<DiscordSocketClient>((context, config) =>
            {
                config.SocketConfig = new DiscordSocketConfig
                {
                    LogLevel = LogSeverity.Debug,
                    AlwaysDownloadUsers = true,
                    MessageCacheSize = 200
                };

                config.Token = context.Configuration["Token"];
            })
            .UseCommandService((context, config) =>
            {
                config.CaseSensitiveCommands = true;
                config.LogLevel = LogSeverity.Debug;
                config.DefaultRunMode = RunMode.Sync;
            })
            .ConfigureServices((context, services) =>
            {
                services
                .AddHostedService<CommandHandler>()
                .AddHostedService<EventsHandler>();
                
            })
            .UseConsoleLifetime();

            var host = builder.Build();

            using (host)
            {
                await host.RunAsync();
            }
        }
    }
}

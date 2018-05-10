using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Net.WebSocket;
using Newtonsoft.Json;
using Tweescord.Commands;

namespace Tweescord
{
    class Discord
    {
        public static Discord Instance { get; private set; }

        public Dictionary<string, CommandBase> Commands { get; private set; }
        public DiscordClient DiscordClient { get; private set; }
        public DiscordConfig DiscordConfig { get; private set; }

        private DiscordChannel _channel;

        public Discord()
        {
            Commands = new Dictionary<string, CommandBase>();
            Instance = this;

            Commands.Add("help", new HelpCommand());
            Commands.Add("follow", new FollowCommand());
            Commands.Add("unfollow", new UnfollowCommand());
            Commands.Add("search", new SearchUsersCommand());
            Commands.Add("uptime", new UptimeCommand());
        }

        public async void Connect(string configFile)
        {
            DiscordConfig = JsonConvert.DeserializeObject<DiscordConfig>(File.ReadAllText(configFile));
            var config = new DiscordConfiguration
            {
                Token = DiscordConfig.Token,
                TokenType = TokenType.Bot,

                AutoReconnect = true,
                LogLevel = LogLevel.Debug,
                UseInternalLogHandler = true
            };
            DiscordClient = new DiscordClient(config);
            DiscordClient.SetWebSocketClient<WebSocket4NetCoreClient>();
            DiscordClient.MessageCreated += MessageCreated;

            await DiscordClient.ConnectAsync();

            if (DiscordConfig.Channel != 0)
            {
                _channel = DiscordClient.GetChannelAsync(DiscordConfig.Channel).Result;
            }
        }


        public void SendInvalidCommand(CommandBase command)
        {
            SendMessage($"{command.Name}: **Invalid Command Usage!**\n`{command.Usage}`");
        }

        public void SendCommandMessage(CommandBase command, string message)
        {
            SendMessage($"**{command.Name}**:{Environment.NewLine}{message}");
        }

        public void SendMessage(string text)
        {
            if (_channel == null)
            {
                return;
                
            }
            DiscordClient.SendMessageAsync(_channel, text);
        }

        public void SendMessage(DiscordEmbed discordEmbed)
        {
            if (_channel == null)
            {
                return;

            }
            DiscordClient.SendMessageAsync(_channel, embed: discordEmbed);
        }


        private Task MessageCreated(MessageCreateEventArgs messageCreateEventArgs)
        {
            if(messageCreateEventArgs.MentionedUsers.All(d => d.Id != DiscordClient.CurrentUser.Id))
            {
                return Task.CompletedTask;
            }
            if (DiscordConfig.Channel == 0)
            {
                DiscordConfig.Channel = messageCreateEventArgs.Channel.Id;
                _channel = DiscordClient.GetChannelAsync(DiscordConfig.Channel).Result;
            }
            else if (messageCreateEventArgs.Channel.Id != DiscordConfig.Channel)
            {
                return Task.CompletedTask;
            }

            HandleCommand(messageCreateEventArgs.Message.Content.Replace(DiscordClient.CurrentUser.Mention, ""));

            return Task.CompletedTask;
        }

        private void HandleCommand(string text)
        {
            var parsed = ParseCommand(text);
            if (!Commands.ContainsKey(parsed.name))
            {
                SendMessage($"Unknown command *{parsed.name}*");
                return;
            }
            Commands[parsed.name].Execute(this, parsed.args);
        }
        
        private (string name, string[] args) ParseCommand(string text)
        {
            string[] split = text.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            string name = split[0];

            string[] args = new string[split.Length - 1];
            Array.Copy(split, 1, args, 0, args.Length);

            return (name, args);
        }
    }
}

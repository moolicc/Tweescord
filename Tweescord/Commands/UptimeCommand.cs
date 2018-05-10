using System;
using System.Collections.Generic;
using System.Text;

namespace Tweescord.Commands
{
    class UptimeCommand : CommandBase
    {
        public override string Name => "uptime";
        public override string Usage => "uptime";
        public override string Description => "Displays the amount of time the bot has been online.";

        public DateTime StartTime { get; private set; }

        public UptimeCommand()
        {
            StartTime = DateTime.UtcNow;
        }

        public override void Execute(Discord discord, params string[] parameters)
        {
            discord.SendCommandMessage(this, $"The bot has been online since: {StartTime}.{Environment.NewLine} (UTC)" +
                $"Total time the bot has been online: {TimeSpan.FromTicks(DateTime.UtcNow.Ticks - StartTime.Ticks).TotalDays:0.00} days");
        }
    }
}

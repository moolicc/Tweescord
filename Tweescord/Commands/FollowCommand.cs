using System;
using System.Collections.Generic;
using System.Text;

namespace Tweescord.Commands
{
    class FollowCommand : CommandBase
    {
        public override string Name => "follow";
        public override string Usage => "follow <user>";
        public override string Description => "Causes the bot to follow the specified twitter user.";

        public override void Execute(Discord discord, params string[] parameters)
        {
            if (parameters.Length != 1)
            {
                discord.SendInvalidCommand(this);
                return;
            }

            Twitter.Instance.Follow(parameters[0]);
            discord.SendMessage($"Now following {parameters[0]}.");
        }
    }
}

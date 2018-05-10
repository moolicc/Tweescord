using System;
using System.Collections.Generic;
using System.Text;

namespace Tweescord.Commands
{
    class UnfollowCommand : CommandBase
    {
        public override string Name => "unfollow";
        public override string Usage => "unfollow <user>";
        public override string Description => "Causes the bot to stop following the specified twitter user.";

        public override void Execute(Discord discord, params string[] parameters)
        {
            if (parameters.Length != 1)
            {
                discord.SendInvalidCommand(this);
                return;
            }

            Twitter.Instance.Unfollow(parameters[0]);
            discord.SendMessage($"No longer following {parameters[0]}.");
        }
    }
}

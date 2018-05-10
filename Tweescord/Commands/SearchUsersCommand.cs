using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DSharpPlus.Entities;
using Tweetinvi;

namespace Tweescord.Commands
{
    class SearchUsersCommand : CommandBase
    {
        public override string Name => "search";
        public override string Usage => "search <user>"; //" [page]";

        public override string Description => "Performs a twitter search for the specified user";
            //"Performs a twitter search for the specified user. Search results are returned in multiple pages,  " +
            //"use the optional 'page' argument to specify which page of results to see.";

        public override void Execute(Discord discord, params string[] parameters)
        {
            int page = 0;
            /*
            if (parameters.Length == 2)
            {
                if (!int.TryParse(parameters[1], out page))
                {
                    discord.SendInvalidCommand(this);
                    return;
                }
                page--;
            }
            */
            if (parameters.Length != 1)
            {
                discord.SendInvalidCommand(this);
                return;
            }

            var users = Search.SearchUsers(parameters[0], page: page);
            if (users == null || !users.Any())
            {
                discord.SendCommandMessage(this, "There are no results for that search.");
                return;
            }
            discord.SendMessage(BuildMessage(users, parameters[0], page));
        }

        private DiscordEmbed BuildMessage(IEnumerable<Tweetinvi.Models.IUser> users, string query, int page)
        {
            var builder = new DiscordEmbedBuilder();
            builder.Title = $"Results for \"{query}\" )";
            //builder.Description =
            //    $"There may be multiple pages of results. Use \"search {query} {page + 2}\" to see the next page.";

            foreach (var user in users)
            {
                var userBuilder = new StringBuilder();

                if (user.Verified)
                {
                    userBuilder.AppendLine("•  Verified account");
                }

                userBuilder.Append("•  ");
                userBuilder.AppendLine(user.Description);
                userBuilder.AppendLine($"•  Account created: {user.CreatedAt}");
                userBuilder.AppendLine($"•  Location: {user.Location}");
                userBuilder.AppendLine($"•  Followers: {user.FollowersCount}");

                if (user.Following)
                {
                    userBuilder.AppendLine($"•  NOTE: This bot is already following this twitter user.");
                }
                builder.AddField(user.Name, userBuilder.ToString());
            }

            return builder.Build();
        }
    }
}

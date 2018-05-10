using System;
using System.IO;
using Newtonsoft.Json;
using Tweetinvi.Models;

namespace Tweescord
{
    class Program
    {
        static void Main(string[] args)
        {
            if (!File.Exists("twitter.txt"))
            {
                Console.WriteLine("You must create the twitter.txt tokens file.");
                Console.ReadLine();
                return;
            }
            if (!File.Exists("discord.json"))
            {
                Console.WriteLine("You must create the discord.json configuration file.");

                string json = JsonConvert.SerializeObject(new DiscordConfig());
                File.WriteAllText("discord.json", json);

                Console.ReadLine();
                return;
            }

            var twitter = new Twitter();
            twitter.Authenticate("twitter.txt");

            var discord = new Discord();
            discord.Connect("discord.json");

            twitter.StartStreaming();


            while (Console.ReadLine() != "exit")
            {
                
            }

            File.WriteAllText("discord.json", JsonConvert.SerializeObject(discord.DiscordConfig));
        }
    }
}

using System;
using System.IO;
using Tweetinvi;
using Tweetinvi.Events;
using Tweetinvi.Models;
using Tweetinvi.Streaming;

namespace Tweescord
{
    class Twitter
    {
        public static Twitter Instance { get; private set; }

        private IUserStream _twitterStream;


        public Twitter()
        {
            Instance = this;
        }

        public void Authenticate(string tokensFile)
        {
            Console.WriteLine("Authenticating...");
            var tokens = LoadTokens(tokensFile);
            Auth.SetCredentials(new TwitterCredentials(tokens.consumerKey, tokens.consumerSecret, tokens.accessToken, tokens.accessSecret));
        }

        public void Follow(string user)
        {
            User.FollowUser(user);
        }

        public void Unfollow(string user)
        {
            User.UnFollowUser(user);
        }

        public void StartStreaming()
        {
            _twitterStream = Tweetinvi.Stream.CreateUserStream();
            _twitterStream.TweetCreatedByFriend += TweetCreated;
            _twitterStream.StartStreamAsync();
        }

        private void TweetCreated(object sender, TweetReceivedEventArgs tweetReceivedEventArgs)
        {
            Discord.Instance.SendMessage(tweetReceivedEventArgs.Tweet.Url);
        }

        public void StopStreaming()
        {
            _twitterStream.StopStream();
        }

        private (string accessToken, string accessSecret, string consumerKey, string consumerSecret) LoadTokens(string file)
        {
            string[] lines = File.ReadAllLines(file);
            return (lines[0], lines[1], lines[2], lines[3]);
        }
    }
}

# Tweescord
Twitter -> Discord reposter
This bot reposts tweets into discord.

# Usage
## 1:
You'll need two files in the working directory of the bot
  * twitter.txt
  * discord.json

twitter.txt:
```
accesstoken
accesssecret
consumerkey
consumersecret
```

discord.json:
```json
{
  "Token" : "discordtoken",
  "Channel" : 0
}
```
You can set the channel value to the channel ID of the discord channel you want the bot to use, or
you can let the bot automatically resolve the ID. He'll just remember the ID of the channel he
receives his first command in.

## 2:
Once you've started the bot, go to whatever channel you want the bot to use and hit him with an `@botname help`.
This will cause him to remember the channel. Now type `exit` in the bot's terminal. He'll save the channel ID into the json
when he exits.

## 4:
The help command should be enough to get you going as far as actual discord usage goes.

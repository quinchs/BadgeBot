using Discord;
using Discord.Interactions;
using Discord.Rest;

namespace BadgeBot.Commands;


public class BadgeCommandModule : StepModuleBase
{
    public override int StepNumber => 0;

    [SlashCommand("badge", "Start the process of getting the badge")]
    public async Task ExecuteAsync()
    {
        // respond with the confirmation embed
        var embed = new EmbedBuilder()
            .WithTitle("Before you proceed")
            .WithDescription(
                "Internet privacy and security is imporant! This process requires you to " +
                "input your application id, client secret, and public key\n" +
                "The client secret is used to create a slash command in this server, it allows applications " +
                "like this one to authorize with the Discord API to perform actions as your bot.\nThe public key " +
                "is used to verify the data sent from discord. " +
                "You can read more about what a public key is [here](https://en.wikipedia.org/wiki/Public-key_cryptography).\n\n" +
                "Here are some security rules before we continue:")
            .AddField("Basic security practices",
                "If someone is offering you a badge and they want you to run a program on your " +
                "computer **IT IS A SCAM**! Never run random programs on your computer unless you know " +
                "the source!")
            .AddField("Credentials",
                "**DO NOT** share you user or bot tokens/credentials with anyone! This application " +
                "is stateless and does not store any data sent to it. You can verify this " +
                "by [viewing the source code](https://github.com/discord-net/Discord.Net).\n\n" +
                "**IT IS STRONGLY RECOMMENDED** to reset the credentials and or delete the application you used " +
                "after the badge process has been completed.")
            .AddField("Scope",
                "**DO NOT** use a bot that is already being used for something! For this process " +
                "please make sure you **create a new** application that is in zero servers.\nAdditionally," +
                "do not add said bot to a public server or a server that is not meant for testing/development.")
            .AddField("Legality",
                "**BY CLICKING 'AGREE'** you acknowledge that any information you submit has the posibillity of being " +
                "accessed by unwanted/third parties. I've opensourced the code of this bot which can give attackers " +
                "information about how to exploit it, this application does not store any credentials sent to it " +
                "but that does not stop exploiters from hijacking this bot. You hereby agree that you are responsible " +
                "for any misuse caused by the application whos credentials you submit to this one.")
            .WithColor(Color.Orange);

        var components = new ComponentBuilder()
            .WithButton("Agree", "badge-accept", ButtonStyle.Success, Emoji.Parse("✅"));

        await RespondAsync(embed: embed.Build(), components: components.Build(), ephemeral: true);
    }
}
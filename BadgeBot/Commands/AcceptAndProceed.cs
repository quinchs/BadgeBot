using System;
using Discord;
using Discord.Interactions;
using Discord.Rest;

namespace BadgeBot.Commands
{
	public class AcceptAndProceed : RestInteractionModuleBase<RestInteractionContext>
	{
		[ComponentInteraction("badge-accept")]
		public async Task ExecuteAsync()
		{
			var embed = new EmbedBuilder()
				.WithTitle("Create a bot")
				.AddField("Step 1",
					"Head over to the [developer portal](https://discord.com/developers/applications) " +
					"and click \"New Application\" in the top-right corner. Fill out the forum and click \"Create\"")
				.AddField("Step 2",
					"Click on the newly created apllication and copy the \"Client Id\" and \"Public Key\", " +
					"then go to the OAuth2 page on the left and click \"Reset Secret\" and then copy the value.")
				.AddField("Step 3",
					"Below this message, click the \"Submit\" button and enter in the " +
					"\"Client Id\", \"Public key\", and \"Client secret\" into the popup.")
				.WithImageUrl("https://cdn.discordapp.com/attachments/1045724368371724341/1046365646184779817/final1.png")
				.WithColor(Color.Green);

			var components = new ComponentBuilder()
				.WithButton("Submit", "application-submit", ButtonStyle.Primary);

			await RespondAsync(embed: embed.Build(), components: components.Build(), ephemeral: true);
		}
	}
}


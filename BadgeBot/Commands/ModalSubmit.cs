using System;
using System.Text.RegularExpressions;
using Discord;
using Discord.Interactions;
using Discord.Rest;
using Microsoft.AspNetCore.DataProtection.KeyManagement;

namespace BadgeBot.Commands
{
	public partial class ModalSubmit : RestInteractionModuleBase<RestInteractionContext>
	{
		public const string BASE_ENDPOINT_URL = "https://bbot.quinch.dev";

		[ModalInteraction("application_modal")]
		public async Task ExecuteAsync(ApplicationModal modal)
		{
			if(!ulong.TryParse(modal.ApplicationId, out var appId))
			{
				var embed = new EmbedBuilder()
					.WithTitle("Invalid Application Id")
					.WithDescription("The application id was incorrect! Make sure the id is *all numbers* and *not negative*")
					.WithColor(Color.Red);

				await RespondAsync(embed: embed.Build(), ephemeral: true);
				return;
			}

			var appKey = modal.ApplicationKey!.Trim();

			if(!PublicKeyRegex().IsMatch(appKey))
			{
                var embed = new EmbedBuilder()
                    .WithTitle("Invalid Application Key")
                    .WithDescription("The application id was incorrect! Make sure that you copied the right one, The name of the field is \"Public Key\"")
                    .WithColor(Color.Red);

                await RespondAsync(embed: embed.Build(), ephemeral: true);
                return;
            }

			var url = $"discord://discord.com/oauth2/authorize?client_id={appId}&scope=applications.commands";

			var finalStep = new EmbedBuilder()
				.WithTitle("Final steps")
				.AddField("Step 4",
					$"On the [developer portal](https://discord.com/developers/applications/{appId}/information), put " +
					$"the url `{BASE_ENDPOINT_URL}/interactions/badges/{appKey}/callback` in the \"Interaction Endpoint URL\" box " +
					$"and click \"Save Changes\"")
				.AddField("Step 5",
					$"Add the bot to *this* server by clicking this link <{url}> or by generating an invite " +
					$"url with the `applications.commands` scope. After the bot as been authorized, click \"Finish\"")
				.AddField("Note", "If you added the bot to a different server, this will not work!")
				.WithImageUrl("https://media.discordapp.net/attachments/1045724368371724341/1046365764246044692/6.png")
				.WithColor(Color.Green);

			var components = new ComponentBuilder()
				.WithButton("Finish", $"finish-badge-{modal.ApplicationId},{modal.OAuth2Secret}");

			await RespondAsync(embed: finalStep.Build(), components: components.Build(), ephemeral: true);
        }

        [GeneratedRegex("^[a-f\\d]{64}$")]
        public static partial Regex PublicKeyRegex();
    }
}


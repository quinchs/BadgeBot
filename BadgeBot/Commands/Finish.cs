using System;
using Discord;
using Discord.Interactions;
using Discord.Rest;

namespace BadgeBot.Commands
{
	public class Finish : StepModuleBase
	{
		public override int StepNumber => 4;

        [ComponentInteraction("finish-badge-*,*")]
		public async Task ExecuteAsync(string rawId, string secret)
		{
			if(!ulong.TryParse(rawId, out var appId))
			{
				await RespondAsync("Seems like the command failed. this is a bug with your application id not being a number...");
				return;
			}


			// try to register the command to the guild
			await DeferAsync(true);

			var token = await DiscordUtils.CreateBearerTokenAsync(rawId, secret);

			var id = await DiscordUtils.CreateDefaultSlashCommandAsync(appId, Context.Interaction.GuildId!.Value, token);

			var embed = new EmbedBuilder()
				.WithTitle("Success!")
				.WithDescription($"You can now run </claim-badge:{id}> to claim your badge!")
				.AddField("IMPORTANT",
					"It's strongly suggested to now reset your OAuth secret by clicking the \"Reset\" button on the" +
                    $"[OAuth2](https://discord.com/developers/applications/{rawId}/oauth2/general) page. You can also delete the " +
					$"application after you run </claim-badge:{id}> command.")
				.WithColor(Color.Green);

			await FollowupAsync(embed: embed.Build(), ephemeral: true);
		}
	}
}


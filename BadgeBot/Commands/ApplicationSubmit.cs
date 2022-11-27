using System;
using Discord.Interactions;
using Discord.Rest;

namespace BadgeBot.Commands
{
	public class ApplicationSubmit : RestInteractionModuleBase<RestInteractionContext>
	{
		[ComponentInteraction("application-submit")]
		public async Task ExecuteAsync()
		{
			await RespondWithModalAsync<ApplicationModal>("application_modal");
		}
	}
}


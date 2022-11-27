using System;
using Discord.Interactions;
using Discord.Rest;

namespace BadgeBot.Commands
{
	public class ApplicationSubmit : StepModuleBase
    {
		public override int StepNumber => 2;

        [ComponentInteraction("application-submit")]
		public async Task ExecuteAsync()
		{
			await RespondWithModalAsync<ApplicationModal>("application_modal");
		}
	}
}


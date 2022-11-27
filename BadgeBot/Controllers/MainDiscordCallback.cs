using System;
using Discord.Interactions;
using Discord.Rest;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BadgeBot.Controllers
{
	[ApiController]
	[Route("/interactions/callback")]
	public class MainDiscordCallback : ControllerBase
	{
		private readonly DiscordRestClient _client;
		private readonly InteractionService _interactionService;
		private readonly IServiceProvider _provider;
		private readonly ILogger _logger;
		private readonly string _publicKey;

		public MainDiscordCallback(ILogger<MainDiscordCallback> logger, DiscordRestClient client, InteractionService service, IServiceProvider provider)
		{
			_logger = logger;
            _provider = provider;
			_interactionService = service;
			_client = client;
			_publicKey = Environment.GetEnvironmentVariable("BOT_PUBLIC_KEY")!;
		}

		[HttpPost]
		public async Task<ActionResult> HandleCallbackAsync()
		{
			if(
				!Request.Headers.TryGetValue("X-Signature-Ed25519", out var sig) ||
				!Request.Headers.TryGetValue("X-Signature-Timestamp", out var timestamp))
			{
				_logger.LogInformation("[400]: no signature/timestamp");
                return BadRequest();
            }

            using var sr = new StreamReader(Request.Body);
            var body = await sr.ReadToEndAsync();

			try
			{
				var interaction = await _client.ParseHttpInteractionAsync(_publicKey, sig, timestamp, body).ConfigureAwait(false);

				if(interaction is RestPingInteraction ping)
				{
					_logger.LogInformation("[200]: ACK pink");
					return Content(ping.AcknowledgePing(), "application/json");
				}

				ActionResult? result = null;
				TaskCompletionSource tcs = new TaskCompletionSource();

				var context = new RestInteractionContext(_client, interaction, (content) =>
                {
					_logger.LogInformation("[200]: Interaction Response");
                    result = Content(content, "application/json");
                    tcs.TrySetResult();
                    return Task.CompletedTask;
                });

				var executeResult = await _interactionService.ExecuteCommandAsync(context, _provider).ConfigureAwait(false);

				await Task.WhenAny(tcs.Task, Task.Delay(5000));

				// TODO: logging

				if (result is not null)
					return result;

				_logger.LogInformation("[500]: Timed out without response");
				return StatusCode(500);
			}
			catch (BadSignatureException)
			{
				_logger.LogInformation("[401]: Bad signature");
				return Unauthorized();
			}
        }
	}
}


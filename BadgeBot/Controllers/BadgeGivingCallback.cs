using System;
using System.Text.RegularExpressions;
using BadgeBot.DB;
using Discord;
using Discord.Interactions;
using Discord.Net;
using Discord.Rest;
using EdgeDB;
using Microsoft.AspNetCore.Mvc;

namespace BadgeBot.Controllers
{
    [ApiController]
    [Route("/interactions/badges/{key}/callback")]
    public partial class BadgeGivingCallback : ControllerBase
	{
        private readonly DiscordRestClient _client;
        private readonly EdgeDBClient _edgedb;

        public BadgeGivingCallback(EdgeDBClient edgedb, DiscordRestClient client)
        {
            _client = client;
            _edgedb = edgedb;
        }

        [HttpPost]
        public async Task<ActionResult> HandleCallbackAsync(string key)
        {
            if (!Regex.IsMatch(key, @"^[a-f\d]{64}$"))
                return BadRequest();

            if (
                !Request.Headers.TryGetValue("X-Signature-Ed25519", out var sig) ||
                !Request.Headers.TryGetValue("X-Signature-Timestamp", out var timestamp))
            {
                return BadRequest();
            }

            using var sr = new StreamReader(Request.Body);
            var body = await sr.ReadToEndAsync();

            try
            {
                var interaction = await _client.ParseHttpInteractionAsync(key, sig, timestamp, body).ConfigureAwait(false);

                if (interaction is RestPingInteraction ping)
                {
                    return Content(ping.AcknowledgePing(), "application/json");
                }

                if (interaction is not RestSlashCommand slashCommand || slashCommand.Data.Name != "claim-badge")
                    return BadRequest();

                await _edgedb.UpdateUsersStepsAsync(interaction.User.Id.ToString(), 5);

                return Content(
                    slashCommand.Respond(embed: new EmbedBuilder()
                        .WithTitle(":tada: Congratulations :tada:")
                        .WithDescription(
                            "You've just made yourself elegible for the Active Developer Badge! " +
                            "To claim it, follow this link https://discord.com/developers/active-developer. " +
                            "Make sure to reset your OAuth2 secret!")
                        .AddField("Note",
                            "It may take a while after claiming the badge to show up on your profile.")
                        .WithColor(Color.Green)
                        .Build()), "application/json");
            }
            catch (BadSignatureException)
            {
                return Unauthorized();
            }
        }
    }
}


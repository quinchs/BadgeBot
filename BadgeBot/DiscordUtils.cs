using System;
using System.Text;
using Discord;
using Discord.Rest;
using Newtonsoft.Json;

namespace BadgeBot
{
	public static class DiscordUtils
	{
		public const string OAUTH2_TOKEN_URL = "https://discord.com/api/v10/oauth2/token";

		private class OAuthResult
		{
			[JsonProperty("access_token")]
			public string? AccessToken { get; set; }
		}

		private class CommandCreateResult
		{
			[JsonProperty("id")]
			public string? Id { get; set; }
		}

        public static async Task<string> CreateBearerTokenAsync(string id, string secret)
		{
			using var client = new HttpClient();

			var urlParams = new Dictionary<string, string>()
			{
				{ "grant_type", "client_credentials" },
				{ "scope", "applications.commands applications.commands.update" }
			};

			var content = new FormUrlEncodedContent(urlParams);

			client.DefaultRequestHeaders.Authorization
				= new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes($"{id}:{secret}")));

			var result = await client.PostAsync(OAUTH2_TOKEN_URL, content);

			result.EnsureSuccessStatusCode();

			var json = await result.Content.ReadAsStringAsync();

			return JsonConvert.DeserializeObject<OAuthResult>(json)!.AccessToken!;
		}

		public static async Task<string> CreateDefaultSlashCommandAsync(ulong appId, ulong guildId, string token)
		{
            using var client = new HttpClient();

			var command = @"{ ""name"": ""claim-badge"", ""type"": 1, ""description"": ""Claim the Active Developer Badge"" }";

			var content = new StringContent(command, Encoding.UTF8, "application/json");

			client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

			var result = await client.PostAsync($"https://discord.com/api/v10/applications/{appId}/guilds/{guildId}/commands", content);

			result.EnsureSuccessStatusCode();

			return JsonConvert.DeserializeObject<CommandCreateResult>(await result.Content.ReadAsStringAsync())!.Id!;
		}
	}
}


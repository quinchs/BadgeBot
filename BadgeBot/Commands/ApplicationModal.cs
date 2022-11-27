using System;
using Discord.Interactions;

namespace BadgeBot.Commands
{
    public class ApplicationModal : IModal
    {
        public string Title => "Submit Application";

        [InputLabel("Application Id")]
        [ModalTextInput("app_id", Discord.TextInputStyle.Short, "123456...")]
        public string? ApplicationId { get; set; }

        [InputLabel("Public Key")]
        [ModalTextInput("app_key", Discord.TextInputStyle.Short, "a1b2c3d4e5f6...")]
        public string? ApplicationKey { get; set; }

        [InputLabel("OAuth2 Secret")]
        [ModalTextInput("app_secret", Discord.TextInputStyle.Short, "aof2jSkfm...")]
        public string? OAuth2Secret { get; set; }
    }
}


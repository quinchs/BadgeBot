# BadgeBot

A bot written to easily help you become eligible for the 
[Active Developer Badge](https://support-dev.discord.com/hc/en-us/articles/10113997751447-Active-Developer-Badge).

### Goal
The goal with this small project was to create a way to help others get the badge without the need to 
install/run/visit anything since people are being exploited by scammers promising the badge.
 
This bot uses a stateless approach when dealing with credentials to minimize potential hacks/leaks of data.
 
### Data notice
 
Your user id and progress is the only thing stored by this bot, using [EdgeDB](https://edgedb.com). Please 
shoot me a message on Discord if you want your data removed.

## Invite

**PLEASE DO NOT INVITE TO PUBLIC SERVERS**

To avoid any potential damages related to this bot, only add it to a test/temp/dev server. 

Invite the bot [here](https://discord.com/api/oauth2/authorize?client_id=1045723581654507590&scope=applications.commands)

# How to use
- Add the bot to a test server
- Run `/badge`
- Follow the prompts.

## How it works
You give the bot an application id, public key, and OAuth2 secret. It then creates a slash command on the application, and listens for that command to be used, its that simple. Shoutout to toonlink#0666 for the inspiration on the logic, they made https://discordactivedev.vercel.app/! 

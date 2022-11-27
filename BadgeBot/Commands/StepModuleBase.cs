using BadgeBot.DB;
using Discord.Interactions;
using Discord.Rest;
using EdgeDB;
using Microsoft.Extensions.Caching.Memory;

namespace BadgeBot.Commands
{
    public abstract class StepModuleBase : RestInteractionModuleBase<RestInteractionContext>
    {
        public abstract int StepNumber { get; }        
        public EdgeDBClient? EdgeDB { get; set; }
        
        public override async Task AfterExecuteAsync(ICommandInfo command)
        {
            await EdgeDB!.UpdateUsersStepsAsync(Context.Interaction.User.Id.ToString(), StepNumber);
        }
    }
}

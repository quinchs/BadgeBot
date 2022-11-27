using Discord.Rest;
using Discord.Interactions;
using System.Reflection;
using EdgeDB;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Logging.AddConsole();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var edgedbConfig = (IServiceProvider s) => new EdgeDBClientPoolConfig()
{
    Logger = s.GetRequiredService<ILoggerFactory>().CreateLogger<EdgeDBClient>(),
    SchemaNamingStrategy = INamingStrategy.SnakeCaseNamingStrategy
};

#if DEBUG

builder.Services.AddSingleton((s) => new EdgeDBClient(edgedbConfig(s)));

#else

builder.Services.AddSingleton((s) => new EdgeDBClient(new EdgeDBConnection
{
    Database = "BadgeBot",
    Hostname = "edgedb",
    Password = Environment.GetEnvironmentVariable("EDGEDB_PASSWORD"),
    Port = 5656,
    TLSSecurity = TLSSecurityMode.Insecure,
    Username = "edgedb"
}, edgedbConfig(s)));

#endif

var discordClient = new DiscordRestClient(new DiscordRestConfig
{
     APIOnRestInteractionCreation = false,
     UseInteractionSnowflakeDate = false,
});

await discordClient.LoginAsync(Discord.TokenType.Bot, Environment.GetEnvironmentVariable("BOT_TOKEN"));

builder.Services.AddSingleton(discordClient);

var interactionService = new InteractionService(discordClient, new InteractionServiceConfig
{
    UseCompiledLambda = true,
    LogLevel = Discord.LogSeverity.Debug,
});

interactionService.Log += (m) =>
{
    Console.WriteLine($"[{m.Severity}] {m.Source}: {m.Message} {m.Exception}");
    return Task.CompletedTask;
};


builder.Services.AddSingleton(interactionService);

var app = builder.Build();

await interactionService.AddModulesAsync(Assembly.GetExecutingAssembly(), app.Services);

await interactionService.RegisterCommandsToGuildAsync(1045724367218286594);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


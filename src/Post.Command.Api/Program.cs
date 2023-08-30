using CQRS.Core.Domain;
using CQRS.Core.Handlers;
using CQRS.Core.Infra;
using Post.Command.Api.Commands;
using Post.Command.Domain.Aggregates;
using Post.Command.Infra.Config;
using Post.Command.Infra.Dispatchers;
using Post.Command.Infra.Handlers;
using Post.Command.Infra.Repositories;
using Post.Command.Infra.Stores;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<MongoDbConfig>(builder.Configuration.GetSection(nameof(MongoDbConfig)));
builder.Services.AddScoped<IEventStoreRepository, EventStoreRepository>();
builder.Services.AddScoped<IEventStore, EventStore>();
builder.Services.AddScoped<IEventSourcingHandler<PostAggregate>, EventSourcingHandler>();
builder.Services.AddScoped<ICommandHandler, CommandHandler>();

var commandHandler = builder.Services.BuildServiceProvider().GetRequiredService<ICommandHandler>();
var dispatcher = new CommandDispatcher();
dispatcher.RegisterHandler<CreatePost>(commandHandler.HandleAsync);
dispatcher.RegisterHandler<UpdatePost>(commandHandler.HandleAsync);
dispatcher.RegisterHandler<DeletePost>(commandHandler.HandleAsync);
dispatcher.RegisterHandler<LikePost>(commandHandler.HandleAsync);
dispatcher.RegisterHandler<CommentPost>(commandHandler.HandleAsync);
dispatcher.RegisterHandler<UpdateComment>(commandHandler.HandleAsync);
dispatcher.RegisterHandler<DeleteComment>(commandHandler.HandleAsync);
builder.Services.AddSingleton<ICommandDispatcher>(_ => dispatcher);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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

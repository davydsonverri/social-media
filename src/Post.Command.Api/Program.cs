using Confluent.Kafka;
using CQRS.Core.Domain;
using CQRS.Core.Handlers;
using CQRS.Core.Infra;
using CQRS.Core.Messages;
using CQRS.Core.Producers;
using Domain.Identity;
using Domain.Identity.ULID;
using Microsoft.OpenApi.Models;
using MongoDB.Bson.Serialization;
using Post.Command.Api.Commands;
using Post.Command.Domain.Aggregates;
using Post.Command.Infra.Config;
using Post.Command.Infra.Dispatchers;
using Post.Command.Infra.Handlers;
using Post.Command.Infra.Producers;
using Post.Command.Infra.Repositories;
using Post.Command.Infra.Stores;
using Post.Common.Events;

var builder = WebApplication.CreateBuilder(args);

BsonClassMap.RegisterClassMap<BaseEvent>();
BsonClassMap.RegisterClassMap<PostCreated>();
BsonClassMap.RegisterClassMap<PostUpdated>();
BsonClassMap.RegisterClassMap<PostDeleted>();
BsonClassMap.RegisterClassMap<PostLiked>();
BsonClassMap.RegisterClassMap<CommentAdded>();
BsonClassMap.RegisterClassMap<CommentUpdated>();
BsonClassMap.RegisterClassMap<CommentDeleted>();
BsonSerializer.RegisterSerializer<Did>(new DidMongoSerializer());


// Add services to the container.
builder.Services.Configure<MongoDbConfig>(builder.Configuration.GetSection(nameof(MongoDbConfig)));
builder.Services.Configure<ProducerConfig>(builder.Configuration.GetSection(nameof(ProducerConfig)));
builder.Services.AddScoped<IEventStoreRepository, EventStoreRepository>();
builder.Services.AddScoped<IEventProducer, EventProducer>();
builder.Services.AddScoped<IEventStore, EventStore>();
builder.Services.AddScoped<IEventSourcingHandler<PostAggregate>, EventSourcingHandler<PostAggregate>>();
builder.Services.AddScoped<ICommandHandler, CommandHandler>();
builder.Services.AddSingleton<IDomainIdentity, UlidGenerator>();
builder.Services.AddSingleton<ICommandDispatcher>(sp =>
{
    var commandHandler = sp.CreateScope().ServiceProvider.GetRequiredService<ICommandHandler>();
    var dispatcher = new CommandDispatcher();
    dispatcher.RegisterHandler<CreatePost>(commandHandler.HandleAsync);
    dispatcher.RegisterHandler<UpdatePost>(commandHandler.HandleAsync);
    dispatcher.RegisterHandler<DeletePost>(commandHandler.HandleAsync);
    dispatcher.RegisterHandler<LikePost>(commandHandler.HandleAsync);
    dispatcher.RegisterHandler<CommentPost>(commandHandler.HandleAsync);
    dispatcher.RegisterHandler<UpdateComment>(commandHandler.HandleAsync);
    dispatcher.RegisterHandler<DeleteComment>(commandHandler.HandleAsync);
    dispatcher.RegisterHandler<RestoreReadDb>(commandHandler.HandleAsync);
    return dispatcher;
} );

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
    c.MapType<Did>(() => new OpenApiSchema()
    {
        Type = "string"
    });
});

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
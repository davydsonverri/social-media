using Confluent.Kafka;
using CQRS.Core.Consumers;
using CQRS.Core.Infra;
using Microsoft.EntityFrameworkCore;
using Post.Query.Api.Queries;
using Post.Query.Domain.Entities;
using Post.Query.Domain.Repositories;
using Post.Query.Infra.ConsumerHostedService;
using Post.Query.Infra.Consumers;
using Post.Query.Infra.DataAccess;
using Post.Query.Infra.Dispatchers;
using Post.Query.Infra.Handlers;
using Post.Query.Infra.Repositories;
using EventHandler = Post.Query.Infra.Handlers.EventHandler;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var con = builder.Configuration.GetConnectionString("SqlServer");
Action<DbContextOptionsBuilder> configureDbContext = (o => o.UseLazyLoadingProxies().UseSqlServer(con));

builder.Services.AddDbContext<DatabaseContext>(configureDbContext);
builder.Services.AddSingleton(new DatabaseContextFactory(configureDbContext));

builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<IEventHandler, EventHandler>();
builder.Services.Configure<ConsumerConfig>(builder.Configuration.GetSection(nameof(ConsumerConfig)));
builder.Services.AddScoped<IEventConsumer, EventConsumer>();
builder.Services.AddScoped<IQueryHandler, QueryHandler>();

builder.Services.AddSingleton<IQueryDispatcher<PostEntity>>(sp =>
{
    var queryHandler = sp.CreateScope().ServiceProvider.GetRequiredService<IQueryHandler>();
    var dispatcher = new QueryDispatcher();
    dispatcher.RegisterHandler<ListAllPostsQuery>(queryHandler.HandleAsync);
    dispatcher.RegisterHandler<FindPostByAuthorQuery>(queryHandler.HandleAsync);
    dispatcher.RegisterHandler<FindPostByIdQuery>(queryHandler.HandleAsync);
    return dispatcher;
});

builder.Services.AddControllers();
builder.Services.AddHostedService<ConsumerHostedService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
});

var app = builder.Build();

var dbContextFactory = app.Services.GetService<DatabaseContextFactory>()!;
dbContextFactory.CreateDbContext().Database.EnsureCreated();

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

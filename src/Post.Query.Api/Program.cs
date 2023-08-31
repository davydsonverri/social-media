using Microsoft.EntityFrameworkCore;
using Post.Query.Infra.DataAccess;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var con = builder.Configuration.GetConnectionString("SqlServer");
Action<DbContextOptionsBuilder> configureDbContext = (o => o.UseLazyLoadingProxies().UseSqlServer(con));

//var context = new DatabaseContext()
builder.Services.AddDbContext<DatabaseContext>(configureDbContext);
builder.Services.AddSingleton<DatabaseContextFactory>(new DatabaseContextFactory(configureDbContext));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using Posterr.Application;
using Posterr.Repositories;
using Posterr.Settings;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
BsonSerializer.RegisterSerializer(new DateTimeSerializer(BsonType.String));
builder.Services.AddSingleton<IMongoClient>(serviceProvider =>
{
    var setting = builder.Configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();
    return new MongoClient(setting.ConnectionString);
});

// Repositorys interface
builder.Services.AddSingleton<IPostsRepository, MongoDbPostsRepository>();
builder.Services.AddSingleton<IUsersRepository, MongoDbUsersRepository>();
builder.Services.AddSingleton<ICountersRepository, MongoDbCountersRepository>();
// Applications interface
builder.Services.AddSingleton<IPostsApplication, PostsApplication>();
builder.Services.AddSingleton<IUsersApplication, UsersApplication>();

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

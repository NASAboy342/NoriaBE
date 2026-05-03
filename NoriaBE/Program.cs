using NoriaBE.Filters;
using NoriaBE.Repositories;
using NoriaBE.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<ExceptionFilter>();
builder.Services.AddSingleton<IBuildingService, BuildingService>();
builder.Services.AddSingleton<IRoomService, RoomService>();
builder.Services.AddSingleton<INoriaRepository, NoriaRepository>();


var app = builder.Build();

// Configure the HTTP request pipeline.
// (app.Environment.IsDevelopment())

app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

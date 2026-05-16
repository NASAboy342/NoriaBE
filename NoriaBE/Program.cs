using NLog;
using NLog.Web;
using NoriaBE.Filters;
using NoriaBE.Repositories;
using NoriaBE.Services;

var logger = LogManager.Setup()
    .LoadConfigurationFromFile("nlog.config")
    .GetCurrentClassLogger();

try
{

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Host.UseNLog();

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.Filters.AddService<LogFilter>();
}).AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGenNewtonsoftSupport();
builder.Services.AddSingleton<ExceptionFilter>();
builder.Services.AddSingleton<LogFilter>();
builder.Services.AddSingleton<IBuildingService, BuildingService>();
builder.Services.AddSingleton<IRoomService, RoomService>();
builder.Services.AddSingleton<INoriaRepository, NoriaRepository>();
builder.Services.AddSingleton<IPaymentService, PaymentService>();
builder.Services.AddSingleton<ILoggerService, LoggerService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
// (app.Environment.IsDevelopment())

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("AllowLocalhost");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

}
catch (Exception ex)
{
    logger.Error(ex, "Application stopped due to an unhandled exception.");
    throw;
}
finally
{
    LogManager.Shutdown();
}

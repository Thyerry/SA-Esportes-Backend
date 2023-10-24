using Domain.Contracts;
using Domain.Service;
using WebApi.Hubs;
using WebApi.RabbitMQ;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSignalR();
builder.Services.AddSingleton<RabbitMqConsumer>();
builder.Services.AddSingleton<IConsumerService, ConsumerService>();
builder.Services.AddScoped<IRabbitMQService, RabbitMQService>();
builder.Services.AddScoped<IMessageService, MessageService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder
            .WithOrigins("http://localhost:4200")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});

var app = builder.Build();

app.UseCors("AllowSpecificOrigin");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var consumer = app.Services.GetRequiredService<RabbitMqConsumer>();
consumer.StartListening();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapHub<MessageHub>("/hubs/messages");

app.Run();
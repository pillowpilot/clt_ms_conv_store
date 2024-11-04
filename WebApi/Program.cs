//using WebApi.Integration.Events.Consumers.MessageReceived;

using MassTransit;
using WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddApi(configuration)
                .AddMongoDB(configuration)
                .AddRabbitMQ(configuration);

var app = builder.Build();

app.UseHttpsRedirection();

//Endpoints
app.MapGet("/health", () => Results.Ok("Ok and running"));

//Endpoints para publicar los eventos, OBS: Solo a modo de poder simular el disparo de eventos de los otros microservicios
app.MapPost($"/{nameof(WABATextMsgWithUserInfo)}", async (WABATextMsgWithUserInfo request, ISendEndpointProvider sendEndpointProvider, MongoDBService mongo) =>
{
    await sendEndpointProvider.Send(nameof(WABATextMsgWithUserInfo), request);
    return Results.Ok("Evento publicado");
});

app.Run();
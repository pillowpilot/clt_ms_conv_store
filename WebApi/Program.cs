using ConvCrmContracts.Conv.Querys;
using MassTransit;
using MassTransit.Clients;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using WebApi.Extensions;
using WebApi.Integration.Events.Consumers;

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


app.MapPost($"/get-conversation", async ([FromBody] GetConversation request, IBus _bus) =>
{
    var message = await _bus.GetResponse<GetConversation, ConversationDocument>(nameof(GetConversation), request);
    return message;
});

app.Run();
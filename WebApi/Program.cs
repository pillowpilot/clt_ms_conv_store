using WebApi.Integration.Events.Consumers.MessageReceived;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

builder.Services.AddApi(configuration)
                .AddMongoDB(configuration)
                .AddRabbitMQ(configuration);

var app = builder.Build();

app.UseHttpsRedirection();

//Endpoints
app.MapGet("/health", () => Results.Ok("Ok and running"));

//Endpoints para publicar los eventos, OBS: Solo a modo de poder simular el disparo de eventos de los otros microservicios
app.MapPost($"/{nameof(MessageReceivedEvent)}", async (MessageReceivedEvent request, IPublishEndpoint endpoint, MongoDBService mongo) => 
{
    await endpoint.Publish(request);
    return Results.Ok("Evento publicado");
});

app.Run();
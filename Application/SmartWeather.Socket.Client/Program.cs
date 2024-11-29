using Microsoft.AspNetCore.SignalR.Client;
using System.Data.Common;

DateTime lastMsg = DateTime.Now;

var connection = new HubConnectionBuilder()
            .WithUrl("http://localhost:8093/MeasurePointHub")
            .Build();

connection.On<MeasurePointDataDto>("receivedMeasurePointData", (message) =>
{
    DateTime newMsg = DateTime.Now;
    Console.WriteLine($"Message reçu: {message} en {lastMsg.Subtract(newMsg).TotalMilliseconds}ms");
    lastMsg = newMsg;
});

await connection.StartAsync();

Console.WriteLine("Connecté au Hub");

await connection.InvokeAsync("InitiateStream", new
{
    Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiIxIiwiZW1haWwiOiJjaGFwdWlzODYzQGdtYWlsLmNvbSIsInJvbGUiOiJVc2VyIiwibmJmIjoxNzMyODg5OTYyLCJleHAiOjE3MzI5MDA3NjIsImlhdCI6MTczMjg4OTk2MiwiaXNzIjoiU21hcnRXZWF0aGVyIiwiYXVkIjoiU21hcnRXZWF0aGVyIn0.pA5pXV8sumEY0KTRYtXuaR9L0HymD1iBJv1yl8zBhZA",
    TargetId = 2
});

Console.WriteLine("Méthode InitiateStream invoquée");

Console.WriteLine("Appuyez sur une touche pour arrêter...");
Console.ReadKey();
record MeasurePointDataDto
{
    public int Id { get; set; }
    public float Value { get; set; }
}
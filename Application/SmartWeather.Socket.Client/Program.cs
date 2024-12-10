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

await connection.InvokeAsync("InitiateMonitoring", new
{
    Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiIxIiwiZW1haWwiOiJhZG1pbkBzbWFydHdlYXRoZXIubmV0Iiwicm9sZSI6IkFkbWluIiwibmJmIjoxNzMzNTAxNzQ5LCJleHAiOjE3MzM1MTI1NDksImlhdCI6MTczMzUwMTc0OSwiaXNzIjoiU21hcnRXZWF0aGVyIiwiYXVkIjoiU21hcnRXZWF0aGVyIn0.Brs27JdKXR7URmhw5kaIxyF8rynmdwGTG_Ya9sLo808"
});

Console.WriteLine("Méthode InitiateStream invoquée");

Console.WriteLine("Appuyez sur une touche pour arrêter...");
Console.ReadKey();
record MeasurePointDataDto
{
    public int Id { get; set; }
    public float Value { get; set; }
}
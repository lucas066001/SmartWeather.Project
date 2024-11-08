using Microsoft.AspNetCore.SignalR.Client;
using System.Data.Common;

var connection = new HubConnectionBuilder()
            .WithUrl("http://localhost:8093/MeasurePointHub")
            .Build();

connection.On<MeasurePointDataDto>("receivedMeasurePointData", (message) =>
{
    Console.WriteLine($"Message reçu: {message}");
});

await connection.StartAsync();

Console.WriteLine("Connecté au Hub");

await connection.InvokeAsync("InitiateStream", new
{
    Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiIxIiwiZW1haWwiOiJjaGFwdWlzODYzQGdtYWlsLmNvbSIsInJvbGUiOiJVc2VyIiwibmJmIjoxNzMxMDYxMjM4LCJleHAiOjE3MzEwNzIwMzgsImlhdCI6MTczMTA2MTIzOCwiaXNzIjoiU21hcnRXZWF0aGVyIiwiYXVkIjoiU21hcnRXZWF0aGVyIn0.y1wrjtZz52KzRa8fsRNodplo24L11pP1CKvQww8VGKg",
    TargetId = 5
});

Console.WriteLine("Méthode InitiateStream invoquée");

Console.WriteLine("Appuyez sur une touche pour arrêter...");
Console.ReadKey();
record MeasurePointDataDto
{
    public int Id { get; set; }
    public float Value { get; set; }
}
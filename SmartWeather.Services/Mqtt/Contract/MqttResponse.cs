using System.Reflection.PortableExecutable;
using System.Runtime.InteropServices.JavaScript;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace SmartWeather.Services.Mqtt.Contract;

public class MqttResponse
{
    public MqttHeader Header { get; set; } = null!;
    public Status ExecutionResult { get; set; }
    public string ExecutionMessage { get; set; }
    public string JsonObject { get; set; }
    public int JsonLenght { get; set; }
    public ObjectTypes? JsonType { get; set; }

    public MqttResponse(MqttHeader header, Status executionResult, string executionMessage, string jsonObject, int jsonLenght, ObjectTypes? jsonType)
    {
        Header = header;
        ExecutionResult = executionResult;
        ExecutionMessage = executionMessage;
        JsonObject = jsonObject;
        JsonLenght = jsonLenght;
        JsonType = jsonType;
    }

    public static MqttResponse Success(MqttHeader requestHeader, ObjectTypes type, Object data)
    {
        string jsonString = JsonSerializer.Serialize(data);
        return new MqttResponse(requestHeader, Status.OK, BaseResponses.OK, jsonString, jsonString.Count(), type);
    }

    public static MqttResponse Failure(MqttHeader requestHeader, string customMessage = BaseResponses.INTERNAL_ERROR)
    {
        return new MqttResponse(requestHeader, Status.INTERNAL_ERROR, customMessage, String.Empty, 0, null);
    }
}

using SmartWeather.Socket.Api.Hubs.MeasurePoint;

namespace SmartWeather.Socket.Api.Configuration;

public static class HubConfiguration
{
    public static void MapHubs(this WebApplication? app)
    {
        if(app == null)
        {
            throw new Exception("Application build failed");
        }

        app.MapHub<MeasurePointHub>("/" + nameof(MeasurePointHub));
    }
}

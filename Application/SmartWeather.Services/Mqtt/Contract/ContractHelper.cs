using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWeather.Services.Mqtt.Contract;

public class ContractHelper
{
    public static bool IsResponseType(int objectType)
    {
        switch ((ObjectTypes)objectType)
        {
            case ObjectTypes.CONFIG_RESPONSE:
            case ObjectTypes.ACTUATOR_RESPONSE:
                return true;
            default:
                return false;
        }
    }

    public static bool IsRequestType(int objectType)
    {
        switch ((ObjectTypes)objectType)
        {
            case ObjectTypes.CONFIG_REQUEST:
            case ObjectTypes.ACTUATOR_REQUEST:
                return true;
            default:
                return false;
        }
    }
}

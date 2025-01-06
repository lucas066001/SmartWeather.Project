#ifndef STATION_CONFIG_REQUEST_H
#define STATION_CONFIG_REQUEST_H

#include "ISerializableEntity.h"
#include "ArduinoJson.h"
#include <vector>
#include <Dtos/StationConfig/PinConfig.h>

namespace SmartWeather::Entities::Dtos
{

    class StationConfigRequest : public ISerializableEntity
    {
    public:
        String MacAddress;
        std::vector<PinConfig> ComponentsConfigs;

        void FromString(const String &jsonString) override;
        String ToString() override;
    };
}

#endif

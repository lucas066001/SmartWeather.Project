#ifndef STATION_CONFIG_RESPONSE_H
#define STATION_CONFIG_RESPONSE_H

#include <ISerializableEntity.h>
#include "ArduinoJson.h"
#include <Dtos/StationConfig/ComponentConfig.h>
#include <vector>

namespace SmartWeather::Entities::Dtos {

    class StationConfigResponse : public ISerializableEntity {
    public:
        int StationDatabaseId;
        std::vector<ComponentConfig> ConfigComponents;

        void FromString(const String& jsonString) override;
        String ToString() override;

    };
}

#endif

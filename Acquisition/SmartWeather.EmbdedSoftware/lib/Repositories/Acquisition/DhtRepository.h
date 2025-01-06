#ifndef DHT_REPOSITORY_H
#define DHT_REPOSITORY_H

#include "Arduino.h"
#include <DHT.h>
#include <ContractConstants.h>
#include <PinsConstants.h>
#include <IAcquisitionRepository.h>
#include <Mqtt/BrokerService.h>

using namespace SmartWeather::Services::Mqtt;
using namespace SmartWeather::Constants;

namespace SmartWeather::Repositories::Acquisition
{
    class DhtRepository : public IAcquisitionRepository
    {
    public:
        DhtRepository(BrokerService *brokerService);

        void Acquire() override;
        PinConfig GetConfig() override;

    private:
        BrokerService *_brokerService;
        DHT _dht;
        int _tempId = 1;
        int _humidityId = 2;
    };
}

#endif

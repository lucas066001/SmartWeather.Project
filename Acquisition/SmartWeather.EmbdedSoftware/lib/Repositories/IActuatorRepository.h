#ifndef IACTUATOR_REPOSITORY_H
#define IACTUATOR_REPOSITORY_H

#include <Dtos/StationConfig/PinConfig.h>

using namespace SmartWeather::Entities::Dtos;

namespace SmartWeather::Repositories
{
    class IActuatorRepository
    {
    public:
        virtual void Activate() = 0;
        virtual void DeActivate() = 0;
        virtual bool GetState() = 0;
        virtual int GetPin() = 0;
        virtual PinConfig GetConfig() = 0;
    };
}
#endif
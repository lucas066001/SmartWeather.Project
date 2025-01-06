#ifndef IACQUISITION_REPOSITORY_H
#define IACQUISITION_REPOSITORY_H

#include <Dtos/StationConfig/PinConfig.h>

using namespace SmartWeather::Entities::Dtos;

namespace SmartWeather::Repositories
{
    class IAcquisitionRepository
    {
    public:
        virtual void Acquire() = 0;
        virtual PinConfig GetConfig() = 0;
    };
}
#endif
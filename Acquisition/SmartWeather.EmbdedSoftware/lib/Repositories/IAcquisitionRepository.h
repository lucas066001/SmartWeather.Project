#ifndef IACQUISITION_REPOSITORY_H
#define IACQUISITION_REPOSITORY_H

#include <Dtos/StationConfig/PinConfig.h>

using namespace SmartWeather::Entities::Dtos;

namespace SmartWeather::Repositories
{
    class IAcquisitionRepository
    {
    public:
        virtual void StartAcquisition() = 0;
        virtual void StopAcquisition() = 0;
        virtual bool GetState() = 0;
        virtual PinConfig GetConfig() = 0;
    };
}
#endif